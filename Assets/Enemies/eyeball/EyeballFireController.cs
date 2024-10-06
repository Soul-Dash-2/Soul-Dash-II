using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeballFireController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(selfDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator selfDestroy()
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
