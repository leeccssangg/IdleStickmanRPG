using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class SPSkill11  : ShootPattern{
public override void Shoot(){
    m_COShooting = StartCoroutine(CO_OnShooting());
}
IEnumerator CO_OnShooting(){
    OnStartShoot();
    string bulletPrefabName = m_BulletInfo.prefabName;
    var pos = m_FirePos.position;
    for(int i = 0; i < BulletAmount; i++){
        GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
        if(go == null) break;
        go.transform.position = pos;
        var bl = go.GetComponent<Bullet>();
        ConfigBullet(bl);
        bl.RegisterInScene();
        bl.InitPiercingValue(m_BulletInfo.piercing);
        bl.FireToward(UnityEngine.Vector3.right);
        OnBulletAppear();
        yield return Yielders.Get(FireRate);
    }
    OnShotDone();
}
}
