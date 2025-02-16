using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BulletSkill8 : Bullet{
    public Transform m_Hammer;
    [SerializeField] private Transform m_Root;

    public override void FireTween(){
        base.FireTween();
        m_LifeTime = 0.7f + 1.5f;
        m_Hammer.DOKill();
        var startPos = new Vector2(3.5f, 6);
        m_Hammer.DOLocalMove(Vector3.zero, 0.7f).From(startPos).SetEase(Ease.InCirc).OnComplete(() => {
            Explosive();
            CameraManger.Ins.ShakeCamera(0.15f);
            m_Root.localEulerAngles = (new Vector3(0, 0, Random.Range(90, 180)));
        }).OnUpdate(RotateHamer);
    }
    private void RotateHamer(){
        m_Root.Rotate(Vector3.forward * Time.deltaTime * 2000);
    }
}