using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject bar_fill;
    [SerializeField] private GameObject rotation_control;
	
	[SerializeField] private GameObject red_fill;
	[SerializeField] private GameObject red_bar;
	[SerializeField] private GameObject yellow_fill;
	[SerializeField] private GameObject yellow_bar;

    private float health;
    private float maxHealth;
	private float shields;
	private float maxShields;
    private float fill_width;

    void Start()
    {
		maxShields = (float) target.GetComponent<Enemy>().getShields();
		shields = maxShields;
        maxHealth = (float) target.GetComponent<Enemy>().getHealth();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {	
        transform.rotation = transform.parent.rotation * Quaternion.Inverse(transform.parent.rotation);
		shields = (float)target.GetComponent<Enemy>().getShields();
        health = (float)target.GetComponent<Enemy>().getHealth();
		
		if(shields > 1)
		{
			red_bar.SetActive(false);
			red_fill.SetActive(false);
			yellow_bar.SetActive(true);
			yellow_fill.SetActive(true);
			fill_width = shields / maxShields;
		}
		else
		{
			if (gameObject.transform.parent.transform.Find("Shield") != null)
				Destroy(gameObject.transform.parent.transform.Find("Shield").gameObject);
            red_bar.SetActive(true);
			red_fill.SetActive(true);
			yellow_bar.SetActive(false);
			yellow_fill.SetActive(false);
			fill_width = health / maxHealth;
		}
		
		// Debug.Log("STATUS Health: " + health + " Shields: " + shields);
		
        bar_fill.transform.localScale = new Vector3(1.0f * fill_width, 1.0f, 1.0f);
    }

	public void SetMaxHealth(float maxHealth) {
		this.maxHealth = maxHealth;
	}
}
