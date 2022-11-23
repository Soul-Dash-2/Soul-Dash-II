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

    [SerializeField] String prefabName;
    private GameObject enemyPrefab;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private bool spawning;

    void Start()
    {
        enemyPrefab = (GameObject)Resources.Load(prefabName);
        player = GameObject.Find("Hero").GetComponent<PlayerController>();
        norm = GetComponent<SpriteRenderer>().material;
        flashTime = player.GetFlashTime();
        spawnPosition = this.transform.position;
        spawnRotation = this.transform.rotation;
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
    public virtual void playerDamage(float dmg, GameObject source)
    {
        Flash();
        this.Knockback(source, 20f);
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

    public void Knockback(GameObject source, float force) {
        float knockbackAmp = force;
        Vector2 sourceLoc = source.transform.position;
        Vector2 playerLoc = this.transform.position;

        Vector2 knockbackDirection = (playerLoc - sourceLoc);
        knockbackDirection.Normalize();

        this.GetComponent<Rigidbody2D>().velocity = (knockbackDirection * knockbackAmp) + this.GetComponent<Rigidbody2D>().velocity;
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
            hasDead = true;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            transform.Find("healthBar").gameObject.SetActive(false);
            if (transform.Find("LaserBeam") != null)
            {
                transform.Find("LaserBeam").gameObject.SetActive(false);
            }
                if (expostionPrefab != null)
                StartCoroutine(explode());
            StartCoroutine(spawn());

        }
    }
    IEnumerator explode()
    {
        GameObject exposion = Instantiate(expostionPrefab, new Vector3(this.gameObject.transform.position.x,
        this.gameObject.transform.position.y + 1,
        this.gameObject.transform.position.z), Quaternion.identity);
        yield return new WaitForSeconds(3);
        Destroy(exposion);
        StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        yield return new WaitForSeconds(8);
        Instantiate(enemyPrefab, new Vector3(spawnPosition.x, spawnPosition.y, spawnPosition.z), spawnRotation);
        yield return new WaitForSeconds(1);
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
