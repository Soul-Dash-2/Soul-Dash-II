using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;
    public float shields;
    public float damage;
    public string dashType; //if the enemy gives a specific dash type this is the value for it
    public float flashTime;
    public PlayerController player;
    
    private Material norm;
    public Material white;

    void Start()
    {
        player = GameObject.Find("Hero").GetComponent<PlayerController>();
        norm = GetComponent<SpriteRenderer>().material;
        flashTime = player.GetFlashTime();
    }

    public void Flash() {
        player.FlashTime();
        StartCoroutine(flash());
    }

    public IEnumerator flash() {
        GetComponent<SpriteRenderer>().material = white;
        yield return new WaitForSecondsRealtime(flashTime);
        GetComponent<SpriteRenderer>().material = norm;
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        checkHealth();
    }

    //Method that tracks the player's damage to the enemy
    public virtual void playerDamage(float dmg)
    {
        Flash();
        if(shields > 0)
        {
            float excess = shields - dmg;
            shields = shields - dmg;
            health = health + excess;
        }
        else
        {
            health = health - dmg;
        }
    }

    //Damage and Health Methods
    public float dealDamage()
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
	
	public float getShields()
    {
        return shields;
    }

    public string getDashType()
    {
        return dashType;
    }

    public void breakShields()
    {
        shields = 0;
    }

}
