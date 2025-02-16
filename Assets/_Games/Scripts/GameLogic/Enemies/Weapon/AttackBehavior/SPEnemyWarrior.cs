using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPEnemyWarrior : ShootPattern
{
    public override void Shoot() {
        StartCoroutine(coOnShooting());
    }
    IEnumerator coOnShooting() {
        OnStartShoot();
        OnStartShoot();
        int _bulletNum = GetBulletAmount();
        string bulletPrefabName = m_BulletInfo.prefabName;
        Transform from = transform;
        //IngameObject target = IngameEntityManager.Instance.GetNearestEnemy(from, GetOwnerTeam());
        IngameObject target = IngameManager.Ins.MainCharacter;
        if(target == null) yield break;
        Vector3 pos = target.transform.position;
        for (int i = 0; i < _bulletNum; i++) {
            GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
            //Bullet bl = Instantiate(m_Bullet);
            if (go == null)
                break;
            Bullet bl = go.GetComponent<Bullet>();
            bl.RegisterInScene();
            bl.transform.position = pos;
            ConfigBullet(bl);
            bl.SetLockedTarget(target);
            bl.IsEndEffectOnTarget = true;
            //bl.FireFollowTarget(target.transform);
            OnBulletAppear();
        }
        yield return Yielders.Get(0.002f);
        OnShotDone();
    }
}
