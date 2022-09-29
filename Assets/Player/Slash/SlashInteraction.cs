using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashInteraction : MonoBehaviour
{
    private float deathTimer;
    // Start is called before the first frame update
    void Start()
    {
        deathTimer = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (deathTimer <= 0)
        {
            Destroy(this.gameObject);
        }
        deathTimer -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // if the previously collided object was the ground, set isGrounded false
        if (collision.gameObject.CompareTag("Enemy")){
            collision.gameObject.GetComponent<Enemy>().playerDamage(2, "slash");
        }
    }

}
