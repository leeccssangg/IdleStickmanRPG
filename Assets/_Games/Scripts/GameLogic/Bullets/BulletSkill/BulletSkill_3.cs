using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BulletSkill_3 : Bullet{
    public override void FireToward(Vector3 direction){
        Transform.DOKill();
        Transform.DOMoveY(0, 0.55f).SetEase(Ease.InExpo).OnComplete(() => {
            Explosive();
            CameraManger.Ins.ShakeCamera(0.15f);
            IngameManager.Ins.PutEffect(Transform.position,m_EndEffect);
        });
    }
}