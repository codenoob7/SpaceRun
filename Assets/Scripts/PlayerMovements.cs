using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] public float playerSpeed;
    [SerializeField] float slideSpeed;
    [SerializeField] float jumpForce;
    public float Gravity = -20;
    private CharacterController controller;
    private Vector3 direction;
    private int desiredLane = 1;  // 0 is left lane , 1 is middle lane , 2 is right lane
    public float laneDistance = 4;
    private Animator anim;
    private bool isRolling;

    private SplineFollower _splineFollower;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        _splineFollower = GetComponentInParent<SplineFollower>();
    }

    void Update()
    {
        direction.z = playerSpeed;

        ProcessJump();
        ProcessRoll();

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Right");
            //anim.SetTrigger("RightDash");
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Left");
            //anim.SetTrigger("LeftDash");
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }

        // Jog();

        // Run();
    }

    void ProcessJump()
    {
        //Jump Start
        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // direction.y = -1;
            anim.SetBool("isJumping",true);
            anim.SetBool("isFalling",false);
            anim.SetBool("isLanding",false);
            anim.SetBool("isMoving",false);
            anim.SetBool("isRolling",false);
            anim.SetBool("isMovingWithoutLand",false);
            direction.y = jumpForce;
        }
        //Back to Movement
        else
        {
            anim.SetBool("isMoving",true);
            anim.SetBool("isJumping",false);
            anim.SetBool("isFalling",false);
            anim.SetBool("isLanding",false);
            anim.SetBool("isRolling",false);
            direction.y += Gravity * Time.deltaTime;
            // direction.y = -1;
        }
        //Jump Loop in Air
        if(transform.position.y > 1f)
        {
            anim.SetBool("isFalling",true);
            anim.SetBool("isMoving",false);
            anim.SetBool("isJumping",false);
            anim.SetBool("isLanding",false);
            anim.SetBool("isRolling",false);
            anim.SetBool("isMovingWithoutLand",false);
        }
        //Landing from jump
        if(transform.position.y < 1f && !controller.isGrounded)
        {
             if(playerSpeed < 11f)
            {
                // anim.SetBool("isLanding",false);
                anim.SetBool("isMovingWithoutLand",true);
            }
            else
            {
                anim.SetBool("isLanding",true);
                anim.SetBool("isFalling",false);
                anim.SetBool("isMoving",false);
                anim.SetBool("isJumping",false);
                anim.SetBool("isRolling",false);
            }
        }
    }

    void ProcessRoll()
    {
        if(isRolling)
        {  
            Physics.IgnoreLayerCollision(6,7,true);
            GetComponent<SphereCollider>().enabled = true;
        }
        else
        {
            Physics.IgnoreLayerCollision(6,7,false);
            GetComponent<SphereCollider>().enabled = false;
        }


        if(Input.GetKeyDown(KeyCode.T))
        {
            if(!controller.isGrounded)
            {
                Debug.Log("Roll Roll!!");
                direction.y += Gravity;
            }
            else
            {
                isRolling = true;
                anim.SetBool("isRolling",true);
            }
        }
    }
    
    //for the animation event to reset the isRolling bool
    public void EndRolling()
    {
        isRolling = false;
    }

    void FixedUpdate()
    {   
        //controller.Move(direction * Time.fixedDeltaTime);

         Vector3 targetPos = transform.position.z * transform.forward + transform.position.y * transform.up;
        
        if(desiredLane == 0)
        {
            targetPos += Vector3.left * laneDistance;
        }
        else if(desiredLane == 2)
        {
            targetPos += Vector3.right * laneDistance;
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, slideSpeed * Time.deltaTime);
    }
}
