using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject bar_fill;
    [SerializeField] private GameObject rotation_control;

    private float health;
    private float maxHealth;
    private float fill_width;
    private float fixed_angle;
    void Start()
    {
        fixed_angle = 0.0f;
        maxHealth = (float) target.GetComponent<Enemy>().getHealth();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(fixed_angle, fixed_angle, fixed_angle, Space.World);
        //transform.rotation = rotation_control.transform.rotation * Quaternion.Inverse(transform.parent.rotation); //Quaternion.Inverse(rotation_control.transform.rotation;//);//transform.parent.rotation);//new Quaternion(fixed_angle, fixed_angle, fixed_angle, fixed_angle);
        transform.rotation = transform.parent.rotation * Quaternion.Inverse(transform.parent.rotation);
        health = (float)target.GetComponent<Enemy>().getHealth();
        fill_width = health / maxHealth;
        bar_fill.transform.localScale = new Vector3(1.0f * fill_width, 1.0f, 1.0f);
        Debug.Log("HEALTH %: " + fill_width);
    }
}
