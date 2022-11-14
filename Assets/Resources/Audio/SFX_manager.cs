using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_manager : MonoBehaviour
{
    [SerializeField] private AudioSource oneShotAudioSource;
    [SerializeField] private AudioSource walkingAudioSource;
    [SerializeField]  AudioClip basicDashSound;
    [SerializeField] AudioClip slashSound;
    AudioClip normalWalking;
    private bool isWaking = false;

    // Start is called before the first frame update
    void Start()
    {
        basicDashSound = Resources.Load<AudioClip>("Audio/basicDash");
        slashSound= Resources.Load<AudioClip>("Audio/slash");
        normalWalking = Resources.Load<AudioClip>("Audio/normalWalking");
        
        



    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaking&& walkingAudioSource.isPlaying)
        {
            walkingAudioSource.Stop();
        }else if (isWaking && !walkingAudioSource.isPlaying)
        {
            Debug.Log("fsdf");
            walkingAudioSource.clip = normalWalking;
            walkingAudioSource.loop = true;
            walkingAudioSource.Play();
        } 

    }

    public void PlaySound(string clip)
    {
        switch (clip)
        {
            case "basicDash":
                oneShotAudioSource.PlayOneShot(basicDashSound);
                break;

            case "slash":
                oneShotAudioSource.PlayOneShot(slashSound);
                break;



         
        }
    }

    public void isWalking() { isWaking = true; }
    public void notWalking() { isWaking = false; }
}
