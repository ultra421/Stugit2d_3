using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public Vector2 pos, speed, accel, maxSpeed;
    public float gravity;
    public bool isAir, isGround, isWall;
    Collider2D coll;
    

    // Start is called before the first frame update
    void Start()
    {
        ReadDefaultValues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * Reads the values from the config file
     * TODO: Change this to actually read the config file
     */
    private void ReadDefaultValues()
    {
        pos = transform.position;
        speed = new Vector2(0, 0);
        accel = new Vector2(0, 0);
        maxSpeed = new Vector2(5, 5);
        gravity = 9.81f;
        isAir = false;
        isGround = false;
        isWall = false;

        coll = GetComponent<Collider2D>();
    }

    private void GravityCalculation()
    {

    }

}
