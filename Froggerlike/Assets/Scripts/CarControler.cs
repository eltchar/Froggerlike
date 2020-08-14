using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControler : MonoBehaviour
{
    //base variables
    private float moveSpeed;
    private int entityDirection;
    [SerializeField] private int entityType = 0;
    Rigidbody2D entityRb;

    private void Start()
    {
        entityRb = GetComponent<Rigidbody2D>();
        SetEntityType(entityType);
    }

    // setting up variables depending on the type of entity sorted by what lane they start at
    public void SetEntityType(int Type)
    {
        switch (Type)
        {
            case 1:
                entityDirection = 1;
                moveSpeed = 3f * GameManagerScript.instance.difficultyFactor;
                entityRb.velocity = new Vector2(moveSpeed* entityDirection, 0f);
                break;
            case 2:
                entityDirection = -1;
                moveSpeed = 5f * GameManagerScript.instance.difficultyFactor;
                entityRb.velocity = new Vector2(moveSpeed * entityDirection, 0f);
                break;
            case 3:
                entityDirection = -1;
                moveSpeed = 2.5f * GameManagerScript.instance.difficultyFactor;
                entityRb.velocity = new Vector2(moveSpeed * entityDirection, 0f);
                break;
            case 4:
                entityDirection = 1;
                moveSpeed = 3f * GameManagerScript.instance.difficultyFactor;
                entityRb.velocity = new Vector2(moveSpeed * entityDirection, 0f);
                break;
            case 5:
                entityDirection = -1;
                moveSpeed = 2.5f*GameManagerScript.instance.difficultyFactor;
                entityRb.velocity = new Vector2(moveSpeed * entityDirection, 0f);
                break;
            default:
                break;
        }
    }

    // if outside of map move on the other side to resume movement
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
}
