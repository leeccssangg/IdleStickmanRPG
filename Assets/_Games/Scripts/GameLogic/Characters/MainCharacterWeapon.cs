using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterWeapon : StickmanWeapon
{
    public override float GetReloadTime() {
        float num = m_RateOfFire + (m_BonusAttackSpeed * 0.01f) * m_RateOfFire;
        return 1f/num;
    }
}
