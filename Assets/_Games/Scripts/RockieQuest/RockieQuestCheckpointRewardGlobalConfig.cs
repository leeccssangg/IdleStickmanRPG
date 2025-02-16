using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "RockieQuestCheckpointRewardGlobalConfig", menuName = "GlobalConfigs/RockieQuestCheckpointRewardGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
[System.Serializable]
public class RockieQuestCheckpointRewardGlobalConfig : GlobalConfig<RockieQuestCheckpointRewardGlobalConfig>
{
    public List<RockieQuestCheckpointRewardConfig> configs = new();

    public RockieQuestCheckpointRewardConfig GetRockieQuestCheckpointRewardConfig(int lv)
    {
        for(int i = 0;i< configs.Count; i++)
        {
            if (configs[i].lv == lv)
                return configs[i];
        }
        return null;
    }
    public int GetNeededPoint(int lv)
    {
        return GetRockieQuestCheckpointRewardConfig(lv).needed;
    }
    public ResourceRewardPackage GetReward(int lv)
    {
        return GetRockieQuestCheckpointRewardConfig(lv).reward;
    }
    public int GetMaxPointNeeded()
    {
        return configs[^1].needed;
    }
}
[System.Serializable]
public class RockieQuestCheckpointRewardConfig
{
    public int lv;
    public int needed;
    public ResourceRewardPackage reward;
}
