using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeballPlayerCheck : MonoBehaviour
{
    // Start is called before the first frame update
    bool ifTriggered=false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision)
            {
            GameObject triggeringObject = collision.gameObject;
            // Debug.Log(triggeringObject.tag);
            if (triggeringObject.CompareTag("Player") && !ifTriggered)
            {
               
                gameObject.transform.parent.gameObject.GetComponent<EyeballController>().AttackMode = true;

                    ifTriggered = false;
                }
            }


    }
}
