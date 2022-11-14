using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_manager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]  AudioClip basicDashSound;
    [SerializeField] AudioClip slashSound;
    AudioClip normalWalking;

    // Start is called before the first frame update
    void Start()
    {
        basicDashSound = Resources.Load<AudioClip>("Audio/basicDash");
        slashSound= Resources.Load<AudioClip>("Audio/slash");
        normalWalking = Resources.Load<AudioClip>("Audio/normalWalking");

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
                audioSource.PlayOneShot(basicDashSound);
                break;

            case "slash":   
                audioSource.PlayOneShot(slashSound);
                break;
            case "normalWalking":
                audioSource.PlayOneShot(normalWalking,0.3f);
                break;


         
        }
    }
}
