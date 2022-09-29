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

    public string playerDamage(float dmg, string attack)
    {
        health = health - dmg;
        if(health <= 0)
        {
            if (attack.CompareTo("dash") == 1) //checks if killed by dash
            {
                return dashType;
            }
            return null;
        }
        return null;
    }

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

}
