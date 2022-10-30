using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LeveLController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string[] levelNameList;
    void Start()
    {
        foreach (string levelName in levelNameList)
        {
            SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        }
    }

}
