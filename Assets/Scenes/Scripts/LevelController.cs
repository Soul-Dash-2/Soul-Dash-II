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
    private int currLevel = 0;
    private Vector3 deathPos;
    public Vector3 repawnPoint;
    private Vector3 currPos;


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
        int res=0;
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
  
        repawnPoint =new Vector3(respawnPoints[res].transform.position.x, respawnPoints[res].transform.position.y,0);
    }

    public void LoadNextScene()
    {

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(levelList[++currLevel]);
        repawnPoint = GameObject.Find("InitialRespawnPoint").transform.position;



    }


}
