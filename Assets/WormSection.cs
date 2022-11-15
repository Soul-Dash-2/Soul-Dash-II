using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormSection : Enemy
{

    private SandwormManager manager;
    private float maxHealth;

    // Override
    public override void playerDamage(float dmg)
    {
        Flash();
        manager.TakeDamage(dmg);
    }

    public void SetManager(SandwormManager manager)
    {
        this.manager = manager;
    }

    void Start() {
        maxHealth = manager.GetHealth();
    }

    void Update()
    {
        maxHealth = manager.GetHealth();
        health = manager.GetHealth();
    }
}
