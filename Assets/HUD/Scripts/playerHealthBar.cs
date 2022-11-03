using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider herohp_bar;
    private GameObject enemyCollider;
    private float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        enemyCollider = GameObject.Find("EnemyCollider");
        maxHealth = enemyCollider.GetComponent<EnemyCollider>().GetMaxHP();
        herohp_bar.maxValue = maxHealth;
        herohp_bar.value = herohp_bar.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        herohp_bar.value = enemyCollider.GetComponent<EnemyCollider>().GetPlayerHP();
    }
}
