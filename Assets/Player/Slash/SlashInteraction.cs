using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashInteraction : MonoBehaviour
{
    private float deathTimer;
    // Start is called before the first frame update
    void Start()
    {
        deathTimer = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (deathTimer <= 0)
        {
            Destroy(this.gameObject);
        }
        deathTimer -= Time.deltaTime;
    }
}
