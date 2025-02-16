using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Skill2 : Bullet{
    private float m_WaitTime;
    public override void FireMissile(Vector2 targetPos){
        base.FireMissile(targetPos);
        Transform.localEulerAngles = new Vector3(0, 0, Random.Range(-20, 60));
        m_WaitTime = Random.Range(0.35f, 0.55f);
    }
    public override void OnMissileExecute(){
        if(m_WaitTime > 0){
            m_WaitTime -= Time.deltaTime;
            m_Rigidbody.velocity = transform.up * Speed/1.5f;
            return;
        }
        Vector2 dir = m_TargetPos - m_Rigidbody.position;
        dir.Normalize();
        float rotateAmount = Vector3.Cross(dir, transform.up).z;
        m_Rigidbody.angularVelocity = -rotateAmount * 1200;
        m_Rigidbody.velocity = transform.up * Speed;
        
        if(Vector2.Distance(transform.position, m_TargetPos) < 0.2f){
            Deactive();
        }
    }
    public override bool IsOutCamera(){
        return false;
    }
}