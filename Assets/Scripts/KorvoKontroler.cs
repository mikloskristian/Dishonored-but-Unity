using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KorvoKontroler : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 3.0f;
    public static bool IsInputEnabled = true;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    float jumpCount = 0;
    bool canJump;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Fire3"))
        {
            speed = 17.0f;
        }
        if (Input.GetButtonUp("Fire3"))
        {
            speed = 12.0f;
        }



        if (Input.GetButtonDown("Jump"))
        {
            jumpCount = jumpCount + 1;
            Jump();
        }

        if (jumpCount == 1)
        {
            canJump = false;
        }

        if (isGrounded)
        {
            jumpCount = 0;
            canJump = true;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Fire1"))
        {
            var newScale = transform.localScale;
            newScale.y = 0.4f;
            transform.localScale = newScale;
            speed = speed / 2;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            var newScale = transform.localScale;
            newScale.y = 1f;
            transform.localScale = newScale;
            speed = speed * 2;
        }
    }

    void Jump()
    {
        if (canJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
