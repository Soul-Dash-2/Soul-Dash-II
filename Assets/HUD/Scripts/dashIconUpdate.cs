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
        ;
    }

    // Update is called once per frame
    void Update()
    {

        if (hero.GetComponent<PlayerController>().CanDash())
        {
            dashType = hero.GetComponent<PlayerController>().getDashTypeAsInt();
            switch (dashType)
            {
                case 0:
                    GetComponent<Image>().sprite = dash_default;
                    break;
                case 1:
                    GetComponent<Image>().sprite = dash_bounce;
                    break;
                case 2:
                    Debug.Log("Dash 2");
                    break;
                case 3:
                    Debug.Log("Dash 3");
                    break;
                case 4:
                    Debug.Log("Dash 4");
                    break;
                case 5:
                    GetComponent<Image>().sprite = dash_shieldbreaker;
                    break;
                case 6:
                    GetComponent<Image>().sprite = dash_invicibility;
                    break;
                case 7:
                    GetComponent<Image>().sprite = dash_teleport;
                    break;
                default:
                    break;
            }
        }
        else
        {
            GetComponent<Image>().sprite = dash_none;
        }
    }
}