using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class dashIconUpdate : MonoBehaviour
{
    [SerializeField] private GameObject hero;
    [SerializeField] private PlayerController.DashType dashType;
    public Sprite dash_none;
    public Sprite dash_default;
    public Sprite dash_glide;
    public Sprite dash_bounce;
    public Sprite dash_invicibility;
    public Sprite dash_shieldbreaker;
    public Sprite dash_teleport;

    // Update is called once per frame
    void Update()
    {
        if (hero.GetComponent<PlayerController>().CanDash())
        {
            dashType = hero.GetComponent<PlayerController>().getDashType();
            if (dashType == PlayerController.DashType.BASIC)
            {
                GetComponent<Image>().sprite = dash_default;
            }
            else if (dashType == PlayerController.DashType.SLIME)
            {
                GetComponent<Image>().sprite = dash_bounce;
            } else if (dashType == PlayerController.DashType.EYEBALL) {
                GetComponent<Image>().sprite = dash_glide;
            }
            else if (dashType == PlayerController.DashType.SANDWORM)
            {
                GetComponent<Image>().sprite = dash_shieldbreaker;
            }
            else if (dashType == PlayerController.DashType.GOBLIN)
            {
                GetComponent<Image>().sprite = dash_invicibility;
            }
            else if (dashType == PlayerController.DashType.DEMON)
            {
                GetComponent<Image>().sprite = dash_teleport;
            }
            else
            {
                Debug.Log("UNRECOGNIZED DASH TYPE");
            }
        }
        else
        {
            GetComponent<Image>().sprite = dash_none;
        }
    }
}