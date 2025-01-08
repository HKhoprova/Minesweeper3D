using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

//public class PlayerMovement : MonoBehaviour
//{
//    public float moveSpeed = 10.0f;
//    public float jumpForce = 5.0f;

//    public Transform orientation;

//    float horizontalInput;
//    float verticalInput;

//    Rigidbody rb;

//    bool isGrounded;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.freezeRotation = true;
//    }

//    void Update()
//    {
//        Vector3 playerVelocity = new Vector3(horizontalInput * moveSpeed, rb.velocity.y, verticalInput * moveSpeed);
//        rb.velocity = transform.TransformDirection(playerVelocity);
//        //rb.velocity = new Vector3(horizontalInput * moveSpeed, rb.velocity.y, verticalInput * moveSpeed);
//    }

//    public void Move(InputAction.CallbackContext context)
//    {
//        horizontalInput = context.ReadValue<Vector2>().x;
//        verticalInput = context.ReadValue<Vector2>().y;
//    }

//    public void Jump(InputAction.CallbackContext context)
//    {
//        if (context.performed && isGrounded)
//        {
//            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//            isGrounded = false;
//        }
//    }

//    public void OnCollisionEnter(Collision collision) // if player hit the floor
//    {
//        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Tile")
//        {
//            isGrounded = true;
//        }
//    }

//    public void OnCollisionExit(Collision collision) // if player touch the floor
//    {
//        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Tile")
//        {
//            isGrounded = false;
//        }
//    }
//}

//public class PlayerMovement : MonoBehaviour
//{
//    public float moveSpeed = 5.0f;
//    public float jumpForce = 2.0f;
//    public Transform orientation;

//    private float horizontalInput;
//    private float verticalInput;
//    private Rigidbody rb;
//    private bool isGrounded;

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.freezeRotation = true;
//    }

//    private void Update()
//    {
//        ProcessMovement();
//    }

//    public void Move(InputAction.CallbackContext context)
//    {
//        Vector2 input = context.ReadValue<Vector2>();
//        horizontalInput = input.x;
//        verticalInput = input.y;
//    }

//    public void Jump(InputAction.CallbackContext context)
//    {
//        if (context.performed && isGrounded)
//        {
//            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//            isGrounded = false;
//        }
//    }

//    private void ProcessMovement()
//    {
//        Vector3 moveDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;
//        Vector3 force = moveDirection * moveSpeed * Time.deltaTime;

//        // Apply movement as force for smoother physics
//        rb.AddForce(force, ForceMode.VelocityChange);
//    }
//}

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float jumpForce = 5.0f;

    float horizontalInput;
    float verticalInput;

    Rigidbody rb;

    //public Transform orientation;

    //bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        Run();
        //Vector3 playerVelocity = new Vector3(horizontalInput * moveSpeed, rb.velocity.y, verticalInput * moveSpeed);
        //rb.velocity = transform.TransformDirection(playerVelocity);
        //rb.velocity = new Vector3(horizontalInput * moveSpeed, rb.velocity.y, verticalInput * moveSpeed);
    }

    void Run()
    {
        Vector3 playerVelocity = new Vector3(horizontalInput * moveSpeed, rb.velocity.y, verticalInput * moveSpeed);
        rb.velocity = transform.TransformDirection(playerVelocity);
        //horizontalInput = context.ReadValue<Vector2>().x;
        //verticalInput = context.ReadValue<Vector2>().y;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;
        verticalInput = context.ReadValue<Vector2>().y;
    }
}