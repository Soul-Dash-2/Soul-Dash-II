using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine.Profiling;

public class GhostRecorder : MonoBehaviour
{
    private GameObjectRecorder recorder;

    [SerializeField] string saveLocation = "Assets/GhostRecords/";
    [SerializeField] string clipName;
    [SerializeField] string startRecordKey = "i";
    [SerializeField] string stopRecordKey = "o";
    [SerializeField] string _deleteRecKey = "p";
    [SerializeField] GameObject recordedObject;
    string currentClipName;

    private AnimationClip clip;
    private AnimationClip currentClip;
    private bool _canRecord = true;
    int index = 0;

    private void CreateNewClip()
    {
        clip = new AnimationClip();
        clip.name = clipName;
        clip.frameRate = 60;
    }

   private void OnEnable()
    {
        CreateNewClip();

    }
// Start is called before the first frame update
void Start()
    {
        recorder = new GameObjectRecorder(recordedObject);
        recorder.BindComponentsOfType<Transform>(recordedObject, true);
        clipName = recordedObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
