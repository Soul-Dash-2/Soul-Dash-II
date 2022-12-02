using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goblinHeroChecker : MonoBehaviour
{
    bool ifTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision)
        {
            GameObject triggeringObject = collision.gameObject;
            // Debug.Log(triggeringObject.tag);
            if (triggeringObject.CompareTag("Player") && !ifTriggered)
            {
                gameObject.transform.parent.gameObject.GetComponent<GoblinController>().ifRun = true;
                gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                gameObject.transform.parent.gameObject.transform.Find("healthBar").gameObject.SetActive(true);
                ifTriggered = true;
            }
        }


    }
}

