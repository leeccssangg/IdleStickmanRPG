using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities;
using Sirenix.OdinInspector;

[GUIColor("@MyExtension.EditorExtension.GetColorById((int)$value,10)")]
public enum ArtifactBuffType
{
    NONE,
    SKILL,
    HP,
    ATK,
}

[CreateAssetMenu(fileName = "ArtifactsGlobalConfig", menuName = "GlobalConfigs/ArtifactsGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]

public class ArtifactsGlobalConfig : GlobalConfig<ArtifactsGlobalConfig>
{
    public List<ArtifactConfig> configs = new();
    public int costUnlockBase;
    public int costRatePerUnlock;

    public ArtifactConfig GetArtifactConfigById(int id)
    {
        for(int i = 0;i< configs.Count; i++)
        {
            if (configs[i].artifactId == id)
                return configs[i];
        }
        return null;
    }
    public int GetTotalSlotId()
    {
        return configs[^1].slotId;
    }
    public int GetCostUnlockBase()
    {
        return costUnlockBase;
    }
    public int GetCostRatePerUnlock()
    {
        return costRatePerUnlock;
    }
    public int GetCost(int numUnloced)
    {
        return costUnlockBase + (numUnloced*costRatePerUnlock);
    }

}
[System.Serializable]
public class ArtifactConfig
{
    public int artifactId;
    public int slotId;
    public string name;
    public int maxLevel;
    public List<ArtifactBuff> buffs;
    public int baseCost;
    public string description;
}
[System.Serializable]
public class ArtifactBuff
{
    public ArtifactBuffType buffType;
    public float baseStat;
    public float increaseStat;
}
