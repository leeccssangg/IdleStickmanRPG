using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPSkill2Shot : ShootPattern
{
    public override void Shoot() {
        m_COShooting = StartCoroutine(coOnShooting());
    }
    IEnumerator coOnShooting() {
        OnStartShoot();
        List<IngameObject> list = IngameEntityManager.Ins.GetAllEnemies(IngameTeam.Team1);
        Vector3 pos = GetFirePos();
        string bulletPrefabName = "Bullet_Warrior";
        for (int i = 0; i < 10; i++) {
            GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
            if (go == null)break;
            Bullet bl = go.GetComponent<Bullet>();
            IngameObject target = list[Random.Range(0, list.Count)];
            bl.RegisterInScene();
        }
        for (int i = 0; i < list.Count; i++) {
            GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
            if (go == null)
                break;
            IngameObject target = list[i];
            Bullet bl = go.GetComponent<Bullet>();
            bl.RegisterInScene();
            go.transform.position = target.Transform.position;
            ConfigBullet(bl);
            bl.FireFollowTarget(target.Transform);
            bl.SetLockedTarget(target);
            OnBulletAppear();
            yield return Yielders.Get(FireRate);
        }
        OnShotDone();
    }
}
