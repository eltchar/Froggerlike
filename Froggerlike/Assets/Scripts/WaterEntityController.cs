using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEntityController : MonoBehaviour
{
    //base variables
    private float moveSpeed;
    private Vector3 movePoint;
    private bool entityDirection;
    private float sinkingTime = 5f;
    private bool isSunken=false;
    [SerializeField] private int entityType = 0;
    [SerializeField] private bool isSinkingType=false;

    private void Start()
    {
        SetEntityType(entityType);
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint, moveSpeed * Time.deltaTime);
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
                entityDirection = true;
                moveSpeed = 2f * GameManagerScript.instance.difficultyFactor;
                movePoint = new Vector3(20f, transform.position.y, transform.position.z);
                break;
            case 2:
                entityDirection = false;
                moveSpeed = 3f * GameManagerScript.instance.difficultyFactor;
                movePoint = new Vector3(-20f, transform.position.y, transform.position.z);
                break;
            case 3:
                entityDirection = false;
                moveSpeed = 1.5f * GameManagerScript.instance.difficultyFactor;
                movePoint = new Vector3(-20f, transform.position.y, transform.position.z);
                break;
            case 4:
                entityDirection = true;
                moveSpeed = 2f * GameManagerScript.instance.difficultyFactor;
                movePoint = new Vector3(20f, transform.position.y, transform.position.z);
                break;
            case 5:
                entityDirection = false;
                moveSpeed = 1.5f * GameManagerScript.instance.difficultyFactor;
                movePoint = new Vector3(-20f, transform.position.y, transform.position.z);
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
        if (entityDirection)
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name=="Player")
        {
            if (isSunken)
            {
                GameManagerScript.instance.DeathEvent(this, EventArgs.Empty);
            }
        }
    }

    public float GetSpeed()
    {
        return moveSpeed;
    }

    public bool GetDirection()
    {
        return entityDirection;
    }
}
