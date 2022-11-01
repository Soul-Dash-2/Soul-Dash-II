using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormExplosionInteraction : MonoBehaviour
{
    public float deathTimer;
    private GameObject player;
    private Transform t;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Hero");
        t = GetComponent<Transform>();
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().playerDamage(3);
        }
    }

}
