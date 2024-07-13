using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{




    public PlayerControlls controlls { get; private set; }
    public PlayerAim aim { get; private set; } // read-only
    public PlayerMovement movement { get; private set; }

    public PlayerWeaponController weapon { get; private set; }
    public PlayerWeaponVisuals weaponVisuals { get; private set; }


    private void Awake()
    {
        controlls =  new PlayerControlls();
        aim = GetComponent<PlayerAim>();
        movement = GetComponent<PlayerMovement>();
        weapon = GetComponent<PlayerWeaponController>();
        weaponVisuals = GetComponent<PlayerWeaponVisuals>();
    }

    private void OnEnable()
    {
        controlls.Enable();
    }


    private void OnDisable()
    {
        controlls.Disable();
    }

}

