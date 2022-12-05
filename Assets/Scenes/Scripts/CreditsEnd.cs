using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsEnd : MonoBehaviour
{
    public string sceneToLoad;
    private float timer = 15f;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Application.LoadLevel(sceneToLoad);
        }
    }
}
