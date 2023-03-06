using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public Vector2 pos, speed, maxSpeed;
    [SerializeField] Vector2 accel;
    public float gravity, coyoteTime, timeSinceJumpInput, timeSinceTouchGround;
    Rigidbody2D rb;
    public bool isGround, isWall;
    private bool jumpInput, isExtraJumpLowered;
    public int defaultJumps,jumps;
    [SerializeField] Animator animator;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = rb.velocity;
        maxSpeed = new Vector2(10, 15);
        isGround = false;
        gravity = 38f;
        timeSinceJumpInput = 0;
        //set jumps
        if (PlayerPrefs.GetInt("jumpAmount") == 0)
        {
            defaultJumps = 2; //Default
        } else
        {
            defaultJumps = PlayerPrefs.GetInt("jumpAmount");
        }
        jumps = defaultJumps;
        coyoteTime = 0.12f;
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
        CheckGround();
        //Jump
        Gravity();
        JumpCalcs();
        //Speed Calcs
        AddSpeed();
        CheckMaxSpeed();
        SlowDownHorizontal();
        //Final
        AnimatorVars();
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true;
            timeSinceJumpInput = 0;
        }
    }

    private void JumpCalcs()
    {
        //You were doing timeSinceJumpInput, should be time SINCE TOUCH GROUND

        //Timer
        if ((timeSinceTouchGround > coyoteTime) && jumps > 1 && !isGround)
        {
            isExtraJumpLowered = true; //Change this var name makes no sense
        }

        //Extra time for jump
        if (!isGround && (timeSinceTouchGround <= coyoteTime) && jumpInput && jumps > 0 && !isExtraJumpLowered)
        {
            speed.y = 15;
            jumpInput = false;
            jumps--;
            Debug.Log("--Extra Jump");
            isExtraJumpLowered = true;
        } //Normal Jump
        else if (isGround && jumpInput)
        {
            speed.y = 15;
            jumpInput = false;
            jumps--;
            Debug.Log("--Normal Jump");

        } //Double Jump
        else if (!isGround && jumpInput && jumps != 0)
        {
            speed.y = 15;
            jumpInput = false;
            jumps--;
            Debug.Log("--Double Jump");
        }

        timeSinceJumpInput += Time.deltaTime;
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

    private void CheckGround()
    {
        //Get the box
        BoxCollider2D bc = transform.Find("GroundBox").gameObject.GetComponent<BoxCollider2D>();
        //Top left
        Vector2 startPoint = new Vector2(bc.bounds.center.x - bc.bounds.extents.x, bc.bounds.center.y + bc.bounds.extents.y);
        Vector2 endPoint = new Vector2(bc.bounds.center.x + bc.bounds.extents.x, bc.bounds.center.y - bc.bounds.extents.y);
        Debug.DrawLine(startPoint, endPoint, Color.red);

        bool collidedGround = false;
        foreach (Collider2D coll in Physics2D.OverlapAreaAll(startPoint, endPoint))
        {
            if (coll.gameObject.CompareTag("SolidGround"))
            {
                collidedGround = true;
                jumps = defaultJumps;
                isGround = true;
                timeSinceTouchGround = 0;
                isExtraJumpLowered = false;
            }
        }
        //if not a single collision
        if (!collidedGround)
        {
            isGround = false;
            timeSinceTouchGround += Time.deltaTime;
        }
    }

    private void AnimatorVars()
    {
        animator.SetBool("moveSide", (speed.x > 0 || speed.x < 0));
        animator.SetBool("standStill", speed.x == 0);
        animator.SetBool("isJumping", (!isGround && speed.y > 0));
        animator.SetBool("isGround", isGround);
        animator.SetBool("isFalling", speed.y < 0);
        //Animator jumps is jumps made
        animator.SetInteger("jumps", (defaultJumps - jumps));

        //Flip the Sprite depending on direction
        if (speed.x > 0)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = false;
        } else if (speed.x < 0)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = true;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

}
