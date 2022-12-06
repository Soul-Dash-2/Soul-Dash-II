using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string[] levelList;
    [SerializeField] private int waitTime = 2;
    [SerializeField] Vector3 defaultPos;
    int currLevel = 0;
    private Vector3 deathPos;
    public Vector3 respawnPoint;
    private Vector3 currPos;
    public Vector3 overidePos =new Vector3();
    [SerializeField] private Vector3[] respawnPointList;

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("LevelController").Length > 1)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);

    }
    private void reloadAllScenes()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //the player will revive in the nearest repwawn point 
    public void onDeathControl() {
        deathPos = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
        reloadAllScenes();
        revive();
    }

    private void revive()
    {

        GameObject[] respawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        int index = 0;
        int res=-1;
        float lowestVal = Int32.MaxValue;
        foreach(GameObject e in respawnPoints)
        {

            if (e.GetComponent<RepsawnPointController>())
            {
                if (e.GetComponent<RepsawnPointController>().actived)
                {
                    if (lowestVal > Mathf.Abs(Vector3.Distance(e.transform.position, deathPos)))
                    {
                        lowestVal = Mathf.Abs(Vector3.Distance(e.transform.position, deathPos));
                        res = index;

                    }
                }
            }
            else
            {
                Debug.LogError("one of the repsawn points dont contain the RepsawnPointController script. Please attach one to it");
            }
            index++;          
        }



        if (res != -1)
        {
            if (overidePos == new Vector3())
            {
                respawnPoint = new Vector3(respawnPoints[res].transform.position.x, respawnPoints[res].transform.position.y, 0);
            }
            else
            {
                respawnPoint = overidePos;
            }
        }
        else
        {
            Debug.LogWarning("respawn in initialRespawnPoint");
            respawnPoint = GameObject.Find("initialRespawnPoint").transform.position;
        }

    }

    public void LoadNextScene()
    {
        PlayerPrefs.SetInt("current_lvl", Int32.Parse(SceneManager.GetActiveScene().name.Split(' ')[1])+1);
		SceneManager.LoadScene("Map_Screen");   
    }


}
