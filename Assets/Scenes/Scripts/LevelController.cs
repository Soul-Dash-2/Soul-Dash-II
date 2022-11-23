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
    [SerializeField] private Vector3[] respawnPointList;

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("LevelController").Length > 1)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);

    }
    void Start()
    {  
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
            if(lowestVal> Mathf.Abs(Vector3.Distance(e.transform.position, deathPos))&&deathPos.x>=e.transform.position.x)
            {
                lowestVal = Mathf.Abs(Vector3.Distance(e.transform.position, deathPos));
                res = index;
              
            }
            index++;          
        }



        if (res != -1)
        {
            respawnPoint = new Vector3(respawnPoints[res].transform.position.x, respawnPoints[res].transform.position.y, 0);
        }
        else
        {
           // respawnPoint = GameObject.Find("initialRespawnPoint").transform.position;
        }

    }

    public void LoadNextScene()
    {

        PlayerPrefs.SetInt("current_lvl", Int32.Parse(SceneManager.GetActiveScene().name.Split(' ')[1])+1);
		SceneManager.LoadScene("Map_Screen");


    }


}
