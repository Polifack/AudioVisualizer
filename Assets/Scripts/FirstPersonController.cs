using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravitiy = -9.8f;
    public float jumpHeight = 30f;

    public Transform groundCheck;
    public float groundCheckDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

        if (isGrounded) Debug.Log("ground");

        if (isGrounded && velocity.y < 0) { velocity.y = -2f; }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Jump!");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravitiy);
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * x + transform.forward * z;
        controller.Move(movement*speed*Time.deltaTime);
        velocity.y += gravitiy * Time.deltaTime;
        controller.Move(velocity*Time.deltaTime);
    }
}
