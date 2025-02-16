using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPSkill6Shot : ShootPattern
{
    public GameObject m_Effect;
    public ParticleSystem m_EffectParticle;
    public override void Shoot() {
        m_COShooting = StartCoroutine(coOnShooting());
    }
    IEnumerator coOnShooting() {
        OnStartShoot();
        string bulletPrefabName = m_BulletInfo.prefabName;
        var pos = m_FirePos.position;
        for(int i = 0; i < BulletAmount; i++){
            GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
            if(go == null) break;
            go.transform.position = pos;
            var bl = go.GetComponent<Bullet>();
            ConfigBullet(bl);
            bl.RegisterInScene();
            bl.FireToward(GetFireDirection());
            OnBulletAppear();
            yield return Yielders.Get(FireRate);
        }
        OnShotDone();
    }
}
