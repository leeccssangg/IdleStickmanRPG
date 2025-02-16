using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPWeaponWarriorSkill :  ShootPattern {
    public float m_Range;
    public override void Shoot() {
        StartCoroutine(coOnShooting());
    }
   private  IEnumerator coOnShooting() {
        Vector3 center = transform.position;
        List<IngameObject> l = IngameEntityManager.Ins.GetAllEnemyInRange(GetOwnerTeam(),center, m_Range);
        string bulletPrefabName = m_BulletInfo.prefabName;
        for (int i = 0; i < l.Count; i++) {
            GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName); 
            if (go == null) break;
            IngameObject target = l[i];
            Bullet bl = go.GetComponent<Bullet>();
            bl.RegisterInScene();
            go.transform.position = target.Transform.position;
            ConfigBullet(bl);
            bl.FireFollowTarget(target.Transform);
            bl.SetLockedTarget(target);
            OnBulletAppear();
        }
        yield return Yielders.Get(0.002f);
        OnShotDone();
    }
}
