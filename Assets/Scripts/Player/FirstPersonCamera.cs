using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float mouseSensitivity = 500.0f;

    public Transform playerBody;

    float rotationX;
    float rotationY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSensitivity;

        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}

//public class FirstPersonController : MonoBehaviour
//{
//    public float mouseSensitivity = 500.0f;
//    public Transform playerBody;

//    private float rotationX = 0f;
//    private float rotationY = 0f;

//    private void Start()
//    {
//        Cursor.lockState = CursorLockMode.Locked;
//        Cursor.visible = false;
//    }

//    private void Update()
//    {
//        // Get mouse input
//        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
//        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

//        // Rotate camera up/down
//        rotationX -= mouseY;
//        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Limit vertical rotation
//        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);

//        // Rotate player body left/right
//        rotationY += mouseX;
//        playerBody.rotation = Quaternion.Euler(0f, rotationY, 0f);
//    }
//}

//public class FirstPersonController : MonoBehaviour
//{
//    public float mouseSensitivity = 500.0f;

//    [SerializeField] Transform playerObject;

//    float rotationX;
//    float rotationY;

//    private void Start()
//    {
//        Cursor.lockState = CursorLockMode.Locked;
//        Cursor.visible = false;
//    }

//    private void Update()
//    {
//        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
//        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;

//        rotationY += mouseX;
//        rotationX -= mouseY;
//        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

//        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
//        playerObject.Rotate(Vector3.up * mouseX);
//    }
//}