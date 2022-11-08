using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{


    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.tag);
        if (col.CompareTag("Player"))
        {
            
            GameObject.Find("LevelController").SendMessage("LoadNextScene", true);
        }
    }
}
