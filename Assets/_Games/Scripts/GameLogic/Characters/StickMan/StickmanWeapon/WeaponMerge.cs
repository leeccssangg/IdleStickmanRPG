using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMerge : StickmanWeapon {
    public override void InitWeapon( WeaponDataConfig weaponDataConfig, IngameType targetType, Character owner ) {
        base.InitWeapon(weaponDataConfig, targetType, owner);
        m_AttackRange = 10;
    }
}
