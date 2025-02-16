using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPSkill22Shot : ShootPattern {
    public override void Shoot() {
        m_COShooting = StartCoroutine(CO_OnShooting());
    }
    IEnumerator CO_OnShooting() {
        string bulletPrefabName = "Bullet_Skill_22";
        int bulletAmount = GetBulletAmount();
        for (int i = 0; i < bulletAmount; i++) {
            GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
            if (go == null)break;
            Bullet bl = go.GetComponent<Bullet>();
            bl.RegisterInScene();
            go.transform.position = m_FirePos.position;
            float force = m_BulletInfo.speed ;
            ConfigBullet(bl);
            force += Random.Range(-2.5f, 2f);
            bl.Speed = force;
            Vector3 dir = Quaternion.AngleAxis(Random.Range(-10,10),Vector3.forward) * m_FirePos.up;
            bl.FireBoom(dir);
            OnBulletAppear();
            yield return Yielders.Get(0.5f);
        }
        OnShotDone();
    }
}
