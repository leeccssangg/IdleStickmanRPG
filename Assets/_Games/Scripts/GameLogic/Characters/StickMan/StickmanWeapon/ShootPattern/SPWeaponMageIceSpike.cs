using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPWeaponMageIceSpike : ShootPattern {
    public GameObject m_IceSpikeEffect;
    public float m_DealDamageDelayTime = 0.8f;
    public ParticleSystem m_IceSpikeEffectParticle;
    public override void Shoot() {
        StartCoroutine(coOnShooting());
    }
    IEnumerator coOnShooting() {
        OnStartShoot();
        
        int _bulletNum = GetBulletAmount();
        List<IngameObject> l = IngameEntityManager.Ins.GetRandomEnemies(_bulletNum, GetOwnerTeam());
        //Debug.Log("Enemy Count " + l.Count);
        if (l.Count > 0) {
            if (m_IceSpikeEffect != null) {
                Vector3 v = l[Random.Range(0, l.Count)].Transform.position;
                Vector3 v1 = m_IceSpikeEffect.transform.position;
                m_IceSpikeEffect.transform.position = new Vector3(v.x, v1.y, v1.z);
                m_IceSpikeEffect.SetActive(true);
            } else if(m_IceSpikeEffectParticle != null) {
                Vector3 v = l[Random.Range(0, l.Count)].Transform.position;
                Vector3 v1 = m_IceSpikeEffectParticle.transform.position;
                m_IceSpikeEffectParticle.transform.position = new Vector3(v.x, v1.y, v1.z);
                m_IceSpikeEffectParticle.gameObject.SetActive(true);
                m_IceSpikeEffectParticle.Play();
            }
        }
        yield return Yielders.Get(m_DealDamageDelayTime);
        string bulletPrefabName = m_BulletInfo.prefabName;
        Vector3 direction = GetFireDirection();
        for (int i = 0; i < l.Count; i++) {
            Vector3 pos = GetFirePos();
            GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
            if (go == null) break;
            IngameObject target = l[i];
            Bullet bl = go.GetComponent<Bullet>();
            bl.RegisterInScene();
            go.transform.position = target.Transform.position;
            ConfigBullet(bl);
            bl.FireFollowTarget(target.Transform);
            bl.SetLockedTarget(target);
            OnBulletAppear();
        }
        yield return Yielders.Get(0.002f);
        OnShotDone();
    }
}
