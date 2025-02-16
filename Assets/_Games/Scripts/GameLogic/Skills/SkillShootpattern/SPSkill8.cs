using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPSkill8 : ShootPattern{
    public override void Shoot(){
        m_COShooting = StartCoroutine(CO_OnShooting());
    }
    private IEnumerator CO_OnShooting(){
        OnStartShoot();
        int _bulletNum = GetBulletAmount();
        string bulletPrefabName = m_BulletInfo.prefabName;
        for(int i = 0; i < _bulletNum; i++){
            Vector3 pos = GetFirePos();
            GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
            if(go == null) break;
            var bl = go.GetComponent<Bullet>();
            go.transform.position = pos;
            bl.RegisterInScene();
            ConfigBullet(bl);
            bl.FireTween();
            OnBulletAppear();
        }
        yield return Yielders.Get(m_FireRate);
        OnShotDone();
    }
}