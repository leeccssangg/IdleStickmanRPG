using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPSkill17Shot : ShootPattern {
    public GameObject m_Effect;
    public ParticleSystem m_EffectParticle;
    public override void Shoot() {
        m_COShooting = StartCoroutine(coOnShooting());
    }
    IEnumerator coOnShooting() {
        OnStartShoot();
        Vector3 pos = GetFirePos();
        if (m_Effect != null) {
            Vector3 v = pos;
            Vector3 v1 = m_Effect.transform.position;
            m_Effect.transform.position = new Vector3(v.x, v1.y, v1.z);
            m_EffectParticle.Play();
            CameraManger.Ins.ShakeCamera(0.15f);
        } else if (m_EffectParticle != null) {
            m_EffectParticle.Play();
        }
        List<IngameObject> list = IngameEntityManager.Ins.GetAllEnemyInRange(IngameTeam.Team1,pos,3.5f);
        for (int i = 0; i < list.Count; i++) {
            GameObject go = PrefabManager.Instance.SpawnBulletPool(m_BulletInfo.prefabName);
            if (go == null) break;
            IngameObject target = list[i];
            Bullet bl = go.GetComponent<Bullet>();
            bl.RegisterInScene();
            go.transform.position = target.Transform.position;
            ConfigBullet(bl);
            bl.FireFollowTarget(target.Transform);
            bl.SetLockedTarget(target);
            OnBulletAppear();
        }
        yield return Yielders.Get(0.1f);
        OnShotDone();
    }
}
