using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPSkill9Shot : ShootPattern
{
    public GameObject m_Effect;
    public ParticleSystem m_EffectParticle;
    public override void Shoot() {
        m_COShooting = StartCoroutine(coOnShooting());
    }
    IEnumerator coOnShooting() {
        OnStartShoot();
        for (int j = 0; j < 2; j++) {
            List<IngameObject> list = IngameEntityManager.Ins.GetRandomEnemies(10, IngameTeam.Team1);
            if (m_Effect != null) {
                Vector3 v = m_FirePos.position;
                Vector3 v1 = m_Effect.transform.position;
                m_Effect.transform.position = new Vector3(v.x, v1.y, v1.z);
                m_EffectParticle.Play();
            } else if (m_EffectParticle != null) {
                m_EffectParticle.Play();
            }
            string bulletPrefabName = "Bullet_Warrior";
            int num = 0;
            while (num < 10) {
                for (int i = 0; i < list.Count; i++) {
                    GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
                    if (go == null)
                        break;
                    IngameObject target = list[i];
                    Bullet bl = go.GetComponent<Bullet>();
                    bl.RegisterInScene();
                    go.transform.position = target.Transform.position;
                    ConfigBullet(bl);
                    bl.m_Damage = bl.m_Damage / 10;
                    bl.FireFollowTarget(target.Transform);
                    bl.SetLockedTarget(target);
                    OnBulletAppear();
                }
                num++;
                yield return Yielders.Get(0.1f);
                list = IngameEntityManager.Ins.GetRandomEnemies(10, IngameTeam.Team1);
            }
            yield return Yielders.Get(.3f);
        }

        OnShotDone();
    }
}
