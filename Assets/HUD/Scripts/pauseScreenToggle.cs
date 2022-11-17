using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseScreenToggle : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject settingsScreen;
    private HUDControls controls;

    private bool paused;

    void Start()
    {
        // Setup Controls
        controls = new HUDControls();
        controls.Enable();
        controls.HUD.TogglePause.started += _ => TogglePause(); // when the toggle pause button is pressed, do TogglePause()

        Deactivate();
    }

    // Unity events that allow HUD controls to be enabled or disabled
    void onEnable() => controls.Enable();
    void onDisable() => controls.Disable();

    // Toggle the pause screen
    public void TogglePause() {
        if (paused) {
            Deactivate();
            return;
        }
        Activate();
    }

    // Activate the pause screen, and set timeScale to 0
    public void Activate()
    {
        pauseScreen.SetActive(true);
        paused = true;
        Time.timeScale = 0;
    }

    // Deactivate the pause scree, and set timeScale to 1
    public void Deactivate()
    {
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(false);
        paused = false;
        Time.timeScale = 1;
    }

    // Functions for each individual button in pause screen
    public void Button_Resume()
    {
        Deactivate();
    }

    public void Button_Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Button_Settings()
    {
        settingsScreen.SetActive(true);
        pauseScreen.SetActive(false);
    }

    public void Button_Quit()
    {
        Application.Quit();
    }

    // Functions for each individual button in the settings screen
    public void Button_Back()
    {
        settingsScreen.SetActive(false);
        pauseScreen.SetActive(true);
    }
}
