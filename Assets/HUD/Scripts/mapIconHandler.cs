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
		if(Input.GetKeyDown(KeyCode.Mouse0))
        {
			switch(current_lvl)
			{
				case 0:
					SceneManager.LoadScene("Level 1");
					break;
				case 1:
                    GameObject.Find("LevelController").GetComponent<LevelController>().respawnPoint = new Vector3(-60.48f, 1.19f, 0);
                    SceneManager.LoadScene("Level 1");
					break;
				case 2:
                    GameObject.Find("LevelController").GetComponent<LevelController>().respawnPoint = new Vector3(886.53f, 28.68f, 0);
                    SceneManager.LoadScene("Level 2");
					break;
				case 3:
					GameObject.Find("LevelController").GetComponent<LevelController>().respawnPoint=new Vector3(-110.39f, -69.6f,0);
                    SceneManager.LoadScene("Level 3");
                    break;
				default:
					SceneManager.LoadScene("CreditScreen");
					break;
			}
		}
    }
}
