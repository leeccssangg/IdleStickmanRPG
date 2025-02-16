using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public enum PlayerStatsTalent
{
    ATTACK,
    GOLD,
    HP,
    TURRET_ATTACK_SPEED,
    EXP,
    TURRET_ATTACK,
    OFFLINE_EARN,
    EVASION,
    BATTLE_TIME,
    SKILL_DAMAGE,
    ALL_DAMAGE,
    ATTACK_SPEED,
    DAMAGE_REDUCTION,
    SKILL_COOLDOWN,
}
public class PlayerStatsTalentNode : TalentNode
{
    [PropertyOrder(-1)]
    [OnValueChanged("OnValueChange")]
    public PlayerStatsTalent m_NodeType;

    public override Enum GetNodeType()
    {
        return m_NodeType;
    }
    public override void SetNodeType(System.Enum value)
    {
        m_NodeType = (PlayerStatsTalent)value;
    }
#if UNITY_EDITOR
    [Button]
    private void GenerateLevelData()
    {
        m_TalentLevelDatas.Clear();
        switch (m_NodeType)
        {
            case PlayerStatsTalent.ATTACK:
                SpawnAttackLevelData();
                break;
            case PlayerStatsTalent.GOLD:
                SpawnGoldLevelData();
                break;
            case PlayerStatsTalent.HP:
                SpawnHPLevelData();
                break;
            case PlayerStatsTalent.TURRET_ATTACK_SPEED:
                SpawnTurretAttackSpeedLevelData();
                break;
            case PlayerStatsTalent.EXP:
                SpawnExpLevelData();
                break;
            case PlayerStatsTalent.TURRET_ATTACK:
                SpawnTurretAttackLevelData();
                break;
            case PlayerStatsTalent.OFFLINE_EARN:
                SpawnOfflineEarningLevelData();
                break;
            case PlayerStatsTalent.EVASION:
                SpawnEvasionLevelData();
                break;
            case PlayerStatsTalent.BATTLE_TIME:
                SpawnBattleTimeLevelData();
                break;
            case PlayerStatsTalent.SKILL_DAMAGE:
                SpawnSkillDamageLevelData();
                break;
            case PlayerStatsTalent.ALL_DAMAGE:
                SpawnAllDamageLevelData();
                break;
            case PlayerStatsTalent.ATTACK_SPEED:
                SpawnAttackSpeedLevelData();
                break;
            case PlayerStatsTalent.DAMAGE_REDUCTION:
                SpawnDamageReductionLevelData();
                break;
            case PlayerStatsTalent.SKILL_COOLDOWN:
                SpawnSkillCoolDownLevelData();
                break;
        }
    }
    private void SpawnAttackLevelData()
    {
        int rate = 0;
        int level = 0;
        if (m_NodeLine == 0)
        {
            rate = 5;
            level = 5;
        }
        else if (m_NodeLine == 2)
        {
            rate = 6;
            level = 15;
        }
        else if (m_NodeLine == 5)
        {
            rate = 20;
            level = 50;
        }
        else if (m_NodeLine == 9)
        {
            rate = 200;
            level = 200;
        }
        SpawnTalentLevelData(rate, level, "Attack");
    }
    private void SpawnGoldLevelData()
    {
        int rate = 0;
        int level = 0;
        if (m_NodeLine == 1)
        {
            rate = 1;
            level = 10;
        }
        else if (m_NodeLine == 7)
        {
            rate = 4;
            level = 25;
        }
        SpawnTalentLevelData(rate, level, "Gold");
    }
    private void SpawnHPLevelData()
    {
        int rate = 0;
        int level = 0;
        if (m_NodeLine == 1)
        {
            rate = 5;
            level = 10;
        }
        else if (m_NodeLine == 3)
        {
            rate = 20;
            level = 5;
        }
        else if (m_NodeLine == 7)
        {
            rate = 50;
            level = 25;
        }
        else if (m_NodeLine == 9)
        {
            rate = 200;
            level = 100;
        }
        SpawnTalentLevelData(rate, level, "HP");
    }
    private void SpawnTurretAttackSpeedLevelData()
    {
        int rate = 0;
        int level = 0;
        if (m_NodeLine == 1)
        {
            rate = 1;
            level = 10;
        }
        else if (m_NodeLine == 8)
        {
            rate = 1;
            level = 50;
        }
        SpawnTalentLevelData(rate, level, "Turret Attack Speed");
    }
    private void SpawnExpLevelData()
    {
        int rate = 0;
        int level = 0;
        if (m_NodeLine == 2)
        {
            rate = 4;
            level = 15;
        }
        else if (m_NodeLine == 5)
        {
            rate = 10;
            level = 25;
        }
        SpawnTalentLevelData(rate, level, "EXP");
    }
    private void SpawnTurretAttackLevelData()
    {
        int rate = 0;
        int level = 0;
        if (m_NodeLine == 2)
        {
            rate = 6;
            level = 15;
        }
        SpawnTalentLevelData(rate, level, "Turret Attack");
    }
    private void SpawnOfflineEarningLevelData()
    {
        int rate = 0;
        int level = 0;
        if (m_NodeLine == 4)
        {
            rate = 1;
            level = 20;
        }
        SpawnTalentLevelData(rate, level, "Offline Earning");
    }
    private void SpawnEvasionLevelData()
    {
        int rate = 0;
        int level = 0;
        if (m_NodeLine == 4)
        {
            rate = 1;
            level = 10;
        }
        SpawnTalentLevelData(rate, level, "Evasion");
    }
    private void SpawnBattleTimeLevelData()
    {
        int rate = 0;
        int level = 0;
        if (m_NodeLine == 4)
        {
            rate = 1;
            level = 5;
        }
        SpawnTalentLevelData(rate, level, "Battle Time");
    }
    private void SpawnSkillDamageLevelData()
    {
        int rate = 0;
        int level = 0;
        if (m_NodeLine == 5)
        {
            rate = 20;
            level = 20;
        }
        SpawnTalentLevelData(rate, level, "Skill Damage");
    }
    private void SpawnAllDamageLevelData()
    {
        int rate = 0;
        int level = 0;
        if (m_NodeLine == 6)
        {
            rate = 10;
            level = 5;
        }
        SpawnTalentLevelData(rate, level, "All Damage");
    }
    private void SpawnAttackSpeedLevelData()
    {
        int rate = 0;
        int level = 0;
        if (m_NodeLine == 7)
        {
            rate = 1;
            level = 20;
        }
        SpawnTalentLevelData(rate, level, "Attack Speed");
    }
   
    private void SpawnDamageReductionLevelData()
    {
        int rate = 0;
        int level = 0;
        if (m_NodeLine == 8)
        {
            rate = 1;
            level = 30;
        }
        SpawnTalentLevelData(rate, level, "Damage Reduction");
    }
    private void SpawnSkillCoolDownLevelData()
    {
        int rate = 0;
        int level = 0;
        if (m_NodeLine == 8)
        {
            rate = 1;
            level = 15;
        }
        SpawnTalentLevelData(rate, level, "Skill CoolDown");
    }
    private void SpawnTalentLevelData(int rate, int level, string des)
    {
        for (int i = 0; i < level+1; i++)
        {
            TalentLevelData talentLevelData = new()
            {
                m_Value = rate * (i),
                m_Description = des + " + " + (rate * (i)).ToString() + "%",
            };
            m_TalentLevelDatas.Add(talentLevelData);
        }
    }
#endif
}
