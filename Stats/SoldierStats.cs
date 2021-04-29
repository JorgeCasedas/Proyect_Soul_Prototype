using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierStats : BaseStats
{
    [SerializeField]
    float shieldBlockPercentaje;

    public override void GetHit(GameObject enemy, Vector3 weaponHitBox, float _damage)
    {
        if (!controller.IsAlive ())
            return;
        float damage = -_damage;
        if (((SoldierController)controller).IsDefending ())
            damage = (damage / 100) * (100 - shieldBlockPercentaje);
        AddHealth (damage);
        controller.GetHit (enemy, weaponHitBox);
    }

}
