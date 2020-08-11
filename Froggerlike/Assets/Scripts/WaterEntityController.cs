using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEntityController : MonoBehaviour
{
    //base variables
    private float moveSpeed;
    private int entityDirection;
    private float sinkingTime = 5f;
    public bool isSunken=false;
    [SerializeField] private int entityType = 0;
    [SerializeField] private bool isSinkingType=false;
    Rigidbody2D entityRb;

    private void Start()
    {
        entityRb = GetComponent<Rigidbody2D>();
        SetEntityType(entityType);
    }
    private void Update()
    {
        if (isSinkingType)
        {
            if (sinkingTime > 0f)
            {
                sinkingTime -= Time.deltaTime;
            }
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
                moveSpeed = 1.5f * GameManagerScript.instance.difficultyFactor;
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
