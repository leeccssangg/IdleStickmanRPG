using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPSkill16 : ShootPattern{
    public override void Shoot() {
        m_COShooting = StartCoroutine(coOnShooting());
    }
    private IEnumerator coOnShooting() {
        OnStartShoot();
        for(int i = 0; i < m_BulletAmount; i++){
            List<IngameObject> enemies = IngameEntityManager.Ins.GetAllEnemies(IngameTeam.Team1);
            var pos = new Vector2(m_FirePos.position.x, 0.75f);
            for(int j = 0; j < enemies.Count; j++){
                GameObject go = PrefabManager.Instance.SpawnBulletPool(m_BulletInfo.prefabName);
                if (go == null)break;
                Bullet bl = go.GetComponent<Bullet>();
                bl.RegisterInScene();
                IngameObject target = enemies[j];
                go.transform.position = target.Transform.position;
                ConfigBullet(bl);
                bl.FireFollowTarget(target.Transform);
                bl.SetLockedTarget(target);
                OnBulletAppear();
            }
            IngameManager.Ins.PutEffect(pos,"ExplosionIce");
            yield return Yielders.Get(m_FireRate);
        }
    }
}