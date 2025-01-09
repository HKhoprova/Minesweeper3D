using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform cameraPosition;
        
    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
