using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class pauseScreenToggle : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject settingsScreen;
	[SerializeField] private GameObject sfxManager;
    private HUDControls controls;

	[SerializeField] private TextMeshProUGUI tmp_text;
	private AudioSource audioSrc;
	private float current_vol;
	private float vol_step;
	
    private bool paused;

    void Start()
    {
        // Setup Controls
        controls = new HUDControls();
        controls.Enable();
        controls.HUD.TogglePause.started += _ => TogglePause(); // when the toggle pause button is pressed, do TogglePause()

		sfxManager = GameObject.Find("SFXManager");//GetComponent<SFX_manager>();
		audioSrc = sfxManager.GetComponent<AudioSource>();
		current_vol = 1.0f;
		vol_step = 0.1f;
		tmp_text.text = Mathf.Abs((int) (current_vol * 10)).ToString();

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
	
	public void Button_IncVolume()
	{
		if(current_vol < 1.0f)
		{
			current_vol += vol_step;
			audioSrc.volume = current_vol;
			tmp_text.text = Mathf.Abs((int) (current_vol * 10)).ToString();
			//tmp_text.text = current_vol.ToString();
		}
	}
	
	public void Button_DecVolume()
	{
		if(current_vol > 0.0f)
		{
			current_vol -= vol_step;
			audioSrc.volume = current_vol;
			tmp_text.text = Mathf.Abs((int) (current_vol * 10)).ToString();
			//tmp_text.text = current_vol.ToString();
		}
	}
	
	public void RemoveGUI()
	{
		gameObject.SetActive(false);
	}
}
