using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactAutoSkillWithClassStickMan :  ArtifactInGame
{
    [SerializeField] private int m_SkillID;
    [SerializeField] private Class m_StickManClassType;
    private Skill m_Skill;

    private void Awake()
    {
        //EventManager.StartListening(Constant.EVENT_EQUIPSTICKMAN,CheckEnableAttack);
    }

    // private void Update()
    // {
    //     CheckEnableAttack();
    // }

    // private void Start()
    // {
    //     float value = m_Artifact.GetSkillValue().ToFloat();
    //     m_Skill = SkillManager.Ins.AddSkillArtifact(m_SkillID, 5);
    //     CheckEnableAttack();
    // }
    public override void OnUpdateArtifact()
    {
        CheckEnableAttack();
    }
    public override void OnInit(Artifact artifact)
    {
        base.OnInit(artifact);
        var value = m_Artifact.GetSkillValue().ToFloat();
        m_Skill = SkillManager.Ins.AddSkillArtifact(m_SkillID, value);
        CheckEnableAttack();
    }

    public override void OnDespawn()
    {
        SkillManager.Ins.RemoveSkillArtifact(m_Skill);
    }

    private void CheckEnableAttack()
    {
        if(!m_Skill) return;
        m_Skill.SetAuto(StickManManager.Ins.GetStickmanClassAmount(m_StickManClassType) > 1);
    }
    
}
