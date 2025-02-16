using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "StatFormulaConfig", menuName = "ScriptableObjects/Formula/StatFormulaConfig", order = 1)]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class StatCostConfig : GlobalConfig<StatCostConfig> {
    public CampaignStatConfig campaignStatConfig;
    public PlayerStatConfig playerStat;
    public PlayerStatCostConfig playerStatCostConfig;
    public OwnedEffectConfig ownedConfig;

    public DungeonBossStatConfig dungeonBossRush;
    public DungeonGoldStatConfig dungeonGoldRush;
    public DungeonDeadHunterStatConfig dungeonDeadHunter;
    public DungeonMassiveShadowStatConfig dungeonMassiveShadow;

    //TODO: ID THEO ID LIST - USE: WEAPON, ARMOR, TURRET, SKILL
    public List<LevelConflig> levelConfligItems;




    [SerializeField] private CostConfig m_EquipmentEquipEffect;
    public CostConfig EquipmentEquipEffect => m_EquipmentEquipEffect;

    public TestStatConfig testStatConfig;



    public CostConfig GetStickmanOwnedEffect(Rarity rarity) {
        for (int i = 0; i < ownedConfig.stickmanOwnedEffects.Count; i++) {
            BaseStatConfig turretStat = ownedConfig.stickmanOwnedEffects[i];
            if (turretStat.rarity == rarity) {
                return turretStat.costConfig;
            }
        }
        return null;
    }
    public CostConfig GetEquipmentOwnedEffect(Rarity rarity) {
        for (int i = 0; i < ownedConfig.equipmentOwnedEffects.Count; i++) {
            BaseStatConfig turretStat = ownedConfig.equipmentOwnedEffects[i];
            if (turretStat.rarity == rarity) {
                return turretStat.costConfig;
            }
        }
        return null;
    }
    public CostConfig GetSkillOwnedEffect(Rarity rarity) {
        for (int i = 0; i < ownedConfig.skillOwnedEffects.Count; i++) {
            BaseStatConfig turretStat = ownedConfig.skillOwnedEffects[i];
            if (turretStat.rarity == rarity) {
                return turretStat.costConfig;
            }
        }
        return null;
    }

    //USE: WEAPON, ARMOR, TURRET, SKILL
    public LevelConflig GetLevelConfligItem(int Level)
    {
        LevelConflig value = null;
        
        for(int i = 0; i < levelConfligItems.Count; i++)
        {
            if(Level + 1 == levelConfligItems[i].Level)
            {
                value = levelConfligItems[i];
            }
        }
        return value;
    }
}

public enum CalculateType {
    PercentageFixed,
    PercentageMilestone,
    PercentageLinearWDownrate,
    Progression,
    Pow,
    PowMultiple
}
[System.Serializable]
public class CostConfig {
    [HorizontalGroup(GroupName = "Stats", LabelWidth = 50)]
    [LabelText("")]
    public CalculateType calculateType;
    [HorizontalGroup(GroupName = "Stats")]
    [LabelText("Base")]
    public float baseStat;
    [HorizontalGroup(GroupName = "Stats")]
    public float rate;
    [HorizontalGroup(GroupName = "Stats")]
    public float minRate;
    [HorizontalGroup(GroupName = "Stats")]
    public float prate;
    [HorizontalGroup(GroupName = "Stats")]
    public float prate2;

#if UNITY_EDITOR
    [TextArea(10, 15)]
    public string editorResult;

    [Button]
    [HorizontalGroup(GroupName = "Stats")]
    public void Calculate() {
        int maxLevel = 500;
        string s = "";
        for (int i = 1; i <= maxLevel; i++) {
            int level = i;
            s += level + "=" + GetResult(level).ToDouble().ToString("F0") + "\n";
        }
        editorResult = s;
    }
#endif
    public BigNumber GetResult(int level) {
        switch (calculateType) {
            case CalculateType.PercentageFixed:
                return FormulaManager.CalculateByPercentage(level, baseStat, rate);
            case CalculateType.PercentageMilestone:
                return FormulaManager.CalculateByPercentageWithMilestones(level, baseStat, rate, prate);
            case CalculateType.PercentageLinearWDownrate:
                return FormulaManager.CalculateByPerWithDownrate(level, baseStat, rate, minRate, prate, prate2);
            case CalculateType.Progression:
                return FormulaManager.CalculateByProgression(level, baseStat, rate, prate2);
            case CalculateType.Pow:
                return FormulaManager.CalculateStatByPow(level, baseStat, rate, prate);
            case CalculateType.PowMultiple:
                return FormulaManager.CalculateByPowMultiple(level, baseStat, rate, prate, minRate);
            default:
                return 0;
        }
    }
}

[System.Serializable]
public class BaseStatConfig {
    public Rarity rarity;
    public CostConfig costConfig;
}

[System.Serializable]
public class PlayerStatConfig {
    
    public CostConfig playerAttack;
    public CostConfig playerAttackSpeed;
    public CostConfig playerHP;
    public CostConfig playerHPRegen;
    public CostConfig playerCriticalChance;
    public CostConfig playerCriticalDamage;
}

[System.Serializable]
public class PlayerStatCostConfig {
    public CostConfig attackCost;
    public CostConfig attackSpeedCost;
    public CostConfig hpCost;
    public CostConfig hpRegenCost;
    public CostConfig criticalChanceCost;
    public CostConfig criticalDamageCost;
}

[System.Serializable]
public class CampaignStatConfig {
    public CostConfig campaignProfit;
    public CostConfig enemyHP;
    public CostConfig enemyAttack;
}

[System.Serializable]
public class OwnedEffectConfig {
    public List<BaseStatConfig> stickmanOwnedEffects;
    public List<BaseStatConfig> equipmentOwnedEffects;
    public List<BaseStatConfig> skillOwnedEffects;
}

public abstract class DungeonStatConfig {
    public CostConfig hp;
    public CostConfig attack;
    public CostConfig reward;

    public virtual BigNumber GetReward(int level) {
        return reward.GetResult(level);
    }
    public virtual BigNumber GetHP(int level) {
        return hp.GetResult(level);
    }
    public virtual BigNumber GetAttack(int level) {
        return attack.GetResult(level);
    }
}
[System.Serializable]
public class DungeonGoldStatConfig : DungeonStatConfig {
}

[System.Serializable]
public class DungeonBossStatConfig : DungeonStatConfig {
}

[System.Serializable]
public class DungeonDeadHunterStatConfig: DungeonStatConfig {
}

[System.Serializable]
public class DungeonMassiveShadowStatConfig : DungeonStatConfig {
}
[System.Serializable]
public class TestStatConfig : DungeonStatConfig {
    public override BigNumber GetAttack(int level) {
        return FormulaManager.CalculateByPerWithDownrate(level - 1, attack.baseStat, attack.rate, attack.minRate, attack.prate, attack.prate2);
    }

    public override BigNumber GetHP(int level) {
        return FormulaManager.CalculateByPerWithDownrate(level - 1, hp.baseStat, hp.rate, hp.minRate, hp.prate, hp.prate2);
    }

    public override BigNumber GetReward(int level) {
        float num = reward.baseStat + (level - 1) * reward.rate;
        num = num * UnityEngine.Random.Range(1f, 1.2f);
        return num;
    }
}

[System.Serializable]
public class LevelConflig
{
    public int Level;
    public int RequiredAmount;
}