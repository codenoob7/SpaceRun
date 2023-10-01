using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
   Animator anim;
   private float runAnimSpeed = 0;
   private float jumpAnimSpeed = 0;
   PlayerMovements pm;

   void Start() 
   {
        anim = GetComponent<Animator>(); 
        pm = GetComponent<PlayerMovements>();
        // anim.SetFloat("Speed",runSpeed);

        
   }

   void OnTriggerEnter(Collider other) 
   {
        if(other.gameObject.tag == "SpeedPickups")
        {
            SpeedPickups sp = other.gameObject.GetComponent<SpeedPickups>();
            runAnimSpeed += sp.increaseAnimOffset;
            
            if(runAnimSpeed > 1.1f)
            {
               return;
            }
            else
            {
               anim.SetFloat("Speed",runAnimSpeed);
            }
            
            if(runAnimSpeed > 0.6f)
            {
               jumpAnimSpeed = 1;
               anim.SetFloat("Jump",jumpAnimSpeed);
            }
            else
            {
               jumpAnimSpeed = 0;
               anim.SetFloat("Jump",jumpAnimSpeed);
            }
            
            pm.playerSpeed += sp.increaseSpeedOffset;
            Destroy(other.gameObject);
        } 
   }
}
