using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpDropBoom:ShootPattern {
    public override void Shoot() {
        m_COShooting = StartCoroutine(coOnShooting());
    }
    IEnumerator coOnShooting() {
        int bulletAmount = GetBulletAmount();
        string bulletPrefabName = "Bullet_Warrior";
        for(int i = 0;i < bulletAmount;i++) {
            GameObject go = PrefabManager.Instance.SpawnBulletPool(bulletPrefabName);
            if(go == null)break;
            Bullet bl = go.GetComponent<Bullet>();
            bl.RegisterInScene();
        }
        yield return Yielders.Get(0.65f);
    }
}
