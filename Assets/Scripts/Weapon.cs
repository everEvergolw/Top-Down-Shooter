public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle


}






[System.Serializable] // make class visible in inspector.
public class Weapon 
{


    public WeaponType weaponType;

    public int bulletInMagazine;

    public int magazineCapacity;

    public int totalReserveAmmo;



    public bool CanShoot()
    {
        return HaveEnoughBullets();

    }




    private bool HaveEnoughBullets()
    {
        if (bulletInMagazine > 0)
        {
            bulletInMagazine--;
            return true;
        }
        else
        {
            return false;
        }
    }


    public bool CanReload()
    {
        if(bulletInMagazine == magazineCapacity) {
            return false;
        }

        if(totalReserveAmmo > 0)
        {

            return true;
        }
        return false;
    }

    public void RefillBullets()
    {

       // totalReserveAmmo += bulletInMagazine; // this will return bullest in magazine to total amount of bullets

        int bulletsToReload = magazineCapacity;

        if(bulletsToReload > totalReserveAmmo)
        {
            bulletsToReload = totalReserveAmmo;

        }

        totalReserveAmmo -= bulletsToReload;

        bulletInMagazine = bulletsToReload;


        if(totalReserveAmmo < 0)
        {
            totalReserveAmmo = 0;   
        }


    }
}

