using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_manager : MonoBehaviour
{
    [SerializeField] private AudioSource oneShotAudioSource;
    [SerializeField] private AudioSource walkingAudioSource;
    [SerializeField]  AudioClip basicDashSound;
    [SerializeField] AudioClip slashSound;
    [SerializeField] AudioClip eyeballLaserSound;
    [SerializeField] private AudioSource enemyAudioSource;
    [SerializeField] AudioClip globinProjectileOnHit;
    [SerializeField] AudioClip heroTakeDamage;
    AudioClip normalWalking;
    private bool isWaking = false;

    // Start is called before the first frame update
    void Start()
    {
        basicDashSound = Resources.Load<AudioClip>("Audio/basicDash");
        slashSound= Resources.Load<AudioClip>("Audio/slash");
        normalWalking = Resources.Load<AudioClip>("Audio/normalWalking");
        eyeballLaserSound = Resources.Load<AudioClip>("Audio/laser");
        globinProjectileOnHit = Resources.Load<AudioClip>("Audio/goblinProjectileOnHit");
        heroTakeDamage = Resources.Load<AudioClip>("Audio/heroTakeDamage");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaking&& walkingAudioSource.isPlaying)
        {
            walkingAudioSource.Stop();
        }else if (isWaking && !walkingAudioSource.isPlaying)
        {
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
                oneShotAudioSource.PlayOneShot(basicDashSound,0.3f);
                break;

            case "slash":
                oneShotAudioSource.PlayOneShot(slashSound,0.3f);
                break;

            case "eyeballLaser":
                enemyAudioSource.PlayOneShot(eyeballLaserSound, 0.1f);
                break;

            case "globinProjectileOnHit":
                enemyAudioSource.PlayOneShot(globinProjectileOnHit,0.1f);
                break;

            case "heroTakeDamage":
                enemyAudioSource.PlayOneShot(heroTakeDamage, 0.1f);
                break;





        }
    }

    public void isWalking() { isWaking = true; }
    public void notWalking() { isWaking = false; }
}
