using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finalSceneTrigger : MonoBehaviour
{
	[SerializeField] private PlayerController hero_ref;
	[SerializeField] private GameObject pos_obj;
	private Vector3 final_pos;
	private PlayerController player_ctrl;
	[SerializeField] private pauseScreenToggle pst;
	
    // Start is called before the first frame update
    void Start()
    {
		player_ctrl = hero_ref.GetComponent<PlayerController>();
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
			//player_ctrl.Disable();
			player_ctrl.onDisable();
			pst.RemoveGUI();
            hero_ref.transform.position = final_pos;
			hero_ref.GetPlayerCamera().Shake(999.99f,1.0f,50.0f);
            //GameObject.Find("LevelController").SendMessage("LoadNextScene", true);
        }
    }
}
