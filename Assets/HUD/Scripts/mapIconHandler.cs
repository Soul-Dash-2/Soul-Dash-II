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
		levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        current_lvl = PlayerPrefs.GetInt("current_lvl");
    }

    // Update is called once per frame
    void Update()
    {
        switch(current_lvl)
		{
			case 1:
                levelController.respawnPoint = new Vector3(380.0f, 240.0f, 1.0f); 
                transform.position = new Vector3(380.0f, 240.0f, 1.0f);
				break;
			case 2:
                levelController.respawnPoint= new Vector3(460.0f, 290.0f, 1.0f);
                transform.position = new Vector3(460.0f, 290.0f, 1.0f);
				break;
			case 3:
                levelController.respawnPoint= new Vector3(490.0f, 370.0f, 1.0f);
                transform.position = new Vector3(490.0f, 370.0f, 1.0f);
				break;
			default:
                levelController.respawnPoint= new Vector3(0.0f, 0.0f, 0.0f);
                transform.position = new Vector3(0.0f, 0.0f, 0.0f);
				break;
		}
		
		if(Input.GetKeyDown(KeyCode.Mouse0))
        {
			switch(current_lvl)
			{
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
