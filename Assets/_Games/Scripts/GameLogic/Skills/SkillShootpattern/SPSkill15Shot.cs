using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPSkill15Shot : ShootPattern{
    public override void Shoot(){
        m_COShooting = StartCoroutine(coOnShooting());
    }
    IEnumerator coOnShooting(){
        OnStartShoot();
        string bulletPrefabName = "Bullet_Skill15";
        GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
        if(go == null) yield break;
        Bullet bl = go.GetComponent<Bullet>();
        bl.RegisterInScene();
        go.transform.position = m_FirePos.position;
        ConfigBullet(bl);
        // bl.m_Damage = bl.m_Damage/2;
        bl.IsEndEffectOnTarget = true;
        bl.FireToward(GetFireDirection());
        yield return Yielders.Get(0.1f);
        OnShotDone();
    }
}