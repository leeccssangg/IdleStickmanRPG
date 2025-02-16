using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpSkill4 : ShootPattern{
    public override void Shoot(){
        List<IngameObject> list = IngameEntityManager.Ins.GetAllEnemies(IngameTeam.Team1);
        for(int i = 0; i < list.Count; i++){
            GameObject go = PrefabManager.Instance.SpawnBulletPool(m_BulletInfo.prefabName);
            if(go == null) break;
            IngameObject target = list[i];
            Bullet bl = go.GetComponent<Bullet>();
            bl.RegisterInScene();
            go.transform.position = target.Transform.position;
            ConfigBullet(bl);
            bl.FireFollowTarget(target.Transform);
            bl.SetLockedTarget(target);
            OnBulletAppear();
        }
        OnShotDone();
    }
}