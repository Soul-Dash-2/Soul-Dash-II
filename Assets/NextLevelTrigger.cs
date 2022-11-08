using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{


    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.tag == "Player")
        {
            GameObject.Find("LevelController").SendMessage("LoadNextScene", true);
        }
    }
}
