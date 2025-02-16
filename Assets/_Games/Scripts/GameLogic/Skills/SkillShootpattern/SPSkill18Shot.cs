using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SPSkill18Shot : ShootPattern
{
    public override void Shoot() {
        m_COShooting = StartCoroutine(CO_OnShooting());
    }
    private IEnumerator CO_OnShooting() {
        for (int i = 0; i < 5; i++) {
            GameObject go = PrefabManager.Instance.SpawnBulletPool(m_BulletInfo.prefabName);
            if (go == null) break;
            Bullet bl = go.GetComponent<Bullet>();
            bl.RegisterInScene();
            go.transform.position = m_FirePos.position;
            ConfigBullet(bl);
            m_FirePos.rotation = Quaternion.Euler(0,0,Random.Range(60,85));
            bl.FireBoom(m_FirePos.right);
            OnBulletAppear();
            yield return Yielders.Get(m_FireRate);
        }
        OnShotDone();
    }
}
