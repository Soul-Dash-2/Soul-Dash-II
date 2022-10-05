using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RespawnController : MonoBehaviour
{


    void OnTriggerEnter2D(Collider2D test) {
        //since lucien's respawn script doesnot work, I rewrite a new one//
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);

       

    }
}
