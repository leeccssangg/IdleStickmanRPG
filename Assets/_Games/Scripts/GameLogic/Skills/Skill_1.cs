using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_1:Skill {
    protected override void Attack() {
        DoStartAttackEvent();
        if(m_ShootPattern != null) {
            SetFirePos();
            m_ShootPattern.m_FirePos = m_FirePos;
            m_ShootPattern.SetFireDirectionDelegate(GetFireDirection);
            m_ShootPattern.SetDamage(GetDamage());
            m_ShootPattern.BulletAmount = m_BulletAmount;
            m_ShootPattern.Shoot();
        }
    }
}
