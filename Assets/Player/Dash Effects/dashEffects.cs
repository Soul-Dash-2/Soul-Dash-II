using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class dashEffects : MonoBehaviour
{
    private GameObject hero;
    [SerializeField] private PlayerController.DashType dashType;
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        hero = GameObject.FindGameObjectsWithTag("Player")[0];
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hero.GetComponent<PlayerController>().isDashing)
        {
            StartCoroutine(StartEffect());
        }
    }
    IEnumerator StartEffect()
    {
        if (hero.GetComponent<PlayerController>().CanDash())
        {
            dashType = hero.GetComponent<PlayerController>().getDashTypeForHud();
            if (dashType == PlayerController.DashType.BASIC)
            {
                _animator.SetBool("isBasicDashing", true);
            }
            else if (dashType == PlayerController.DashType.SLIME)
            {
                _animator.SetBool("isSlimeDashing", true);
            }
            else if (dashType == PlayerController.DashType.EYEBALL)
            {
                _animator.SetBool("isEyeballDashing", true);
            }
            else if (dashType == PlayerController.DashType.SANDWORM)
            {
                _animator.SetBool("isSandwormDashing", true);
            }
            else if (dashType == PlayerController.DashType.GOBLIN)
            {
                _animator.SetBool("isGoblinDashing", true);
            }
            else if (dashType == PlayerController.DashType.DEMON)
            {
                _animator.SetBool("isDemonDashing", true);
            }
            else
            {
                Debug.Log("UNRECOGNIZED DASH TYPE");
            }
        }

        // wait to complete dash
        float dashTime = 0;
        while (dashTime < 0.1)
        {
            dashTime += Time.deltaTime;
            yield return null;
        }
        // finish dash
        _animator.SetBool("isBasicDashing", false);
        _animator.SetBool("isSlimeDashing", false);
        _animator.SetBool("isEyeballDashing", false);
        _animator.SetBool("isSandwormDashing", false);
        _animator.SetBool("isGoblinDashing", false);
        _animator.SetBool("isDemonDashing", false);
    }
}
