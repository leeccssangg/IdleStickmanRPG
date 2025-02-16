using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletSkill_1:Bullet {
    public override void OnBoomStart() {
        m_Collider.enabled = false;
        Transform.DOKill();
        Vector3 endPos = Transform.position;
        Vector3 pos = Transform.position + Vector3.up * 7 + Vector3.right * -2;
        Transform.position = pos;
        Transform.DOMove(endPos,1.1f).SetEase(Ease.Linear).OnComplete(() => {
            Explosive();
            CameraManger.Ins.ShakeCamera(0.15f);
            Deactive();
        });
    }
    public override void OnRunning() {
    }
    public override void OnBoomExecute() { }
    public override void OnBoomExit() { }
}
