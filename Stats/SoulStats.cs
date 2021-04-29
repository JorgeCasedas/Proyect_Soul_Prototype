using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulStats : BaseStats
{
    // Start is called before the first frame update
    protected override void Start ()
    {
        health = maxHealth;
    }

    public override void SetHp ()
    {
        UIManager.Instance.SetSoulHp (health, maxHealth);
    }
    public override void AddHealth (float addition)
    {
        health += addition;
        SetHp ();
        if (health <= 0)
        {
            GameManager.Instance.Lose ();
        }
    }
    public override void GetHit (GameObject enemy, Vector3 weaponHitBox, float damage)
    {
        AddHealth (-damage);
    }
}
