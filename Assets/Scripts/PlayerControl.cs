using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public Vector2 pos, speed, maxSpeed;
    [SerializeField] Vector2 accel;
    public float gravity;
    Rigidbody2D rb;
    public bool isGround;
    private bool jumpInput;
    private float timeSinceJumpInput;
    public int jumps;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = rb.velocity;
        maxSpeed = new Vector2(10, 15);
        isGround = false;
        gravity = 38f;
        timeSinceJumpInput = 0;
        jumps = 2;
    }

    // Update is called once per frame
    void Update()
    {
        CheckJumpInput();
    }
    private void FixedUpdate()
    {
        //Get Values (Start)
        GetValue();
        //Jump
        Gravity();
        JumpCalcs();
        //Speed Calcs
        AddSpeed();
        CheckMaxSpeed();
        SlowDownHorizontal();
        //Final
        ApplyValues();
    }

    //Fist
    private void GetValue()
    {
        speed = rb.velocity;
    }
    //Final
    private void ApplyValues()
    {
        rb.velocity = speed;
    }

    private void AddSpeed()
    {
        //Left
        if (Input.GetKey(KeyCode.A) && speed.x > 0) //If going opposite direciton
        {
            speed.x -= accel.x * Time.deltaTime * 3;
        }else if (Input.GetKey(KeyCode.A)) {
            speed.x -= accel.x * Time.deltaTime;
        }
        //Right
        if (Input.GetKey(KeyCode.D) && speed.x < 0) //If going opposite direciton
        {
            speed.x += accel.x * Time.deltaTime * 3;
        } else if (Input.GetKey(KeyCode.D)) {
            speed.x += accel.x * Time.deltaTime;
        }

    }

    private void CheckMaxSpeed()
    {
        if (speed.x > maxSpeed.x)
        {
            speed.x = maxSpeed.x;
        } else if (speed.x < -maxSpeed.x)
        {
            speed.x = -maxSpeed.x;
        }
    }

    private void SlowDownHorizontal()
    {
        if (!IsDirectionalKeyPressed())
        {
            Vector2 unmodifiedSpeed = new Vector2(speed.x, speed.y);
            //Going Right
            if (speed.x > 0)
            {
                speed.x -= accel.x * 2 * Time.deltaTime;
            }
            //Going Left
            else if (speed.x < 0)
            {
                speed.x += accel.x * 2 * Time.deltaTime;
            }
            //Speed should be set to 0 when speed goes from positive to negative or viceversa
            if ((unmodifiedSpeed.x < 0 && speed.x > 0) || (unmodifiedSpeed.x > 0 && speed.x < 0)) 
            {
                speed.x = 0;
            }
        } else
        {

        }
    }

    private bool IsDirectionalKeyPressed()
    {
        return (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S));
    }

    private void CheckJumpInput()
    {
        if (!jumpInput && Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true;
            timeSinceJumpInput = 0;
        }
    }

    private void JumpCalcs()
    {
        //Timer
        if (timeSinceJumpInput > 0.2f)
        {
            jumpInput = false;
            return;
        }
        //Normal Jump
        if (isGround && jumpInput)
        {
            speed.y = 15;
            jumpInput = false;
            jumps--;
        //Double Jump
        } else if (!isGround && jumpInput && jumps != 0)
        {
            speed.y = 15;
            jumpInput = false;
            jumps--;
        }

        if (!isGround && jumpInput)
        {
            timeSinceJumpInput += Time.deltaTime;
        }
    }

    private void Gravity()
    {
        if (!isGround)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                speed.y += -gravity * Time.deltaTime;
            } else
            {
                speed.y += -gravity * 2 * Time.deltaTime;
            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Get points and compare if ground or smth else
        if (collision.gameObject.CompareTag("SolidGround"))
        {
            isGround = true;
            jumps = 2;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SolidGround"))
        {
            isGround = false;
        }
    }

}
