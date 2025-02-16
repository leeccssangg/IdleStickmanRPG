using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPWeaponPriest : ShootPattern
{
    public override void Shoot() {
        StartCoroutine(coOnShooting());
    }
    IEnumerator coOnShooting() {
        OnStartShoot();
        int _bulletNum = GetBulletAmount();
        string bulletPrefabName = m_BulletInfo.prefabName;
        Vector3 direction = GetFireDirection();
        for (int i = 0; i < _bulletNum; i++) {
            Vector3 pos = GetFirePos();
            GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
            if (go == null)
                break;
            Bullet bl = go.GetComponent<Bullet>();
            bl.RegisterInScene();
            go.transform.position = pos;
            ConfigBullet(bl);
            go.transform.position = pos;
            bl.FireToward(direction);
            OnBulletAppear();
        }
        yield return Yielders.Get(0.002f);
        OnShotDone();
    }
}
