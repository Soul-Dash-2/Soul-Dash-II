using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnGround : MonoBehaviour
{
    bool onGround = true;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
    }

    void OnTriggerStay2D(Collider2D collider) {
        if(collider.gameObject.CompareTag("Ground")) {
            onGround = true;
        } else {
            onGround = false;
        }
    }

    public bool OnGround()
    {
        return onGround;
    }

}
