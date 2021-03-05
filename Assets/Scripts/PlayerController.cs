using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{

    public Rigidbody2D theRB;
    public float moveSpeed;
    public float jumpForce;
    
    private float velocity;

    private bool isGrounded;
    public Transform groundCheckPoint;
    public LayerMask WhatIsGround;

    public Animator anim;

    public bool isKeyboard2;

    //In case player attack delay
    public float timeBetweenAttacks = 0.5f;
    private float attackCounter;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {

        if (isKeyboard2)
        {
            velocity = 0f;

            if (Keyboard.current.lKey.isPressed)
            {
                velocity += 1f;
            }

            if (Keyboard.current.jKey.isPressed)
            {
                velocity = -1f;
            }

            if (isGrounded && Keyboard.current.rightShiftKey.wasPressedThisFrame)
            {
                theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);

            }

            if (!isGrounded && Keyboard.current.rightShiftKey.wasPressedThisFrame && theRB.velocity.y > 0)
            {
                theRB.velocity = new Vector2(theRB.velocity.x, theRB.velocity.y * .4f);

            }

            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                   if (attackCounter <= 0)
            {
                anim.SetTrigger("attack");
                attackCounter = timeBetweenAttacks;
            }
            else
            {
                Debug.Log(attackCounter);
            }


            }
        }

        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, WhatIsGround);
        theRB.velocity = new Vector2(velocity * moveSpeed, theRB.velocity.y);

        //if (Input.GetButtonDown("Jump"))
        //{
        //    theRB.velocity = new Vector2(theRB.velocity.x, jumpForce );
        //}

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("ySpeed", theRB.velocity.y);
        anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));

        if (theRB.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (theRB.velocity.x > 0)
        {
            transform.localScale = Vector3.one;
        }

        //IF we implement this player will stand still for an amount of time when attacking
        if (attackCounter > 0)
        {
            attackCounter = attackCounter - Time.deltaTime;

            //    theRB.velocity = new Vector2(0f, theRB.velocity.y);
        }
}

    //These movements are using the package manager built in INPUT to make it possible to control multiple characters with different controllers
    public void Move(InputAction.CallbackContext context)
    {
        velocity = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
        }

        if (!isGrounded && context.canceled && theRB.velocity.y > 0f)
        {
            theRB.velocity = new Vector2(theRB.velocity.x, theRB.velocity.y * .4f);
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (attackCounter <= 0)
            {
                anim.SetTrigger("attack");
                attackCounter = timeBetweenAttacks;
            }
            else
            {
                Debug.Log(attackCounter);
            }
           

        }
    }
}
