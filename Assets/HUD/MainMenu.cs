using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    AudioSource audioSource;
    [SerializeField] AudioClip clickSound;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayButton()
    {
        
        SceneManager.LoadScene(3);
    }

    public void QuitButton() {
        Debug.Log("User exit using quit button");
        Application.Quit();
    }

    public void OptionsButton() {
        Debug.Log("Options button working");
        //Options menu will be implemented later
    }

    public void SandboxButton()
    {
        SceneManager.LoadScene(1);
    }

    public void CreditsButton()
    {
        SceneManager.LoadScene(2);
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(clickSound, 1f);
    }
}
