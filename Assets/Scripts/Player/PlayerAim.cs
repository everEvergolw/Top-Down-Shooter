using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{

    private Player player;
    private PlayerControlls controlls;
    
    [Header("Aim Viual - Laser")]
    [SerializeField] private LineRenderer aimLaser;


    [Header("Aim control")]
    [SerializeField] private Transform aim;

    [SerializeField] private bool isAimingPrecisly;
    [SerializeField] private bool isLockingToTarget; 

    [Header("Camera control")]
    [Range(0.5f,1)]
    [SerializeField] private float minCameraDistance = 1.5f;
    [Range(1f, 3f)]
    [SerializeField] private float maxCameraDistance = 4f;
    [Range(3f, 5f)]
    [SerializeField] private float cameraSensetivity = 5f;

    [SerializeField]
    private LayerMask aimLayerMask;
    [SerializeField] private Transform cameraTarget;




    private Vector2 mouseInput;
    private RaycastHit lastKnownMouseHit;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
    }


    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.P))
        {
            isAimingPrecisly = !isAimingPrecisly;   

        }

        if(Input.GetKeyDown(KeyCode.L)) {
            isLockingToTarget = !isLockingToTarget;

        }


        UpdateAimVisuals();

        UpdateAimPosition();

        UpdateCameraPosition();
    }

    private void UpdateAimVisuals()
    {

        Transform gunPoint = player.weapon.GunPoint();
        Vector3 laserDirection = player.weapon.BulletDirection();

        float laserTipLength = .5f;
        float gunDistance = 4f;


        Vector3 endPoint = gunPoint.position + laserDirection * gunDistance;

        if(Physics.Raycast(gunPoint.position, laserDirection, out RaycastHit hit, gunDistance))
        {
            endPoint = hit.point;
            laserTipLength = 0;
        }

        aimLaser.SetPosition(0, gunPoint.position);

        aimLaser.SetPosition(1, endPoint);

        aimLaser.SetPosition(2, endPoint+ laserDirection * laserTipLength);


    }


    private void UpdateAimPosition()
    {


        Transform target = Target();    

        if(target != null && isLockingToTarget) {

            if(target.GetComponent<Renderer>() != null)
            {
                aim.position = target.GetComponent<Renderer>().bounds.center;
            }
            else
            {
                aim.position = target.position;

            }

            return;
        
        }

        aim.position = GetMouseHitInfo().point;

        if (!isAimingPrecisly)
        {
            aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z);
        }
    }


    public Transform Aim() => aim;
    public Transform Target()
    {
        Transform target = null;

        if(GetMouseHitInfo().transform.GetComponent<Target>() != null) {
            target = GetMouseHitInfo().transform;
        }
        return target;
    }



    public bool CanAimPrecisly() => isAimingPrecisly;
  


    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);

        if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {

            lastKnownMouseHit = hitInfo;
            return hitInfo;


        }

        return lastKnownMouseHit; 

    }


    #region Camera Region

    private Vector3 DesieredCameraPosition()
    {

      

        float actualMaxCareraDistance = player.movement.moveInput.y < -0.5f ? minCameraDistance : maxCameraDistance;



        Vector3 desiredCameraPosition = GetMouseHitInfo().point;

        Vector3 aimDierction = (desiredCameraPosition - transform.position).normalized;

        float distanceToDesierdPosition = Vector3.Distance(transform.position, desiredCameraPosition);
        float clampedDistance = Mathf.Clamp(distanceToDesierdPosition, minCameraDistance, actualMaxCareraDistance);


      
        
        desiredCameraPosition = transform.position + aimDierction * clampedDistance;

        

        desiredCameraPosition.y = transform.position.y + 1;


        return desiredCameraPosition;
    }

    private void UpdateCameraPosition()
    {
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesieredCameraPosition(), cameraSensetivity * Time.deltaTime);
    }

    #endregion
    private void AssignInputEvents()
    {
        controlls = player.controlls;
        controlls.Character.Aim.performed += context => mouseInput = context.ReadValue<Vector2>();
        controlls.Character.Aim.canceled += context => mouseInput = Vector2.zero;
    }

}
