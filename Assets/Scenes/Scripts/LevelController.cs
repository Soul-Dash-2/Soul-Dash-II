using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject playerPrefab;
    [SerializeField] string[] levelNameList;
    [SerializeField] private int waitTime =2;
    [SerializeField] Vector3 defaultPos;
    private Vector3 deathPos;
    public Vector3 repawnPoint;



    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("LevelController").Length > 1)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);

    }
    void Start()
    {


        
        foreach (string levelName in levelNameList)
        {
            SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        }
    }

    
    private void reloadAllScenes()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        foreach (string levelName in levelNameList)
        {
            SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        }
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

            Debug.LogWarning(deathPos.x+" : "+e.transform.position.x);
            if(lowestVal> Mathf.Abs(Vector3.Distance(e.transform.position, deathPos))&&deathPos.x>=e.transform.position.x)
            {
                lowestVal = Mathf.Abs(Vector3.Distance(e.transform.position, deathPos));
                res = index;
              
            }
            index++;          
        }
  
        repawnPoint = respawnPoints[res].transform.position;
        Debug.LogWarning("respawn in " + respawnPoints[res].transform.position);
    }


}
