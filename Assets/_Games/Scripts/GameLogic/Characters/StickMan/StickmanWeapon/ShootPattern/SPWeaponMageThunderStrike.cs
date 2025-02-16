using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPWeaponMageThunderStrike : ShootPattern {
    public GameObject m_ThunderStrikeEffect;
    public ParticleSystem m_ThunderStrikeEffectParticle;
    public override void Shoot() {
        StartCoroutine(coOnShooting());
    }
    IEnumerator coOnShooting() {
        OnStartShoot();
        int _bulletNum = GetBulletAmount();
        List<IngameObject> l = IngameEntityManager.Ins.GetRandomEnemies(_bulletNum, GetOwnerTeam());
        if (l.Count > 0) {
            if (m_ThunderStrikeEffect != null) {
                Vector3 v = l[UnityEngine.Random.Range(0, l.Count)].Transform.position;
                Vector3 v1 = m_ThunderStrikeEffect.transform.position;
                m_ThunderStrikeEffect.transform.position = new Vector3(v.x, v1.y, v1.z);
                m_ThunderStrikeEffect.SetActive(true);
            } else if (m_ThunderStrikeEffectParticle != null) {
                Vector3 v = l[Random.Range(0, l.Count)].Transform.position;
                Vector3 v1 = m_ThunderStrikeEffectParticle.transform.position;
                m_ThunderStrikeEffectParticle.transform.position = new Vector3(v.x, v1.y, v1.z);
                m_ThunderStrikeEffectParticle.gameObject.SetActive(true);
                m_ThunderStrikeEffectParticle.Play();
            }
        }

        string bulletPrefabName = m_BulletInfo.prefabName;
        
        int num = 0;
        while (num < 10) {
            for (int i = 0; i < l.Count; i++) {
                Vector3 pos = GetFirePos();
                GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
                if (go == null)
                    break;
                IngameObject target = l[i];
                Bullet bl = go.GetComponent<Bullet>();
                bl.RegisterInScene();
                go.transform.position = target.Transform.position;
                ConfigBullet(bl);
                //Debug.Log("Thunder Damage " + bl.m_Damage);
                bl.m_Damage = bl.m_Damage / 10;
                bl.FireFollowTarget(target.Transform);
                bl.SetLockedTarget(target);
                OnBulletAppear();
            }
            num++;
            yield return Yielders.Get(0.1f);
            l = IngameEntityManager.Ins.GetRandomEnemies(_bulletNum, GetOwnerTeam());
        }
        OnShotDone();
    }
}
