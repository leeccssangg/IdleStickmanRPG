using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_6 : Skill{
    private Vector3 m_FireDir;

    protected override void SetFirePos(){
        var pos = new Vector2(){
            x = m_Character.Transform.position.x - 3.5f,
            y = 0,
        };
        m_FirePos.position = pos;
    }

    protected override Vector3 GetFireDirection(){
        if(m_Target == null){
            m_FireDir = Vector3.right;
            m_FireDir = Quaternion.AngleAxis(Random.Range(10,45),Vector3.forward) * m_FireDir;
        } else{
            Vector3 v = m_Target.Transform.position + Vector3.up * 0.5f;
            Vector3 direct = v - m_FirePos.position;
            m_FireDir = direct.normalized;
            m_FireDir = Quaternion.AngleAxis(Random.Range(-5,5f),Vector3.forward) * m_FireDir;
        }
        return m_FireDir;
    }
    public override float GetAttackTime(){
        return 10 * 0.15f;
    }
}