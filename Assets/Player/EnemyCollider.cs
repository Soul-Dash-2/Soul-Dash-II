using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Hero");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject enemy = collision.gameObject;
        //checks if the player is hitting an enemy
        if (enemy.CompareTag("Enemy") && player.GetComponent<PlayerController>().getDashing()) //player dashing through enemy
        {
            if(player.GetComponent<PlayerController>().getDashType() == PlayerController.DashType.SLIME) //player slime dashing into an enemy
            {
                player.GetComponent<PlayerController>().slimeBounce(); //activates slime bounce
                enemy.GetComponent<Enemy>().playerDamage(3);
                return;
            }
            Debug.Log("player dashing into an enemy");
            enemy.GetComponent<Enemy>().playerDamage(3);
            //check if player killed the enemy with dash
            if (enemy.GetComponent<Enemy>().getHealth() <= 0)
            {
                Debug.Log("player killed an enemy with dash of dash type: " + enemy.GetComponent<Enemy>().getDashType());
                player.GetComponent<PlayerController>().letDash(); //resets the player's dash
                player.GetComponent<PlayerController>().setDashType(enemy.GetComponent<Enemy>().getDashType()); //Gives the player the special dash
            }
        }
        else if (enemy.CompareTag("Enemy") && !player.GetComponent<PlayerController>().getDashing()) //player not dashing through enemy
        {
            Debug.Log("player not dashing into an enemy");
            //player damage
            //player knockback
        }
    }

}
