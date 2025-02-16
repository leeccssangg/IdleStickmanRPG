using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPMissile : ShootPattern{
    public GameObject m_Effect;
    public ParticleSystem m_EffectParticle;
    public override void Shoot(){
        m_COShooting = StartCoroutine(CO_OnShooting());
    }
    private IEnumerator CO_OnShooting(){
        OnStartShoot();
        string bulletPrefabName = m_BulletInfo.prefabName;
        int BulletAmount = GetBulletAmount();
        Vector2 direction = Vector2.up;
        for(int i = 0; i < BulletAmount; i++){
            GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
            if(go == null) break;
            var angele = Random.Range(-90f, 20);
            direction = Quaternion.AngleAxis(angele, Vector3.forward) * direction;
            Bullet bl = go.GetComponent<Bullet>();
            bl.RegisterInScene();
            go.transform.position = GetFirePos();
            ConfigBullet(bl);
            bl.FireMissile(TargetPos);
            OnBulletAppear();
            yield return Yielders.Get(FireRate);
        }

        OnShotDone();
    }
}