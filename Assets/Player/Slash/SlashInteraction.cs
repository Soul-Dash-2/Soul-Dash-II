using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashInteraction : MonoBehaviour
{
    public float deathTimer;
    private GameObject player;
    private Transform t;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Hero");
        t = GetComponent<Transform>();
        float xScale = player.GetComponent<PlayerController>().attackRange;

        t.localScale = new Vector3(-xScale, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        t.position = player.transform.position;
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
            collision.gameObject.GetComponent<Enemy>().playerDamage(1, this.gameObject);

            collision.gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 1);

            player.GetComponent<PlayerController>().GetPlayerCamera().Shake(.1f, .75f, 1f);
            player.GetComponent<PlayerController>().letDash(); //resets the player's dash
        }
    }

}
