using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_manager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]  AudioClip basicDash;

    
    // Start is called before the first frame update
    void Start()
    {
        basicDash = Resources.Load<AudioClip>("Audio/basicDash");
        if (basicDash == null) Debug.Log("fuck you");
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(string clip)
    {
        switch (clip)
        {
            case "basicDash":
                audioSource.PlayOneShot(basicDash);
                break;
        }
    }
}
