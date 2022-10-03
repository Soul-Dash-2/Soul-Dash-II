using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrigger : MonoBehaviour
{
    public List<GameObject>  pikes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

 void OnTriggerEnter2D (Collider2D coll)
     {
           if (coll.CompareTag ("Player"))
            { 
             foreach (GameObject pike in pikes)
             {
               
                pike.gameObject.SendMessage("Fall");
             }
            }
     }
    // Update is called once per frame
    void Update()
    {
        
    }
}
