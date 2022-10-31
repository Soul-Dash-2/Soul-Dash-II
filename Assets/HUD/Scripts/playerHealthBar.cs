using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider herohp_bar;
    [SerializeField] private GameObject herohp_ref;
    private float health;
    private float maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        herohp_bar.maxValue = (float)herohp_ref.GetComponent<EnemyCollider>().GetPlayerHP();
        herohp_bar.value = herohp_bar.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        herohp_bar.value = (float)herohp_ref.GetComponent<EnemyCollider>().GetPlayerHP();
    }
}
