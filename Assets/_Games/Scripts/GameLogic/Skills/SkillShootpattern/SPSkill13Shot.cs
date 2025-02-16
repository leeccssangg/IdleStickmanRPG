using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPSkill13Shot : ShootPattern
{
    public override void Shoot() {
        m_COShooting = StartCoroutine(coOnShooting());
    }
    IEnumerator coOnShooting() {
        OnStartShoot();
        Vector3 from = m_FirePos.position;
        float range = 10;
        List<IngameObject> list = IngameEntityManager.Ins.GetAllEnemyInRange(IngameTeam.Team1,from, range);
        string bulletPrefabName = "Bullet_Warrior";
        for (int i = 0; i < list.Count; i++) {
            IngameObject target = list[i];
            OnBulletAppear();
            GameObject vinesEffect = PrefabManager.Instance.SpawnPool("Effect_Skill_13");
            Vector3 v = target.Transform.position;
            Vector3 v1 = vinesEffect.transform.position;
            vinesEffect.transform.position = new Vector3(v.x, v.y, v1.z);
            vinesEffect.SetActive(true);
            vinesEffect.transform.SetParent(target.Transform);
            OnBulletAppear();
        }
        yield return Yielders.Get(1);
        for (int i = 0; i < list.Count; i++) {
            GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
            if (go == null)
                break;
            Bullet bl = go.GetComponent<Bullet>();
            bl.RegisterInScene();
            IngameObject target = list[i];
            go.transform.position = target.Transform.position;
            ConfigBullet(bl);
            bl.FireFollowTarget(target.Transform);
            bl.IsEndEffectOnTarget = true;
            bl.SetLockedTarget(target);
        }
        OnShotDone();
    }
}
