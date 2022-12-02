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
        switch (current_lvl)
        {
            case 1:
                this.gameObject.transform.position = new Vector3(246.9f, -272.1f, 0);
                break;
            case 2:
                this.gameObject.transform.position = new Vector3(337.8f, -219.8f, 0);
                this.gameObject.GetComponent<Animator>().SetBool("level2", true);
                break;
            case 3:
                this.gameObject.transform.position = new Vector3(337.8f, -219.8f, 0);
                this.gameObject.GetComponent<Animator>().SetBool("level3", true);
                break;
            default:
                SceneManager.LoadScene("CreditScreen");
                break;
        }

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
                    GameObject.Find("LevelController").GetComponent<LevelController>().respawnPoint = new Vector3(-96.3f, 13.3f, 0);
                        //new Vector3(Screen.width/5.0f, Screen.height/5.0f, 0);//-60.48f, 1.19f, 0);
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
