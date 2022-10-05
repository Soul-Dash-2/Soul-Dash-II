using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDectector : MonoBehaviour
{
    private bool ifEnter = false;
    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log(coll);
        if (coll.CompareTag("Player") && !ifEnter)
        {
            transform.parent.gameObject.GetComponent<EyeballController>().attackMode = true;
            ifEnter = true;
        }
            
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
