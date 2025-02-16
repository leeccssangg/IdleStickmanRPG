using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPSkill19 : ShootPattern
{
    public override void Shoot() {
        m_COShooting = StartCoroutine(CO_OnShooting());
    }
    private IEnumerator CO_OnShooting() {
        OnStartShoot();
        int BulletAmount = GetBulletAmount();
        string bulletPrefabName = m_BulletInfo.prefabName;
        for (int i = 0; i < BulletAmount; i++) {
            GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
            if (go == null) break;
            Vector3 pos = GetFirePos();
            Bullet bl = go.GetComponent<Bullet>();
            bl.RegisterInScene();
            go.transform.position = pos;
            ConfigBullet(bl);
            bl.InitExplosive(100);
            bl.FireToward(Vector3.right);
            OnBulletAppear();
            yield return Yielders.Get(m_FireRate);
        }
        OnShotDone();
    }
}
