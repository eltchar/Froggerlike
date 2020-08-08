using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControler : MonoBehaviour
{
    //base variables
    private float moveSpeed;
    private Vector3 movePoint;
    private bool entityDirection;
    [SerializeField] private int entityType = 0;

    private void Start()
    {
        SetEntityType(entityType);
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint, moveSpeed * Time.deltaTime);
    }
    public void SetEntityType(int Type)
    {
        switch (Type)
        {
            case 1:
                entityDirection = true;
                moveSpeed = 3f * GameManagerScript.instance.difficultyFactor;
                movePoint = new Vector3(15f, transform.position.y, transform.position.z);
                break;
            case 2:
                entityDirection = false;
                moveSpeed = 5f * GameManagerScript.instance.difficultyFactor;
                movePoint = new Vector3(-15f, transform.position.y, transform.position.z);
                break;
            case 3:
                entityDirection = false;
                moveSpeed = 2.5f * GameManagerScript.instance.difficultyFactor;
                movePoint = new Vector3(-15f, transform.position.y, transform.position.z);
                break;
            case 4:
                entityDirection = true;
                moveSpeed = 3f * GameManagerScript.instance.difficultyFactor;
                movePoint = new Vector3(15f, transform.position.y, transform.position.z);
                break;
            case 5:
                entityDirection = false;
                moveSpeed = 2.5f*GameManagerScript.instance.difficultyFactor;
                movePoint = new Vector3(-15f, transform.position.y, transform.position.z);
                break;
            default:
                break;
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
}
