using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Skill_22 : Skill{
    public Transform m_Sun;

    protected override void Attack(){
        m_Sun.DOScale(Vector3.one, 0.25f).SetEase(Ease.InBounce);
        m_Sun.transform.position = new Vector3(m_Character.transform.position.x, m_Character.transform.position.y + 3.55f,0);
        base.Attack();
    }

    protected override void SetFirePos(){
        m_FirePos = m_Sun;
    }

    public override void OnAttackExit(){
        base.OnAttackExit();
        m_Sun.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBounce);
    }
    public override void Running(){
        base.Running();
        Vector3 pos = new Vector3(m_Character.transform.position.x + 2.2f, m_Character.transform.position.y + 3.55f,0);
        m_Sun.transform.position = Vector3.Lerp(m_Sun.transform.position,pos,0.05f);
        m_ShootPattern.transform.position = m_Sun.transform.position;
    }
}
