using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BulletSkill_10 : Bullet
{
    public override void FireTween(){
        base.FireTween();
        Transform.DOMoveY(0, 0.85f).SetEase(Ease.InCirc).OnComplete(() => {
            Explosive();
            CameraManger.Ins.ShakeCamera(0.15f);
            IngameManager.Ins.PutEffect(Transform.position,m_EndEffect);
            Destroy();
        });
    }
}
