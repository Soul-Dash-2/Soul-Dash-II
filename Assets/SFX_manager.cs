using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_manager : MonoBehaviour
{
    static AudioSource audio;
    public static AudioClip Dash;

    
    // Start is called before the first frame update
    void Start()
    {
        Dash = Resources.Load<AudioClip>("basic dash");
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "dashSound":
                audio.PlayOneShot (Dash);
                break;
        }
    }
}
