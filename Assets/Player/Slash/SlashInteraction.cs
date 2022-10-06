using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashInteraction : MonoBehaviour
{
    public float deathTimer;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Hero");
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
        //checks if slash hits an enemy
        if (collision.gameObject.CompareTag("Enemy")){
            collision.gameObject.GetComponent<Enemy>().playerDamage(2);
            player.GetComponent<PlayerController>().letDash(); //resets the player's dash
        }
    }

}
