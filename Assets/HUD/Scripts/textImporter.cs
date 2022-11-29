using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class textImporter : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextAsset[] textFiles;
    [SerializeField] public int index;
    public string[] textLines;
    public TextMeshProUGUI tmp_text;
    public int currentLine;
	public bool isFinished;

    // Start is called before the first frame update
    void Start()
    {
        index = -1;
        currentLine = -1;
		isFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
		if(index < 0 || currentLine < 0)
			return;
		
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
            if (currentLine < textLines.Length - 1)
                currentLine++;
            else
            {
                deactivate();
            }
        }

        tmp_text.text = textLines[currentLine];
    }

    public void activate()
    {
		isFinished = false;
        panel.SetActive(true);
    }

    public void deactivate()
    {
		isFinished = true;
		index = -1;
		currentLine = -1;
        panel.SetActive(false);
    }

    public void setIndex(int new_index)
    {
        index = new_index;
        currentLine = 0;
        activate();
    }
}
