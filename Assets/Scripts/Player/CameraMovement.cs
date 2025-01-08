using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerCameraPosition;
    
    void Update()
    {
        transform.position = playerCameraPosition.position;
    }
}
