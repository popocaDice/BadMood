using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    private Animator anim;
    private Rigidbody2D rb2d;

    private static readonly int UP = 0;
    private static readonly int LEFT = -1;
    private static readonly int RIGHT = 1;
    private static readonly int NONE = 69;

    public int dashesMax;
    public float runSpeed;
    public float dashCooldown;
    public float inputCooldown;
    public float jumpTime;
    public float jumpForce;

    private int lastInput = NONE;
    private float dashCooldownCount;
    private float jumpTimeCounter;

    private bool jump = false;
    private bool crouch = false;
    private int dashesLeft = 0;
    private float horizontalMove = 0f;
    private float inputCooldownCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
        dashesLeft = dashesMax;
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void ground()
    {
        jumpTimeCounter = jumpTime;
    }

    private void Update()
    {
        inputCooldownCount = (inputCooldownCount>0)?inputCooldownCount - Time.deltaTime:0;
        dashCooldownCount = (dashCooldownCount>0)?dashCooldownCount - Time.deltaTime:0;

        horizontalMove = Input.GetAxis("Horizontal") * runSpeed;
        anim.SetBool("Walk", horizontalMove != 0);
        anim.SetFloat("WalkSpeed", Mathf.Abs(horizontalMove)/30 + 0.2f);
        
        if (Input.GetButtonDown("Jump"))
        {

            jump = true;
        }
        if (Input.GetButton("Crouch"))
        {
            
            crouch = true;
        }
        else
        {

            crouch = false;
        }

        if (Input.GetButton("Jump"))
        {
            if (jumpTimeCounter > 0)
            {
                rb2d.AddForce(Vector2.up * jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
        }
        else
        {
            jump = false;
            jumpTimeCounter = 0f;
        }
        
        if (inputCooldownCount > 0)
        {

            inputCooldownCount -= Time.deltaTime;
        }
        else
        {
            inputCooldownCount = 0;
            if (lastInput != NONE)
            {

                lastInput = NONE;
            }
        }

        if (dashCooldownCount > 0 && dashesLeft < dashesMax)
        {

            dashCooldownCount -= Time.deltaTime;
        }else if (dashesLeft < dashesMax)
        {

            dashesLeft++;
            dashCooldownCount = dashCooldown;
        }

        if (Input.GetButtonUp("Jump"))
        {
            inputCountdown();
            if (lastInput == UP)
            {

                Dash(Vector2.up);
            }
            lastInput = UP;
        }
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {

            inputCountdown();
            if (lastInput == RIGHT)
            {

                Dash(Vector2.right);
            }
            lastInput = RIGHT;
        }
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {

            inputCountdown();
            if (lastInput == LEFT)
            {

                Dash(Vector2.left);
            }
            lastInput = LEFT;
        }

        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, false);
        anim.SetBool("Jump", jump);
    }

    private void inputCountdown()
    {

        inputCooldownCount = inputCooldown;
    }

    public void Jump()
    {
        controller.Move(0, false, true);
    }

    public void Dash(Vector2 dir)
    {
        if (dashesLeft > 0)
        {
            afterImage();
            dashCooldownCount = dashCooldown;
            lastInput = NONE;
            anim.SetBool("Dash", true);
            rb2d.velocity = rb2d.velocity * Vector2.right;
            rb2d.AddForce(dir * jumpForce * 25);
            dashesLeft--;
        }
    }

    public void afterImage()
    {

        foreach (ParticleSystem particle in GetComponentsInChildren<ParticleSystem>())
        {
            particle.Play();
        }
    }
}
