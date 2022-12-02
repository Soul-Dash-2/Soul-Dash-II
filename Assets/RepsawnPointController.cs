using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepsawnPointController : MonoBehaviour
{
    public bool actived=false;
    private bool ifTriggered = false;
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

                actived = true;
                ifTriggered = true;
            }
        }


    }
}
