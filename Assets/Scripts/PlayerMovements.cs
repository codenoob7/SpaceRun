using System.Collections;
using System.Collections.Generic;
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
    private bool isJumping;
    
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private static readonly int IsFalling = Animator.StringToHash("isFalling");
    private static readonly int IsLanding = Animator.StringToHash("isLanding");
    private static readonly int IsRolling = Animator.StringToHash("isRolling");
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int IsMovingWithoutLand = Animator.StringToHash("isMovingWithoutLand");
    
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        direction.z = playerSpeed;

        //ProcessJump();
        Jump();
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

    void Jump()
    {
        if (controller.isGrounded && Input.GetKeyDown((KeyCode.Space)))
        {
            anim.SetTrigger("IsJump");
            isRolling = false;
            direction.y = jumpForce;
        }
        else
        {
            direction.y += Gravity * Time.deltaTime;
        }

        if (!controller.isGrounded)
        {
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping",false);
        }
    }
    
    void ProcessJump()
    {
        //Jump Start
        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // direction.y = -1;
            anim.SetBool(IsJumping,true);
            anim.SetBool(IsFalling,false);
            anim.SetBool(IsLanding,false);
            anim.SetBool(IsMoving,false);
            anim.SetBool(IsRolling,false);
            anim.SetBool(IsMovingWithoutLand,false);
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
            
            anim.SetBool("isRolling",true);
        }
        else
        {
            Physics.IgnoreLayerCollision(6,7,false);
            GetComponent<SphereCollider>().enabled = false;
            
            anim.SetBool("isRolling",false);
        }


        if(!isRolling && Input.GetKeyDown(KeyCode.T))
        {
            if(!controller.isGrounded)
            {
                Debug.Log("Roll Roll!!");
                direction.y += Gravity;
            }
            else
            {
                isRolling = true;
            }
        }
    }
    
    //for the animation event to reset the isRolling bool
    private void EndRolling()
    {
        isRolling = false;
        Debug.Log("End Rolling");
    }

    void FixedUpdate()
    {   
        controller.Move(direction * Time.fixedDeltaTime);

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
