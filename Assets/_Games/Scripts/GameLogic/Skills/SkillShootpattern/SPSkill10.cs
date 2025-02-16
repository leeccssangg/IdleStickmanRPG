using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPSkill10 : ShootPattern{
    public override void Shoot(){
        m_COShooting = StartCoroutine(CO_OnShooting());
    }
    IEnumerator CO_OnShooting(){
        OnStartShoot();
        string bulletPrefabName = m_BulletInfo.prefabName;
        var pos = m_FirePos.position;
        pos.y = 6;
        float startX = pos.x - 1;
        for(int i = 0; i < BulletAmount; i++){
            pos.x = startX +  0.5f * i;
            GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
            if(go == null) break;
            go.transform.position = pos;
            var bl = go.GetComponent<Bullet>();
            ConfigBullet(bl);
            bl.RegisterInScene();
            bl.FireTween();
            OnBulletAppear();
            yield return Yielders.Get(FireRate);
        }
        OnShotDone();
    }
}