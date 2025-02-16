using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPWeaponWarrior : ShootPattern {
    public override void Shoot() {
        StartCoroutine(coOnShooting());
    }
    IEnumerator coOnShooting() {
        OnStartShoot();
        int _bulletNum = GetBulletAmount();
        string bulletPrefabName = m_BulletInfo.prefabName;
        Transform from = transform;
        IngameObject target = IngameEntityManager.Ins.GetNearestEnemy(from, GetOwnerTeam());
        for (int i = 0; i < _bulletNum; i++) {
            if(target == null) break;
            GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
            //Bullet bl = Instantiate(m_Bullet);
            if (go == null) break;
            
            Bullet bl = go.GetComponent<Bullet>();
            bl.RegisterInScene();
            bl.transform.position = target.Transform.position;
            ConfigBullet(bl);
            bl.SetLockedTarget(target);
            bl.FireFollowTarget(target.transform);
            bl.IsEndEffectOnTarget = true;
            OnBulletAppear();
        }
        yield return Yielders.Get(0.002f);
        OnShotDone();
    }
}
