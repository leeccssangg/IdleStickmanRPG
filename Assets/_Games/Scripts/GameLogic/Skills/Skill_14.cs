using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_14 : Skill{
    protected override void FindingTarget(){
        m_Target = IngameEntityManager.Ins.GetNearestEnemy(m_Character.Transform,IngameTeam.Team1);
        if(m_Target == null || m_Target.IsDead()){
            m_Target = IngameEntityManager.Ins.GetNearestEnemy(m_Character.Transform,IngameTeam.Team1);
        }
    }

    protected override void SetFirePos(){
        if(m_Target == null || m_Target.IsDead()){
            m_Target = IngameEntityManager.Ins.GetNearestEnemy(m_Character.Transform,IngameTeam.Team1);
        }

        if(m_Target == null || m_Target.IsDead()){
            m_FirePos.position = m_Character.Transform.position + Vector3.right * 2.5f;
        } else{
            m_FirePos.position = m_Target.Transform.position;
        }
    }
}
