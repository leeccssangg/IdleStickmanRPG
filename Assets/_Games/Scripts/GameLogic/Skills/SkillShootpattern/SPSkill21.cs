using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPSkill21 : ShootPattern
{
    private float m_ReLoadTime = 0.5f;
    private bool m_StopShot = true;
    // private void Update(){
    //     if(m_StopShot) return;
    //     if(m_ReLoadTime > 0){
    //         m_ReLoadTime -= Time.deltaTime;
    //         return;
    //     }
    //     if(m_BulletAmount <= 0) return;
    //     string bulletPrefabName = m_BulletInfo.prefabName;
    //     GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
    //     if (go == null) return;
    //     Vector3 pos = GetFirePos();
    //     Bullet bl = go.GetComponent<Bullet>();
    //     bl.RegisterInScene();
    //     go.transform.position = pos;
    //     ConfigBullet(bl);
    //     bl.FireChase(transform);
    //     OnBulletAppear();
    //     m_BulletAmount--;
    //     m_ReLoadTime = m_FireRate;
    // }
    public override void Shoot() {
        //m_StopShot = false;
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
            bl.FireChase(transform);
            OnBulletAppear();
            yield return Yielders.Get(m_FireRate);
        }
        OnShotDone();
    }
}
