using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Living
{

    protected override void OnEnable() {
        base.OnEnable();
    }
    public override void OnDamage(float damage) {
        base.OnDamage(damage);
    }
    public override void Die() {
        base.Die();
    }
}
