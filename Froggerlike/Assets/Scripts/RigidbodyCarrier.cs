using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class RigidbodyCarrier : MonoBehaviour
{
    private Rigidbody2D playerRigidbody = null;
    private Vector3 lastPos;
    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    private void LateUpdate()
    {
        if (playerRigidbody!=null)
        {
            Vector3 velocity = (transform.position - lastPos);
            //playerRigidbody.transform.Translate(velocity, transform);
            playerRigidbody.transform.Translate(velocity,Space.World);
        }
        lastPos = transform.position;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        if (collision.gameObject.layer == 13)
        {
            playerRigidbody = null;
            collision.gameObject.GetComponent<PlayerControler>().isDragged = false;
        }     
    }
}
