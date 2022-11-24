using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mapIconHandler : MonoBehaviour
{
	public int current_lvl;
	private LevelController levelController;
    // Start is called before the first frame update
    void Start()
    {
		
        current_lvl = PlayerPrefs.GetInt("current_lvl");
    }

    // Update is called once per frame
    void Update()
    {
        switch(current_lvl)
		{
			case 0:
				transform.position = new Vector3(380.0f, 240.0f, 0f);
				break;
			case 1:
                
                transform.position = new Vector3(-60.48f, 1.135472f, 0f);
				break;
			case 2:
           
                transform.position = new Vector3(884.4595f, 26.14012f, 0f);
				break;
			case 3:
                
                transform.position = new Vector3(-110.5f, -70.2f, 0f);
				break;
			// default:
                // levelController.respawnPoint= new Vector3(0.0f, 0.0f, 0.0f);
                // transform.position = new Vector3(0.0f, 0.0f, 0.0f);
				// break;
		}
		
		if(Input.GetKeyDown(KeyCode.Mouse0))
        {
			switch(current_lvl)
			{
				case 0:
					SceneManager.LoadScene("Level 1");
					break;
				case 1:
					SceneManager.LoadScene("Level 1");
					break;
				case 2:
					SceneManager.LoadScene("Level 2");
					break;
				case 3:
					SceneManager.LoadScene("Level 3");
					break;
				default:
					SceneManager.LoadScene("CreditScreen");
					break;
			}
		}
    }
}
