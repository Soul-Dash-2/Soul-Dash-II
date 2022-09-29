using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;        
    public float damage;
    public string dashType; //if the enemy gives a specific dash type this is the alue for it
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkHealth();
    }

    //Method that tracks the player's damage to the enemy
    public void playerDamage(float dmg)
    {
        health = health - dmg;
    }

    //Damage and Health Methods
    float dealDamage()
    {
        return damage;
    }

    void checkHealth()
    {
        if(this.health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public float getHealth()
    {
        return health;
    }

    public string getDashType()
    {
        return dashType;
    }

}
