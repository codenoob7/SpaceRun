using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset;
    [SerializeField] public float followSpeed;
    [SerializeField] Transform playerPos;

    void Start()
    {
        offset = transform.position - playerPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distance = offset + playerPos.position;
        distance.x = 0f;

        transform.position = Vector3.Lerp(transform.position,distance,followSpeed * Time.deltaTime);    
    }
}
