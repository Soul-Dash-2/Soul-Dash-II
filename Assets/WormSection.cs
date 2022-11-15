using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormSection : Enemy
{
    private SandwormManager manager;

    // Override
    public override void playerDamage(float dmg)
    {
        GameObject.Find("Hero").GetComponent<PlayerController>().FlashTime();
        Flash();
        manager.TakeDamage(dmg);
    }

    public void SetManager(SandwormManager manager)
    {
        this.manager = manager;
    }

    void Update()
    {
        health = manager.GetHealth();
    }
}
