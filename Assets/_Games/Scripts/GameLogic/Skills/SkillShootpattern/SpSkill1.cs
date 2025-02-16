using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpSkill1:ShootPattern {
    public GameObject m_Effect;
    public ParticleSystem m_EffectParticle;
    public override void Shoot() {
        m_COShooting = StartCoroutine(coOnShooting());
    }
    IEnumerator coOnShooting() {
        OnStartShoot();
        string bulletPrefabName = m_BulletInfo.prefabName;
        int BulletAmount = GetBulletAmount();
        for(int i = 0;i < BulletAmount;i++) {
            GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
            //GameObject go =Instantiate( PrefabManager.Instance.GetBulletPrefabByName(bulletPrefabName));
            if(go == null)
                break;
            Bullet bl = go.GetComponent<Bullet>();
            bl.RegisterInScene();
            go.transform.position = GetFirePos();
            ConfigBullet(bl);
            bl.FireBoom();
            OnBulletAppear();
            yield return Yielders.Get(0.65f);
        }
        OnShotDone();
    }
}
