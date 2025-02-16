using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTornado : Bullet
{
    public float m_GrowSpeed;
    public float m_Scale = 1;
    private float m_MaxScale = 2;
    public float currentCountTime;
    private float damagedRate = 0.5f;
    public override void InitBullet( BigNumber damage, IngameTeam targetTeam, IngameTeam ownerTeam, string spriteName ) {
        base.InitBullet(damage, targetTeam, ownerTeam, spriteName);
        m_LifeTime = 10;
        Transform.localScale = Vector3.one;
        m_Scale = 1f;
        currentCountTime  = 0.2f;
    }
    public override void OnRunning() {
        base.OnRunning();
        GrowUp();
        currentCountTime -= Time.deltaTime;
        if (currentCountTime <= 0) {
            currentCountTime = damagedRate + currentCountTime;
            HitEnemyInRange();
        }
    }
    private void GrowUp() {
        if(m_Scale <= m_MaxScale) m_Scale += m_GrowSpeed * Time.deltaTime;
        Transform.localScale = Vector3.one * m_Scale;
    }
    private void HitEnemyInRange() {
        float range = m_Scale * 0.5f;
        List<IngameObject> list = IngameEntityManager.Ins.GetAllEnemyInRange(m_OwnerTeam,Transform.position,range);
        for (int i = 0; i < list.Count; i++) {
            IngameObject igo = list[i];
            ITakenDamage it = igo.GetComponent<ITakenDamage>();
            if (it != null && !it.IsDead()) {
                if (m_LockedTarget != null) {
                    if (m_LockedTarget.ID != it.GetID()) {
                        return;
                    }
                }
                //ApplyBuff(it);
                it.OnHit(m_Damage, Transform.position, m_DamageInfo);
                //Execute();
                Vector3 pos = Transform.position;
                if (IsEndEffectOnTarget) {
                    pos = igo.Transform.position + new Vector3(0, 0.5f, 0);
                }
                if (m_EndEffect != "")
                    IngameManager.Ins.PutEffectImpact(m_EndEffect, pos);
            }
        }
            
    }
}
