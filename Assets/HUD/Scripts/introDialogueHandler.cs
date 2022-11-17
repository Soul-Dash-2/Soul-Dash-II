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
	
    // Start is called before the first frame update
    void Start()
    {
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


        if(Input.GetKeyDown(KeyCode.Mouse0))
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
        }
		
		if(index == 2)
			playerOptions.SetActive(true);
		else
			playerOptions.SetActive(false);

        tmp_text.text = textLines[currentLine];
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
			SceneManager.LoadScene("Level 1");
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
}