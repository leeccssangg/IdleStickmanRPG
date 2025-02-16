using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPWeaponPriestSkill : ShootPattern{
    public GameObject m_MeteorEffect;
    public ParticleSystem m_MeteorEffectParticle;
    public override void Shoot(){
        StartCoroutine(coOnShooting());
    }
    IEnumerator coOnShooting(){
        OnStartShoot();
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
        string bulletPrefabName = m_BulletInfo.prefabName;
        for(int i = 0; i < l.Count; i++){
            GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
            if(go == null) break;
            IngameObject target = l[i];
            Bullet bl = go.GetComponent<Bullet>();
            bl.RegisterInScene();
            go.transform.position = target.Transform.position;
            ConfigBullet(bl);
            bl.FireFollowTarget(target.Transform);
            bl.SetLockedTarget(target);
            OnBulletAppear();
        }
        yield return Yielders.Get(0.5f);
        OnShotDone();
    }
}
