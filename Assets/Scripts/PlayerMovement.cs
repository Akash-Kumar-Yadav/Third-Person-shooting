using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 20;
    public float fallSpeed = 20;

    public float InputX;
    public float InputZ;
    public Vector3 desiredMoveDirection;
    public bool bloackRotationPlayer;
    public float desiredrotationSpeed;
    public Animator anim;
    public float speed;
    public float allowPlayerRotation;
    public Camera cam;
    public CharacterController characterController;
    public bool isGrounded;
    //for player movement
    public float moveSpeed;
   

    private float verticalVel;
    private Vector3 moveVector;


   
    public ParticleSystem leftRocket;
    public ParticleSystem rightRocket;
    
    private void Start()
    {
        anim = this.GetComponent<Animator>();
        characterController = this.GetComponent<CharacterController>();
      
        cam = Camera.main;
    }

    private void Update()
    {

         InputMagnitude();

        isGrounded = characterController.isGrounded; 
        if (isGrounded)
        //{
        //    verticalVel -= 0;
        //}
        //else
        //{
        //    verticalVel -= 5 * Time.deltaTime;
        //    moveVector = new Vector3(0, verticalVel, 0);
        //    characterController.Move(moveVector);
        //}

        #region shooting

        if (Input.GetMouseButton(0) && InputX == 0 && InputZ == 0)
        {
            anim.SetBool("Shoot", true);
        }
        else
        {
            anim.SetBool("Shoot", false);
        }

        #endregion

        #region jump

        if (Input.GetKey(KeyCode.Space))
        {
            // verticalVel += jumpForce ;
            InputX = 0;
            InputZ = 0;
            leftRocket.Play();
            rightRocket.Play();
            moveVector = new Vector3(0, jumpForce * Time.deltaTime, 0);
            characterController.Move(moveVector);
            speed = new Vector2(InputX, InputZ).sqrMagnitude;
            anim.SetFloat("Speed", speed, 0.0f, Time.deltaTime);
            anim.SetBool("Run", false);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveVector = new Vector3(0, jumpForce  * Time.deltaTime, 0);
            }
        }
        else if (!Input.GetKey(KeyCode.Space  )&& !isGrounded && InputX == 0 && InputZ ==0)
        {
            moveVector = new Vector3(0,fallSpeed * Time.deltaTime, 0);
            characterController.Move(moveVector);
        }
         else
        {
            leftRocket.Stop();
            rightRocket.Stop();
           
        }

        #endregion


    }

    private void PlayerMoveAndRotation()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * InputZ + right * InputX;

        if (!bloackRotationPlayer)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredrotationSpeed);
            characterController.Move(desiredMoveDirection * Time.deltaTime * moveSpeed);
        }

        #region Running
        if (Input.GetKey(KeyCode.LeftShift))
        {
            characterController.Move(desiredMoveDirection * Time.deltaTime * moveSpeed*2);
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }
        #endregion

      
    }

    private void InputMagnitude()
    {

        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        anim.SetFloat("InputX", InputX, 0.0f, Time.deltaTime * 2);
        anim.SetFloat("InputZ", InputZ, 0.0f, Time.deltaTime * 2);

        speed = new Vector2(InputX, InputZ).sqrMagnitude;

        if (speed > allowPlayerRotation)
        {
            anim.SetFloat("Speed", speed, 0.0f, Time.deltaTime);
            PlayerMoveAndRotation();
        }
        else if(speed < allowPlayerRotation)
        {
            anim.SetFloat("Speed", speed, 0.0f, Time.deltaTime);
           
        }
    }
}
