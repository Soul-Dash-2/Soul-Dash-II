using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class undoRespawnOverideController : MonoBehaviour
{
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
            if (triggeringObject.CompareTag("Player") )
            {
                GameObject.Find("LevelController").GetComponent<LevelController>().overidePos = new Vector3();
            }
        }


    }
}
