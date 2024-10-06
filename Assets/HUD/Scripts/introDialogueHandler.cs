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
    [SerializeField] private int fileIndex;
    public string[] textLines;
    public TextMeshProUGUI tmp_text;
    public int currentLine;
    private bool sentenceFinished;
    [SerializeField] AudioSource ac;
    private int currLineNumber;

    // Start is called before the first frame update
    void Start()
    {
        currLineNumber = textFiles[0].text.Split("\n").Length;
        sentenceFinished = true;
        fileIndex = 0;
        currentLine = 0;
        playerOptions.SetActive(false);
        StartCoroutine(printLetter());

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (currentLine < textFiles[fileIndex].text.Split("\n").Length && sentenceFinished)
            {
                StartCoroutine(printLetter());
            }
            else if (currentLine >= textFiles[fileIndex].text.Split("\n").Length&&fileIndex==0)
            {

                playerOptions.SetActive(true);
            }else if(currentLine >= textFiles[fileIndex].text.Split("\n").Length && fileIndex == 1)
            {
                SceneManager.LoadScene("Map_Screen");
            }
            else if(currentLine >= textFiles[fileIndex].text.Split("\n").Length && fileIndex == 2)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }


    }
    public void Button_skip()
    {
        SceneManager.LoadScene("Map_Screen");
    }


	public void Button_Yes()
	{
        
        fileIndex = 1;
        currentLine = 0;
        playerOptions.SetActive(false);
        StartCoroutine(printLetter());

    }
	
	public void Button_No()
	{

		fileIndex = 2;
        currentLine = 0;
        playerOptions.SetActive(false);
        StartCoroutine(printLetter());

    }

	IEnumerator printLetter()
	{
        tmp_text.text = "";
        sentenceFinished = false;
		ac.Play();
        foreach (string letter in textFiles[fileIndex].text.Split("\n")[currentLine].Split(" "))
		{
			foreach (char c in letter)
			{
				tmp_text.text += c;
				yield return new WaitForSeconds(0.03f);
			}

			tmp_text.text += " ";
		}
        currentLine++;
        sentenceFinished = true;
		ac.Stop();


    }
}


