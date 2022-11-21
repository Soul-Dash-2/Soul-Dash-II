using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hazardsScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
           GameObject.Find("EnemyCollider").GetComponent<EnemyCollider>().TakeDamage(10000000.0f);
        }
    }
}
