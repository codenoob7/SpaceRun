using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public PlayerMovements player;
    private Vector3 startPos;
    public int pixelDistToDetect = 20;
    private bool fingerDown;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(fingerDown == false && Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            fingerDown = true;
        }    

        if(fingerDown)
        {
            if(Input.mousePosition.x >= startPos.x + pixelDistToDetect)
            {
                fingerDown = false;
                // player.isDashing = true;
                // player.Move();
                // player.Move(2 * Vector3.right, "right");
                
                Debug.Log("Swipe Right");
            }
            else if(Input.mousePosition.x <= startPos.x - pixelDistToDetect)
            {
                fingerDown = false;
                // player.Move(2 * Vector3.left, "left");
                Debug.Log("Swipe Left");
            }
            else if(Input.mousePosition.y >= startPos.y + pixelDistToDetect)
            {
                fingerDown = false;
                // player.Jump();
                Debug.Log("Swipe Up");
            }

            if(fingerDown && Input.GetMouseButtonUp(0))
            {
                fingerDown = false;
            }
        }
    }
}
