using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class dashIconUpdate : MonoBehaviour
{
    [SerializeField] private GameObject hero;
    [SerializeField] private int dashType;
    public Sprite dash_none;
    public Sprite dash_default;
    public Sprite dash_glide;
    public Sprite dash_bounce;
    public Sprite dash_invicibility;
    public Sprite dash_shieldbreaker;
    public Sprite dash_teleport;
    // Start is called before the first frame update
    void Start()
    {
        dashType = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch(dashType)
        {
            case 0:
                GetComponent<Image>().sprite = dash_none;
                break;
            case 1:
                GetComponent<Image>().sprite = dash_default;
                break;
            case 2:
                GetComponent<Image>().sprite = dash_glide;
                break;
            case 3:
                GetComponent<Image>().sprite = dash_bounce;
                break;
            case 4:
                GetComponent<Image>().sprite = dash_invicibility;
                break;
            case 5:
                GetComponent<Image>().sprite = dash_shieldbreaker;
                break;
            case 6:
                GetComponent<Image>().sprite = dash_teleport;
                break;
            default:
                GetComponent<Image>().sprite = dash_none;
                break;
        }
    }
}
