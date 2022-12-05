using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    private GameObject player;  //player object
    public float maxHealth;     // player max health
    private float playerHP;     //player's health
    private float MaxIFrames = 1.2f;   //Amount of time until the player can take damage again
    private float currentIFrames;
    private bool takingDamage;
    public bool justTookDamage;
    public bool justDied;
    private bool deathAnimationFinished;

    // Start is called before the first frame update
    void Start()
    {
        playerHP = maxHealth;
        player = GameObject.Find("Hero");
        currentIFrames = 0f;
        justTookDamage = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(takingDamage == true) //If the player has gotten hit recently, give them i frames
        {
            justTookDamage = true;
            currentIFrames += Time.deltaTime;   //Iframes based on time
            if (currentIFrames >= .5f)
            {
                justTookDamage = false;
            }
            if(currentIFrames >= MaxIFrames)    //Once the iframes are gone, reset the taking damage
            {
                takingDamage = false;
                justTookDamage = false;
            }
        }
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
                enemy.GetComponent<Enemy>().playerDamage(3, this.gameObject);
                return;
            }
            if(player.GetComponent<PlayerController>().getDashType() == PlayerController.DashType.SANDWORM)
            {
                //player.GetComponent<PlayerController>().sandwormExplosion();
                enemy.GetComponent<Enemy>().breakShields(); //breaks enemies shields (if there are any)
                enemy.GetComponent<Enemy>().playerDamage(3, this.gameObject);
                return;
            }
            Debug.Log("player dashing into an enemy ");
            enemy.GetComponent<Enemy>().playerDamage(3, this.gameObject);
            currentIFrames = 0.2f; //Giving the player slight Iframes after hitting the enemy with a dash
            //check if player killed the enemy with dash
            if (enemy.GetComponent<Enemy>().getHealth() <= 0)
            {
                Debug.Log("player killed an enemy with dash of dash type: " + enemy.GetComponent<Enemy>().getDashType());
                player.GetComponent<PlayerController>().letDash(); //resets the player's dash
                player.GetComponent<PlayerController>().setDashType(enemy.GetComponent<Enemy>().getDashType()); //Gives the player the special dash

                player.GetComponent<PlayerController>().GetPlayerCamera().Shake(.3f, .75f, 10f);
            } else {
                player.GetComponent<PlayerController>().GetPlayerCamera().Shake(.2f, .75f, 2f);
            }
        }
        else if (enemy.CompareTag("Enemy") && !player.GetComponent<PlayerController>().getDashing()) //player not dashing through enemy
        {
            Debug.Log("player not dashing into an enemy");
            if (player.GetComponent<PlayerController>().getInvisible())
            {
                //take no dmg
            }
            else
            {

                float damage = enemy.GetComponent<Enemy>().dealDamage();
                TakeDamage(damage);
                //player knockback
                player.GetComponent<PlayerController>().Knockback(enemy.gameObject, 20f);
            }
        }

        if (enemy.CompareTag("Projectile"))
        {
            float damage = enemy.GetComponent<FireballController>().dealDamage();
            TakeDamage(damage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject enemy = collision.gameObject;
        if (enemy.CompareTag("Enemy") && !player.GetComponent<PlayerController>().getDashing()) //player not dashing through enemy
        {
            Debug.Log("player not dashing into an enemy");
            if (player.GetComponent<PlayerController>().getInvisible())
            {
                //take no dmg
            }
            else
            {
                float damage = enemy.GetComponent<Enemy>().dealDamage();
                TakeDamage(damage);
                //player knockback
                player.GetComponent<PlayerController>().Knockback(enemy.gameObject, 20f);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if(takingDamage == false){ //If the player has not taken damage yet
            player.GetComponent<PlayerController>().GetPlayerCamera().Shake(0.2f, 0.75f, 5f);
            GameObject.Find("SFXManager").GetComponent<SFX_manager>().PlaySound("heroTakeDamage");
            playerHP -= damage;
            takingDamage = true;
            currentIFrames = 0f;    //Set the iframes to 0
            player.GetComponent<PlayerController>().Flash();
            
            if(playerHP <= 0)
            {
                KillPlayer();
            }
            return;
        }
        else
        {
            return;
        }
    }

    void KillPlayer()
    {
        StartCoroutine(DeathAnimation());
        // reset the scene
        Debug.Log("player death animation went through?");
        //GameObject.Find("LevelController").SendMessage("onDeathControl", true);

    }

	public void SetPlayerHP(float in_newPlayerHP)
    {
        playerHP = in_newPlayerHP;
    }

    public float GetPlayerHP()
    {
        return playerHP;
    }

    public float GetMaxHP() {
        return maxHealth;
    }

    IEnumerator DeathAnimation()
    {
        Debug.Log("player just died");
        justDied = true;
        float deathTime= 0;
        while (deathTime < 1)
        {
            deathTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("player death animation done");
        GameObject.Find("LevelController").SendMessage("onDeathControl", true);
        // finish 
    }

}
