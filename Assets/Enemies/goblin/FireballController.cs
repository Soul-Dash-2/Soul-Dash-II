using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    public float damage = 2f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(selfDestory());
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            GameObject col = collision.gameObject;
            if (col.CompareTag("Player") || col.CompareTag("Ground")) {
                GameObject.Find("SFXManager").GetComponent<SFX_manager>().PlaySound("globinProjectileOnHit");
                Destroy(gameObject);
            }
        }
    }

    public float dealDamage()
    {
        return damage;
    }

    private IEnumerator selfDestory()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
