#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine.Profiling;
using UnityEngine.InputSystem;

public class GhostRecorder : MonoBehaviour
{

    private GameObjectRecorder recorder;

    [SerializeField] string saveLocation = "Assets/GhostRecords/";
    [SerializeField] string clipName;
    [SerializeField] InputAction startRecordButton;
    [SerializeField] InputAction stopRecordButton;
    [SerializeField] GameObject recordedObject;

    private AnimationClip clip;
    private bool canRecord = false;

    private void CreateNewClip()
    {
        clip = new AnimationClip();
        clip.name = clipName;
        clip.frameRate = 60;
    }

    private void OnEnable()
    {
        if (clip == null)
            CreateNewClip();

    }
    // Start is called before the first frame update
    void Start()
    {
        recorder = new GameObjectRecorder(recordedObject);
        recorder.BindComponentsOfType<Transform>(recordedObject, true);

        startRecordButton = new InputAction(
            type: InputActionType.Button,
            binding: "<Keyboard>/i",
            interactions: "press(behavior=1)");
        startRecordButton.Enable();
        stopRecordButton = new InputAction(
            type: InputActionType.Button,
            binding: "<Keyboard>/o",
            interactions: "press(behavior=1)");
        stopRecordButton.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (startRecordButton.triggered)
            StartRecording();
        if (stopRecordButton.triggered)
            StopRecording();
    }

    private void StartRecording()
    {
        //allow the GameObjectRecorder to start recording
        canRecord = true;
        CreateNewClip();
        Debug.Log("Animation Recording for " + gameObject.name + " has STARTED");

    }

    private void StopRecording()
    {
        Debug.Log("Animation Recording for " + gameObject + " has STOPPED");
        //stop recording
        canRecord = false;
        recorder.SaveToClip(clip);
        AssetDatabase.CreateAsset(clip, saveLocation + clipName + ".anim");
        Debug.Log(saveLocation + clipName + ".anim");
        AssetDatabase.SaveAssets();
    }



    private void LateUpdate()
    {
        if (clip == null) return;
        if (canRecord)
        { //the GameObjectRecorder will start recording
            recorder.TakeSnapshot(Time.deltaTime);
        }
    }


}

#endif