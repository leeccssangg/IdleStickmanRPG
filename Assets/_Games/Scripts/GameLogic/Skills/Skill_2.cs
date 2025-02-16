using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_2 : Skill{
    protected override void SetFirePos() {
        if(m_Target == null || m_Target.IsDead()) {
            m_Target = IngameEntityManager.Ins.GetRandomEnemy();
        }
        if(m_Target == null || m_Target.IsDead()) {
            m_FirePos.position = m_Character.Transform.position + Vector3.right * 2.5f;
        } else {
            m_FirePos.position = m_Character.Transform.position;
        }
    }
    public override void StartAttack(){
        if(m_Target == null){
            return;
        }
        base.StartAttack();
    }

    protected override void Attack() {
        DoStartAttackEvent();
        if(m_ShootPattern != null) {
            SetFirePos();
            m_ShootPattern.m_FirePos = m_FirePos;
            m_ShootPattern.m_FirePos = m_FirePos;
            m_ShootPattern.TargetPos = m_Target.Transform.position;
            m_ShootPattern.SetFireDirectionDelegate(GetFireDirection);
            m_ShootPattern.BulletAmount = m_BulletAmount;
            m_ShootPattern.SetDamage(GetDamage());
            m_ShootPattern.Shoot();
        }
    }
}