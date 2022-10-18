using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject bar_fill;

    private float health;
    private float maxHealth;
    private float fill_width;
    void Start()
    {
        maxHealth = (float) target.GetComponent<Enemy>().getHealth();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        health = (float)target.GetComponent<Enemy>().getHealth();
        fill_width = health / maxHealth;
        bar_fill.transform.localScale = new Vector3(1.0f * fill_width, 1.0f, 1.0f);
        Debug.Log("HEALTH %: " + fill_width);
    }
}
