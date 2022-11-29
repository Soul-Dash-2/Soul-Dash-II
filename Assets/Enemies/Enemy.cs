using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

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

    //Respawn Variables
    public bool respawnable;
    private Vector3 respawnLocation;
    private float startingHealth;
    private float startingShields;
    private Collider2D m_Collider;
    private SpriteRenderer _renderer;
    private bool afterRespawning;

    void Start()
    {
        startingHealth = health;
        startingShields = shields;
        respawnLocation = transform.position;

        player = GameObject.Find("Hero").GetComponent<PlayerController>();
        norm = GetComponent<SpriteRenderer>().material;
        flashTime = player.GetFlashTime();
        m_Collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
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
        if (afterRespawning)
        {
            m_Collider.enabled = true;
            _renderer.enabled = true;
            afterRespawning = false;
        }
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
            {
                StartCoroutine(die());
            }
            else if (respawnable == false)
            {
                Debug.Log("respawning");
                Destroy(this.gameObject);
            }
            else if (respawnable == true)
            {
                afterRespawning;
                StartCoroutine(respawn());
            }
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
        if (respawnable)
        {
            Debug.Log("attempting respawn");
            m_Collider.enabled = !m_Collider.enabled;
            Debug.Log("disabled collider");
            _renderer.enabled = !_renderer.enabled;
            Debug.Log("changed renderer");
            float respawnTime = 0; 
            while (respawnTime < 3)
            {
                respawnTime += Time.deltaTime;
                yield return null;
            }
            health = startingHealth;
            shields = startingShields;
            m_Collider.enabled = !m_Collider.enabled;
            _renderer.enabled = !_renderer.enabled;
            afterRespawning = true;
            hasDead = false;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator respawn()
    {
        m_Collider.enabled = !m_Collider.enabled;
        _renderer.enabled = !_renderer.enabled;
        //_renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, _renderer.color.a == 0.00f ? 0.0f : 0.00f);
        float respawnTime = 0;
        while (respawnTime < 3)
        {
            respawnTime += Time.deltaTime;
            yield return null;
        }
        //transform.position = respawnLocation;
        health = startingHealth;
        shields = startingShields;
        m_Collider.enabled = !m_Collider.enabled;
        _renderer.enabled = !_renderer.enabled;
        //_renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 1);
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
