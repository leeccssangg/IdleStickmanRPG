using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWarriorHuman : StickmanWeapon {
    public string m_AttackTriggerName2 = "Attack";
    public override string GetTriggerName() {
        if (Random.Range(0, 2) == 1) {
            return m_AttackTriggerName;
        } else {
            return m_AttackTriggerName2;
        }
    }
}
