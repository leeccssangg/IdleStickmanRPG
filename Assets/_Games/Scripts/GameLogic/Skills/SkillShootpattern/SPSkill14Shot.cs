using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SPSkill14Shot : ShootPattern {
    public override void Shoot() {
        m_COShooting = StartCoroutine(coOnShooting());
    }
    private IEnumerator coOnShooting() {
        OnStartShoot();
        List<IngameObject> list = IngameEntityManager.Ins.GetAllEnemies(IngameTeam.Team1);
        for(int j = 0; j < BulletAmount; j++){
            var effectPos = m_FirePos.position;
            effectPos.x += 0.5f * j;
            IngameManager.Ins.PutEffect(effectPos,"EnergyNovaMuzzleFire");
            CameraManger.Ins.ShakeCamera(0.12f);
            for (int i = 0; i < list.Count; i++) {
                GameObject go = PrefabManager.Instance.SpawnBulletPool(m_BulletInfo.prefabName);
                // Debug.Log(list[i].name + "___AAAAAAAAAAAAAAAAAAAAAA");
                if (go == null)break;
                Bullet bl = go.GetComponent<Bullet>();
                bl.RegisterInScene();
                IngameObject target = list[i];
                go.transform.position = target.Transform.position;
                ConfigBullet(bl);
                bl.FireFollowTarget(target.Transform);
                bl.SetLockedTarget(target);
                OnBulletAppear();
            }
            yield return Yielders.Get(m_FireRate);
        }
        OnShotDone();
    }
}
