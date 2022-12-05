using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class introDialogueHandler : MonoBehaviour
{
	[SerializeField] private GameObject playerOptions;
	[SerializeField] private GameObject panel;
    [SerializeField] private TextAsset[] textFiles;
    [SerializeField] private int index;
    public string[] textLines;
    public TextMeshProUGUI tmp_text;
    public int currentLine;
	public bool script_finished;
	public bool playerChoice;
	public bool canTransition;
	private bool dialogFinished;
	private bool onetime=true;
    [SerializeField] AudioSource ac;	
	
    // Start is called before the first frame update
    void Start()
    {
		dialogFinished = true;
        index = 0;
        currentLine = 0;
		script_finished = true;
		playerOptions.SetActive(false);
		playerChoice = true;
		canTransition = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (textFiles[index] != null)
        {
            textLines = (textFiles[index].text.Split("\n"));
        }
        else
        {
            Debug.Log("Error on textImporter: TextAsset " + textFiles[index] + " not found");
        }

        if (onetime)
        {
            StartCoroutine(addLetter());
			onetime = false;
        }
        if( dialogFinished&&Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (index != 2 && currentLine < textLines.Length - 1)
			{
				script_finished = false;
                currentLine++;

			}
            else
            {
				if(canTransition)
					TransitionScene();
				script_finished = true;
                currentLine = 0;
            }
			
			if(script_finished)
				StepIntro();

            StartCoroutine(addLetter());
        }
		
		if(index == 2) {
            tmp_text.text = "";
            playerOptions.SetActive(true);
        }
			
		else
			playerOptions.SetActive(false);


		
       
    }
	
	public void StepIntro()
	{
		if(index <= 1)
			index++;
		else if(index > 2)
		{
			canTransition = true;
		}
	}
	
	public void TransitionScene()
	{
		if(playerChoice)
			SceneManager.LoadScene("Map_Screen");
		else
			SceneManager.LoadScene("MainMenu");
	}
	
	public void Button_Yes()
	{
		playerChoice = true;
		index = 3;
	}
	
	public void Button_No()
	{
		playerChoice = false;
		index = 4;
	}

	IEnumerator addLetter()
	{
        tmp_text.text = "";
        dialogFinished = false;
		ac.Play();
        foreach (string letter in textLines[currentLine].Split(" "))
		{
			foreach (char c in letter)
			{
				tmp_text.text += c;
				yield return new WaitForSeconds(0.03f);
			}

			tmp_text.text += " ";
		}
		dialogFinished = true;
		ac.Stop();
    }
}


