using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactAutoSkillByStickManAttack : ArtifactInGame
{
    [SerializeField] private int m_SkillID;
    [SerializeField] private Race m_StickManRace;
    private Skill m_Skill;
    private Stickman m_Stickman;
    private int m_Count;
    public override void OnInit(Artifact artifact)
    {
        base.OnInit(artifact);
        AddSkill();
    }
    private void AddSkill()
    {
        const float coolDown = 1f;
        m_Skill = SkillManager.Ins.AddSkillArtifact(m_SkillID, coolDown);
    }

    public override void OnUpdateArtifact()
    {
        // if (m_Stickman == null)
        // {
        // }
        m_Stickman = StickManManager.Ins.GetStickmanByRace(m_StickManRace);

        AttackCount();
    }

    private void AttackCount()
    {
        if (m_Stickman == null)
        {
            m_Count = 0;
            return;
        }
        
        m_Count = m_Stickman.AttackCount;
        if (m_Count % 10 != 0 || m_Count == 0) return;
        if(!m_Skill)return;
        m_Skill.StartAttack();
    }
   
    public override void OnDespawn()
    {
        if(!m_Skill)return;
        SkillManager.Ins.RemoveSkillArtifact(m_Skill);
    }
}