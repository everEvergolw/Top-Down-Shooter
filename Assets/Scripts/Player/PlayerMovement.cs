using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;




public class PlayerMovement : MonoBehaviour
{



    private Player player;



    private PlayerControlls controlls;
    private Animator animator;


    [SerializeField]
    private CharacterController characterController;

    [Header("Movement Info")]
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField] private float turnSpeed;
    private float speed;
    [SerializeField]
    private float gravityScale = 9.8f;
    private float verticalVelocity;
    private bool isRunning;
    public Vector3 movementDirection;
    public Vector2 moveInput { get; private set; }











    

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        player = GetComponent<Player>();

        speed = walkSpeed;

        AssignInputEvent(); 

    }


    private void Update()
    {

        ApplyMovment();

        // Carmera
        ApplyRotation();


        AnimatorControllers();
    }





    private void AnimatorControllers()
    {
        float xVelocity  = Vector3.Dot(movementDirection.normalized, transform.right);
        float zVelocity  = Vector3.Dot(movementDirection.normalized, transform.forward);


        animator.SetFloat("xVelocity",xVelocity,.1f,Time.deltaTime); 
        animator.SetFloat("zVelocity",zVelocity,.1f, Time.deltaTime);

        bool playRunAnimation = isRunning && movementDirection.magnitude > 0;
        animator.SetBool("isRunning", playRunAnimation);

    }



    private void ApplyRotation()
    {
        
            
          Vector3 lookingDirection = player.aim.GetMouseHitInfo().point - transform.position;
          lookingDirection.y = 0f;
          lookingDirection.Normalize();
          

          Quaternion desiredRotation = Quaternion.LookRotation(lookingDirection);

          transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation,turnSpeed * Time.deltaTime);
        
    }

    private void ApplyMovment()
    {
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGravity();

        if (movementDirection.magnitude > 0)
        {
            characterController.Move(movementDirection * Time.deltaTime * speed);


        }
    }

    private void ApplyGravity()
    {
        if(characterController.isGrounded == false)
        {

            verticalVelocity = verticalVelocity - gravityScale * Time.deltaTime;

            movementDirection.y = verticalVelocity;

        }
        else
        {
            verticalVelocity = -.5f;
        }
            


    
    }


    #region New Input System
    private void AssignInputEvent()
    {
        controlls = player.controlls;



        // get access 


        controlls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        controlls.Character.Movement.canceled += context => moveInput = Vector2.zero;

      

        controlls.Character.Run.performed += context =>
        {
           
                speed = runSpeed;
                isRunning = true;
           

        };
        controlls.Character.Run.canceled += context =>
        {

            speed = walkSpeed;
            isRunning = false;


        };
    }


   

    #endregion









}
