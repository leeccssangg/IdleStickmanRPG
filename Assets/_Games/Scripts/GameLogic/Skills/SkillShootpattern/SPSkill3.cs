using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class SPSkill3 : ShootPattern{
    public override void Shoot(){
        m_COShooting = StartCoroutine(CO_OnShooting());
    }
    private IEnumerator CO_OnShooting(){
        OnStartShoot();
        string bulletPrefabName = m_BulletInfo.prefabName;
        var pos = (Vector2)m_FirePos.position;
        pos.y = 6;
        for(int i = 0; i < BulletAmount; i++){
            GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
            if(go == null) break;
            var tempPos = pos + Vector2.right * Random.Range(-1.5f, 1.5f);
            go.transform.position = tempPos;
            var bl = go.GetComponent<Bullet>();
            ConfigBullet(bl);
            bl.RegisterInScene();
            bl.FireToward(Vector2.down);
            OnBulletAppear();
            yield return Yielders.Get(FireRate);
        }
        OnShotDone();
    }
}