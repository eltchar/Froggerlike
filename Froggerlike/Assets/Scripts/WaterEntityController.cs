using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEntityController : MonoBehaviour
{
    //base variables
    private float moveSpeed;
    private int entityDirection;
    private float sinkingTime = 3f;
    public bool isSunken=false;
    [SerializeField] private int entityType = 0;
    [SerializeField] private bool isSinkingType=false;
    private Rigidbody2D entityRb;
    private Animator entityAnimator;

    private void Start()
    {
        entityRb = GetComponent<Rigidbody2D>();
        SetEntityType(entityType);
        if (isSinkingType)
        {
            entityAnimator = GetComponent<Animator>();
        }
    }
    private void Update()
    {
        if (isSinkingType)
        {
            //check how much time before object sinks/resurface
            if (sinkingTime > 0f)
            {
                // if object is about to sink or resurface play animation
                if (sinkingTime < 1 || sinkingTime > 4.75f)
                {
                    entityAnimator.SetBool("Sinking", true);
                }
                else
                {
                    entityAnimator.SetBool("Sinking", false);
                }
                sinkingTime -= Time.deltaTime;
            }
            //if time is out sink/resurface object
            else
            {
                if (isSunken)
                {
                    sinkingTime = 5f;
                    isSunken = false;
                    SinkEntity(isSunken);
                }
                else
                {
                    sinkingTime = 2f;
                    isSunken = true;
                    SinkEntity(isSunken);
                }
            }
        }
    }
    // setting up variables depending on the type of entity sorted by what lane they start at
    public void SetEntityType(int Type)
    {
        switch (Type)
        {
            case 1:
                entityDirection = 1;
                moveSpeed = 2f * GameManagerScript.instance.difficultyFactor;
                entityRb.velocity = new Vector2(moveSpeed * entityDirection, 0f);
                break;
            case 2:
                entityDirection = -1;
                moveSpeed = 3f * GameManagerScript.instance.difficultyFactor;
                entityRb.velocity = new Vector2(moveSpeed * entityDirection, 0f);
                break;
            case 3:
                entityDirection = -1;
                moveSpeed = 1f * GameManagerScript.instance.difficultyFactor;
                entityRb.velocity = new Vector2(moveSpeed * entityDirection, 0f);
                break;
            case 4:
                entityDirection = 1;
                moveSpeed = 2f * GameManagerScript.instance.difficultyFactor;
                entityRb.velocity = new Vector2(moveSpeed * entityDirection, 0f);
                break;
            case 5:
                entityDirection = -1;
                moveSpeed = 1.5f * GameManagerScript.instance.difficultyFactor;
                entityRb.velocity = new Vector2(moveSpeed * entityDirection, 0f);
                break;
            default:
                break;
        }
    }

    //toggling visiblity on sinking/resurfacing
    private void SinkEntity(bool state)
    {
        if (state)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        
    }

    //if out of map move it on the other side to continue movement
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (entityDirection==1)
        {
            if (collision.name == "GameBoundaryRight")
            {
                transform.position = new Vector3(-15f, transform.position.y, transform.position.z);
            }

        }
        else
        {
            if (collision.name == "GameBoundaryLeft")
            {
                transform.position = new Vector3(15f, transform.position.y, transform.position.z);
            }
        }
    }

    public float GetSpeed()
    {
        return moveSpeed;
    }

    public int GetDirection()
    {
        return entityDirection;
    }
}
