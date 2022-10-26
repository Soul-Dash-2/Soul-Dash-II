using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RespawnController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;

    void OnTriggerEnter2D(Collider2D test) {
        //We can comment out the code for our final respawn system depending which one we like better. I've included both here -Lucien
        if (test.tag == "Player") {
            //Checkpoint system
            // player.transform.position = respawnPoint.transform.position;

            //Reload scene system
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


    }
}
