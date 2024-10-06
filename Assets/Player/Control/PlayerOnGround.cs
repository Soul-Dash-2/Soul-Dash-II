using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnGround : MonoBehaviour
{
    /* Tracks every "ground" object the collider is currently making contact with
    The player is said to be on the ground if the collider is touching at least 1
    ground object --> see OnGround() */
    private HashSet<GameObject> collisionObjects = new HashSet<GameObject>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            collisionObjects.Add(collision.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            collisionObjects.Remove(collision.gameObject);
        }
    }

    public bool OnGround()
    {
        return collisionObjects.Count > 0;
    }
}
