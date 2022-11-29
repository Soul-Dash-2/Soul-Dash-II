using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finalSceneTrigger : MonoBehaviour
{
	[SerializeField] private GameObject hero_ref;
	[SerializeField] private GameObject pos_obj;
	private Vector3 final_pos;
	
    // Start is called before the first frame update
    void Start()
    {
        final_pos = pos_obj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.tag);
        if (col.CompareTag("Player"))
        {
            hero_ref.transform.position = final_pos;
            //GameObject.Find("LevelController").SendMessage("LoadNextScene", true);
        }
    }
}
