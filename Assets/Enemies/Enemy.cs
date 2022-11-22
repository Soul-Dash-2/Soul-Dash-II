using System;
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
    private float flashTime;
    private PlayerController player;
    private Material norm;
    public Material white;
    private bool hasDead = false;
    public GameObject? expostionPrefab;

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
            float excess = 0;
            if (shields - dmg < 0)
            {
                excess = shields - dmg;
                excess = -excess;
            }
            shields = shields - dmg;
            health = health - excess;
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
        if(this.health <= 0&&!hasDead)
        {

            if (expostionPrefab != null)
                StartCoroutine(die());
            else
                Destroy(this.gameObject);

        }
    }

    IEnumerator die()
    {
        hasDead = true;
       GameObject exposion= Instantiate(expostionPrefab, new Vector3(this.gameObject.transform.position.x,
            this.gameObject.transform.position.y + 1,
            this.gameObject.transform.position.z),Quaternion.identity);
     


        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        this.gameObject.GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(3);
        Destroy(exposion);
        Destroy(this.gameObject);
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
