using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BulletSkill_11 : Bullet{
    private bool m_IsGrounded;
    public override void FireToward(Vector3 direction){
        base.FireToward(direction);
        m_LifeTime = 3;
        Transform.DOKill();
        Vector2 pos = new Vector3(){
            x = Transform.position.x + 1.5f,
            y = 0.35f,
        };
        Transform.DOMove(pos, 0.75f).SetEase(Ease.InCirc).OnComplete(() => {
            m_IsGrounded = true;
            CameraManger.Ins.ShakeCamera(0.1f);
        });
    }
    public override void OnFiringToward(){
        if(!m_IsGrounded) return;
        base.OnFiringToward();
    }
}