using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{

    private Player player;


    [SerializeField] private Weapon currentWeapon;


    [Header("Bullet details")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpped;
    [SerializeField] private Transform gunPoint;

    private const float REFERENCE_BULLET_SPEED = 20f;
    // this is the default speed 

    [SerializeField] private Transform weaponHolder;



    [Header("Inventory")]
    [SerializeField]private int maxSlots = 2;

    [SerializeField] private List<Weapon> weaponSlots;    



    private void Start()
    {
        player = GetComponent<Player>();

        AssignInputEvents();

        currentWeapon.bulletInMagazine = currentWeapon.totalReserveAmmo;
    }





    public Weapon CurrentWeapon() => currentWeapon;

    private void EquipWeapon(int i)
    {
        currentWeapon = weaponSlots[i];

        player.weaponVisuals.SwitchOffWeaponModels();

        player.weaponVisuals.PlayWeaponEquipAnimation();
        
    }

    public void PickupWeapon(Weapon newWeapon)
    {

        if(weaponSlots.Count > maxSlots)
        {
            return;
        }

        weaponSlots.Add(newWeapon);


    }

    private void DropWeapon()
    {
        if (weaponSlots.Count <= 1)
        {
            return;            
        }

        weaponSlots.Remove(currentWeapon);
        currentWeapon = weaponSlots[0];
    }






    private void Shoot() 
    {

        if(currentWeapon.CanShoot() == false) {
            return;
        }

        GameObject newBullet = Instantiate(bulletPrefab,gunPoint.position, Quaternion.LookRotation(gunPoint.forward));

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();


        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpped;

        rbNewBullet.velocity = BulletDirection() * bulletSpped ;



        Destroy(newBullet, 10);

        GetComponentInChildren<Animator>().SetTrigger("Fire");
    
            
    }


    public Vector3 BulletDirection()
    {

        Transform aim = player.aim.Aim();

        Vector3 direction = (aim.position - gunPoint.position).normalized;

        if (player.aim.CanAimPrecisly() == false && player.aim.Target() == null)
        {
            direction.y = 0;

        }


        weaponHolder.LookAt(aim);
        gunPoint.LookAt(aim);


        return direction;
    }


   
    public Transform GunPoint() => gunPoint;

     
    private void AssignInputEvents()
    {
        PlayerControlls controlls = player.controlls;

        controlls.Character.Fire.performed += context => Shoot();
        controlls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controlls.Character.EquipSlot2.performed += context => EquipWeapon(1);
        controlls.Character.DropCurrentWeapon.performed += context => DropWeapon();

        controlls.Character.Reload.performed += context =>
        {
            if (currentWeapon.CanReload())
            {
                player.weaponVisuals.PlayReloadAnimation();

            }


        };


    }

 
}
