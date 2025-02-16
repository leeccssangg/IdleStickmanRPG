using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPWeaponMageSkill : ShootPattern{
    public GameObject m_MeteorEffect;
    public ParticleSystem m_MeteorEffectParticle;
    public override void Shoot(){
        StartCoroutine(coOnShooting());
    }
    IEnumerator coOnShooting(){
        OnStartShoot();
        yield return Yielders.Get(0.5f);
        int _bulletNum = GetBulletAmount();
        List<IngameObject> l = IngameEntityManager.Ins.GetRandomEnemies(_bulletNum, GetOwnerTeam());
        if(l.Count > 0){
            if(m_MeteorEffect != null){
                Vector3 v = l[Random.Range(0, l.Count)].Transform.position;
                Vector3 v1 = m_MeteorEffect.transform.position;
                m_MeteorEffect.transform.position = new Vector3(v.x, v1.y, v1.z);
                m_MeteorEffectParticle.Play();
            } else if(m_MeteorEffectParticle != null){
                m_MeteorEffectParticle.Play();
            }
        }
        int num = 0;
        string bulletPrefabName = m_BulletInfo.prefabName;
        while(num < 10){
            for(int i = 0; i < l.Count; i++){
                GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
                if(go == null) break;
                IngameObject target = l[i];
                Bullet bl = go.GetComponent<Bullet>();
                bl.RegisterInScene();
                go.transform.position = target.Transform.position;
                ConfigBullet(bl);
                bl.m_Damage = bl.m_Damage / 10;
                bl.FireFollowTarget(target.Transform);
                bl.SetLockedTarget(target);
                OnBulletAppear();
            }
            num++;
            yield return Yielders.Get(0.2f);
            l = IngameEntityManager.Ins.GetRandomEnemies(_bulletNum, GetOwnerTeam());
        }

        OnShotDone();
    }
}