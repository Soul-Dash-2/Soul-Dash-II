using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private Transform respawnPoint;

    void OnTriggerEnter2D(Collider2D test) {
        player.transform.position = respawnPoint.transform.position;
    }
}
