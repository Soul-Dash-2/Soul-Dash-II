using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogueID : MonoBehaviour
{
    [SerializeField] private int id;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int getID()
    {
        return id;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
           GameObject.Find("Empty_dialogueBox").GetComponent<textImporter>().setIndex(id);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("ehrauo");
            GameObject.Find("Empty_dialogueBox").GetComponent<textImporter>().deactivate();
        }
    }
}