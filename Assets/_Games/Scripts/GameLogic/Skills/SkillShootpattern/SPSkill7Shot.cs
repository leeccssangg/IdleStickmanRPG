using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPSkill7Shot : ShootPattern
{
    public override void Shoot() {
        m_COShooting = StartCoroutine(coOnShooting());
    }
    IEnumerator coOnShooting() {
        OnStartShoot();
        int _bulletNum = GetBulletAmount();
        string bulletPrefabName = m_BulletInfo.prefabName;
        Vector3 direction = GetFireDirection();
        for (int i = 0; i < _bulletNum; i++) {
            Vector3 pos = GetFirePos();
            GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
            if (go == null) break;
            BulletBoomerang bl = go.GetComponent<BulletBoomerang>();
            bl.RegisterInScene();
            ConfigBullet(bl);
            go.transform.position = pos;
            if (!bl.IsComeBack) {
                bl.FireToward(direction);
            } else {
                bl.FireBoomerang(direction);
            }
            bl.SetTypeBullet(AttackType.SKILL_ATTACK);
            OnBulletAppear();
        }
        yield return Yielders.Get(0.002f);
        OnShotDone();
    }
}
