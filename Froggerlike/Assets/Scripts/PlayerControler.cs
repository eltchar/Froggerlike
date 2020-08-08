using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    //Movement variables
    private Vector3 movePoint;
    private Vector3 prevPoint;
    private Vector3 startingPos;
    private float moveSpeed = 25f;
    private float waterSpeed = 5f;
    private bool waterDirection = true;
    private Rigidbody2D playerRb;
    //Collison variables
    private LayerMask boundaryMask;
    // Water movement
    private bool inWater;
    private int onWaterEntity;
    //other variables
    private bool respawnTimerEnabled = false;
    private float respawnTime = 0.5f;
    
    void Start()
    {
        movePoint = new Vector3(0f, 0f, 0f);
        prevPoint = transform.position;
        boundaryMask = LayerMask.GetMask("Boundary");
        startingPos = new Vector3(0.5f, -6.5f, 0f);
        GameManagerScript.instance.OnDeathEvent += PlayerDeath;
        playerRb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        WaterCheck();
        HandleMovement();
        RespawnTimerCount();
        if (onWaterEntity > 0)
        {
            HandleWaterDrag(waterSpeed, waterDirection);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Vector3 tempPoint = transform.position;
            tempPoint += new Vector3(1f, 0f, 0f);
            print(Time.deltaTime);
            print(5*Time.deltaTime);
            print(tempPoint);
            print(transform.position);
            print(Vector3.MoveTowards(transform.position, tempPoint, 5f* Time.deltaTime));
            transform.position = Vector3.MoveTowards(transform.position, tempPoint, 5f * Time.deltaTime);
        }
    }
    // checking if frog is on water entity or should fall in water 
    private void WaterCheck()
    {
        if (inWater)
        {
            if (onWaterEntity == 0)
            {
                GameManagerScript.instance.DeathEvent(this, EventArgs.Empty);
            }
        }
    }
    // reset player position and player oriented variables on death
    private void PlayerDeath(object sender, EventArgs e)
    {
        if (GameManagerScript.instance.liveCount > 0)
        {
            transform.position = startingPos;
            movePoint = startingPos;
            inWater = false;
            onWaterEntity = 0;
            respawnTimerEnabled = true;
        }
        else
        {
            print("no more lives!");
            GameManagerScript.instance.MoveToMainMenu();
        }
    }

    //small counter for respawn timer to prevent multiple deaths by chained input
    private void RespawnTimerCount()
    {
        if (respawnTimerEnabled)
        {
            if (respawnTime > 0f)
            {
                respawnTime -= Time.deltaTime;
            }
            else
            {
                respawnTimerEnabled = false;
                respawnTime = 0.5f;
            }
        }
        
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if in water set flag to check for possible death condition
        if (collision.gameObject.layer==10) 
        {
            inWater = true;
        }
        // // if off water entity increase counter of on how many water entities currently on
        if (collision.gameObject.layer == 11)
        {
            onWaterEntity += 1;
        }
        // if collided with car kill player
        if (collision.gameObject.layer == 12)
        {
            GameManagerScript.instance.DeathEvent(this, EventArgs.Empty);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            waterSpeed = collision.gameObject.GetComponent<WaterEntityController>().GetSpeed();
            waterDirection = collision.gameObject.GetComponent<WaterEntityController>().GetDirection();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // if off water
        if (collision.gameObject.layer == 10)
        {
            inWater = false;
        }
        // if off water entity decrease counter of on how many water entities currently on
        if (collision.gameObject.layer == 11)
        {
            if (onWaterEntity>0)
            {
                onWaterEntity -= 1;
            }
            else
            {
                onWaterEntity = 0;
            }
            
        }
    }
    private void HandleWaterDrag(float speed, bool direction)
    {
        Vector3 tempPoint = new Vector3(0f, 0f, 0f);
        if (direction)
        {
            tempPoint = new Vector3(20f, transform.position.y, transform.position.z);
        }
        else
        {
            tempPoint = new Vector3(-20f, transform.position.y, transform.position.z);
        }
        transform.position = Vector3.MoveTowards(transform.position, tempPoint, speed * Time.deltaTime);
    }

   /* private void HandleMovement()
    {
        //move player one square 
        transform.position = Vector3.MoveTowards(transform.position, movePoint, moveSpeed * Time.deltaTime);
        //if player not respawning read input and determine direction of movement
        if (!respawnTimerEnabled)
        {
            if (Vector3.Distance(transform.position, movePoint) <= .05f)
            {
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    if (!Physics2D.OverlapCircle(movePoint + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .1f, boundaryMask))
                    {
                        movePoint += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    }

                }
                else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    if (!Physics2D.OverlapCircle(movePoint + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .1f, boundaryMask))
                    {
                        movePoint += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                    }
                }
            }
        }
        
    }*/

    private void HandleMovement()
    {
        //if player not respawning read input and determine direction of movement
        if (!respawnTimerEnabled)
        {
            //moving player
            playerRb.MovePosition(transform.position + (movePoint * moveSpeed * Time.deltaTime));
            //check if player finsihed movement to next grid spot
            if (Vector3.Distance(transform.position, prevPoint) <= .05f)
            {
                movePoint = new Vector3(0f, 0f, 0f);
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    movePoint = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    prevPoint = transform.position + movePoint;

                }
                else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    movePoint = new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                    prevPoint = transform.position + movePoint;
                }
            }
        }
        
    }
    private void OnDestroy()
    {
        GameManagerScript.instance.OnDeathEvent -= PlayerDeath;
    }
}
