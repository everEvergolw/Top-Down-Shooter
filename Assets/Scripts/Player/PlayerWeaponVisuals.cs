using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{

    private Player player;
    private Animator anim;
    private bool isGrabbingWeapon;

   

    [SerializeField] private WeaponModel[] weaponModels;




    [Header("Rig")]
    [SerializeField] private float rigWeightIncreaseRate;
    private bool shouldIncrease_RigWeight;
    private Rig rig;




    [Header("Left hand IK")]
    [SerializeField] private float leftHandIkWeightIncreaseRate;
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandIK_Target;
    private bool shouldIncrease_LeftHandIKWieght;


    private void Start()
    {
        player = GetComponent<Player>();

        anim = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();

        weaponModels = GetComponentsInChildren<WeaponModel>(true);


    }

    private void Update()
    {
       

        UpdateRigWigth();
        UpdateLefthHandIKWeight();

    }

    public WeaponModel CurrentWeaponModel()
    {
        WeaponModel weaponModel = null;

        WeaponType weaponType =  player.weapon.CurrentWeapon().weaponType;

       for(int i = 0; i < weaponModels.Length; i++)
        {

            if (weaponModels[i].weaponType == weaponType)
            {
                weaponModel = weaponModels[i];
            }

        }




        return weaponModel;

    }

    public void PlayReloadAnimation()
    {
        if (isGrabbingWeapon)
        {
            return;
        }

        anim.SetTrigger("Reload");
        ReduceRigWeight();
    }


    public void PlayWeaponEquipAnimation()
    {
        GrabType grabType = CurrentWeaponModel().grabType;


        leftHandIK.weight = 0;
        ReduceRigWeight();
        anim.SetFloat("WeaponGrabType", ((int)grabType));
        anim.SetTrigger("WeaponGrab");
        SetBusyGrabbingWeaponTo(true);

    }

    public void SetBusyGrabbingWeaponTo(bool busy)
    {
        isGrabbingWeapon = busy;

        anim.SetBool("BusyGrabbingWeapon", isGrabbingWeapon);
    }





    public void SwitchOnCurrentWeaponModel()
    {

        int animationIndex = (int)CurrentWeaponModel().holdType;


        SwitchAnimationLayer(animationIndex);

        CurrentWeaponModel().gameObject.SetActive(true);


        AttachLeftHand();
    }



    public void SwitchOffWeaponModels()
    {
        for (int i = 0; i < weaponModels.Length; i++)
        {

            weaponModels[i].gameObject.SetActive(false);    


        }


    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(layerIndex, 1);
    }








    private void UpdateLefthHandIKWeight()
    {
        if (shouldIncrease_LeftHandIKWieght)
        {
            leftHandIK.weight += leftHandIkWeightIncreaseRate * Time.deltaTime;

            if (leftHandIK.weight >= 1)
            {
                shouldIncrease_LeftHandIKWieght = false;
            }

        }
    }

    private void UpdateRigWigth()
    {
        if (shouldIncrease_RigWeight)
        {
            rig.weight += rigWeightIncreaseRate * Time.deltaTime;

            if (rig.weight >= 1)
            {
                shouldIncrease_RigWeight = false;
            }
        }
    }

    private void ReduceRigWeight()
    {
        rig.weight = 0.15f;
    }
    private void AttachLeftHand()
    {
        Transform targetTransform = CurrentWeaponModel().holdPoint;

        leftHandIK_Target.localPosition = targetTransform.localPosition;
        leftHandIK_Target.localRotation = targetTransform.localRotation;



    }
    public void MaximizeRigWeight() => shouldIncrease_RigWeight = true;

    public void MaximizeLeftHandWeight() => shouldIncrease_LeftHandIKWieght = true;




}
