using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PikeFallController : MonoBehaviour
{
    public float fallDelay;
    // Start is called before the first frame update
    void Start()
    {
    }

    void Fall()
    {
        StartCoroutine(delayController());
    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator delayController(){
         yield return new WaitForSeconds(fallDelay);
         GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
         GetComponent<Rigidbody2D>().gravityScale=10;
    }


}
