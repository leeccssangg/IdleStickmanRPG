using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPWeaponMageDeadlyPoison :ShootPattern {
    public override void Shoot() {
        StartCoroutine(coOnShooting());
    }
    IEnumerator coOnShooting() {
        OnStartShoot();
        int _bulletNum = GetBulletAmount();
        List<IngameObject> l = IngameEntityManager.Ins.GetRandomEnemies(_bulletNum, GetOwnerTeam());
        for (int i = 0; i < l.Count; i++) {
            GameObject go = PrefabManager.Instance.SpawnBulletPool(m_BulletInfo.prefabName);
            if (go == null) break;
            IngameObject target = l[i];
            Bullet bl = go.GetComponent<Bullet>();
            bl.RegisterInScene();
            go.transform.position = target.Transform.position;
            ConfigBullet(bl);
            bl.FireFollowTarget(target.Transform);
            bl.SetLockedTarget(target);
            GameObject vinesEffect = PrefabManager.Instance.SpawnPool("Effect_EM01_Skill");
            Vector3 v = target.Transform.position;
            Vector3 v1 = vinesEffect.transform.position;
            vinesEffect.transform.position = new Vector3(v.x, v.y, v1.z);
            vinesEffect.SetActive(true);
            var ef = vinesEffect.GetComponent<Effect>();
            if(ef){
                ef.SetFollow(target);
            }
            OnBulletAppear();
        }
        yield return Yielders.Get(0.002f);
        OnShotDone();
    }
}
