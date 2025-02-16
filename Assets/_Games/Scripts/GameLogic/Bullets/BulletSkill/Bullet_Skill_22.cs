using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Skill_22 : Bullet
{

    public override void OnExecuteTriggerEnter2D( Collider2D collider ) {
        List<IngameObject> list = IngameEntityManager.Ins.GetAllEnemyInRange(m_OwnerTeam, Transform.position, 0.75f);
        for (int i = 0; i < list.Count; i++) {
            IngameObject igo = list[i];
            igo.OnHit(m_Damage, Transform.position, m_DamageInfo);
        }
        Vector3 pos = Transform.position;
        IngameManager.Ins.PutEffectImpact(m_EndEffect, pos);
        base.Destroy();
    }
}
