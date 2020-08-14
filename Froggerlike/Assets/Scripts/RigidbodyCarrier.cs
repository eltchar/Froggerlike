using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class RigidbodyCarrier : MonoBehaviour
{
    private Rigidbody2D playerRigidbody = null;
    private Vector3 lastPos;

    void Start()
    {
        lastPos = transform.position;
    }

    private void LateUpdate()
    {
        //if player is on the object move it with the object
        if (playerRigidbody!=null)
        {
            Vector3 velocity = (transform.position - lastPos);
            playerRigidbody.transform.Translate(velocity,Space.World);
        }
        lastPos = transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //detect if player is on the object, if yes add its rigid body for moving
        if (collision.gameObject.layer==13)
        {
            if (!collision.gameObject.GetComponent<PlayerControler>().isDragged)
            {
                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                playerRigidbody = rb;
                collision.gameObject.GetComponent<PlayerControler>().isDragged = true;
            }
        }
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //detect if player is already on some object (for the purposes of being on two objects that can drag at same time so only drags and there is no fight)
        if (collision.gameObject.layer == 13)
        {
            if (!collision.gameObject.GetComponent<PlayerControler>().isDragged)
            {
                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                playerRigidbody = rb;
                collision.gameObject.GetComponent<PlayerControler>().isDragged = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //detect if player steps out of the object then remove the rigid body link
        if (collision.gameObject.layer == 13)
        {
            playerRigidbody = null;
            collision.gameObject.GetComponent<PlayerControler>().isDragged = false;
        }     
    }
}
