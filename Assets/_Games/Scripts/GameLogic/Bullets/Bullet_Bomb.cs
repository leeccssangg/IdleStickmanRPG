using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Bomb : Bullet
{
    public override void Destroy() {
        List<IngameObject> list = IngameEntityManager.Ins.GetAllEnemyInRange(m_OwnerTeam,Transform.position,m_ExplosiveRange);
        for (int i = 0; i < list.Count; i++) {
            IngameObject igo = list[i];
            igo.OnHit(m_Damage, Transform.position, m_DamageInfo);
        }
        var pos = Transform.position;
        IngameManager.Ins.PutEffect(pos,m_EndEffect);
        CameraManger.Ins.ShakeCamera(.02f);
        base.Destroy();
    }
}
