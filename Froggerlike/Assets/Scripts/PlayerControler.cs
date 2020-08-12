using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    //Movement variables
    private Vector3 startingPos;
    private Quaternion startingRot;
    private float moveSpeed = 3f;
    private Rigidbody2D playerRb;
    public bool isDragged = false;
    private bool isOnSinking = false;
    private bool isOnStable = false;
    private Animator playerAnimator;
    // Water movement
    private bool inWater;
    private int onWaterEntity;
    private float waterTime = 0.1f;
    //other variables
    private bool respawnTimerEnabled = false;
    private float respawnTime = 0.5f;
    
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        startingPos = new Vector3(0.5f, -6.5f, 0f);
        startingRot = transform.rotation;
        GameManagerScript.instance.OnDeathEvent += PlayerDeath;
        GameManagerScript.instance.OnSuccessEvent += PlayerSuccess;
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        RespawnTimerCount();
    }
    private void LateUpdate()
    {
        WaterCheck();
    }
    // checking if frog is on water entity or should fall in water 
    private void WaterCheck()
    {
        if (inWater)
        {
            if (waterTime<0)
            {
                if (onWaterEntity == 0)
                {
                    GameManagerScript.instance.DeathEvent(this, EventArgs.Empty);
                    AudioManager.instance.Play("WaterSplashSFX");
                }
                else
                {
                    waterTime = 0.1f;
                }
            }
            else if (isOnSinking && !isOnStable)
            {
                GameManagerScript.instance.DeathEvent(this, EventArgs.Empty);
                AudioManager.instance.Play("WaterSplashSFX");

            }
            else
            {
                waterTime -= Time.deltaTime;
                isOnStable = false;
                isOnSinking = false;
            }
        }
        else
        {
            waterTime = 0.1f;
        }
    }
    // reset player position and player oriented variables on death
    private void PlayerDeath(object sender, EventArgs e)
    {
        playerRb.velocity = new Vector2(0f, 0f);
        transform.position = startingPos;
        transform.rotation = startingRot;
        inWater = false;
        isDragged = false;
        isOnSinking = false;
        isOnStable = false;
        onWaterEntity = 0;
        respawnTimerEnabled = true;
        playerAnimator.SetBool("Moving", false);
        playerAnimator.SetInteger("Direction", 0);
    }

    private void PlayerSuccess(object sender, EventArgs e)
    {
        playerRb.velocity = new Vector2(0f, 0f);
        transform.position = startingPos;
        transform.rotation = startingRot;
        inWater = false;
        isOnSinking = false;
        isOnStable = false;
        isDragged = false;
        onWaterEntity = 0;
        respawnTimerEnabled = true;
        playerAnimator.SetBool("Moving", false);
        playerAnimator.SetInteger("Direction", 0);
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
            AudioManager.instance.Play("CarHitSFX");
        }
        // if on the other side trigger one success event
        if (collision.gameObject.layer == 14)
        {
            GameManagerScript.instance.SuccessEvent(this, EventArgs.Empty);
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            if (collision.gameObject.GetComponent<WaterEntityController>().isSunken)
            {
                isOnSinking = true;
            }
            else
            {
                isOnStable = true;
            }

        }
    }

    private void HandleMovement()
    {
        //if player not respawning read input and determine direction of movement
        if (!respawnTimerEnabled)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f && Mathf.Abs(Input.GetAxisRaw("Vertical")) == 0f)
            {
                playerRb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, 0f);
                playerAnimator.SetBool("Moving", true);
                playerAnimator.SetInteger("Direction", 2);
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f && Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 0f)
            {
                playerRb.velocity = new Vector2(0f, Input.GetAxisRaw("Vertical") * moveSpeed);
                playerAnimator.SetBool("Moving", true);
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    playerAnimator.SetInteger("Direction", 0);
                }
                else
                {
                    playerAnimator.SetInteger("Direction", 1);
                }
            }
            else
            {
                playerRb.velocity = new Vector2(0f, 0f);
                playerAnimator.SetBool("Moving", false);
            }
        }

    }
    private void OnDestroy()
    {
        GameManagerScript.instance.OnDeathEvent -= PlayerDeath;
        GameManagerScript.instance.OnSuccessEvent -= PlayerSuccess;
    }
}
