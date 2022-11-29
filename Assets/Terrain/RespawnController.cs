using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RespawnController : MonoBehaviour
{
   
    void OnTriggerEnter2D(Collider2D col) {
       
        if (col.tag == "Player") {
           GameObject.Find("LevelController").SendMessage("onDeathControl", true);
            Debug.Log("sdjaf");
        }
    }
}
