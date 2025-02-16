using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities;

[CreateAssetMenu(fileName = "DungeonsConfig", menuName = "GlobalConfigs/DungeonsConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class DungeonsConfig : GlobalConfig<DungeonsConfig>
{
    public List<DungeonConfig> listDungeons = new();

    public DungeonConfig GetDungeonConfigWithType(DungeonType type)
    {
        DungeonConfig dungeonConfig = new();
        for(int i = 0; i < listDungeons.Count; i++)
        {
            if (listDungeons[i].DType == type)
                dungeonConfig = listDungeons[i];
        }
        return dungeonConfig;
    }
}
[System.Serializable]
public class DungeonConfig
{
    public DungeonType type;
    public int numKeyADay;
    public List<DungeonLevelConfig> listLevel;
    
    public DungeonType DType { get => type; set => type = value; }
    public List<DungeonLevelConfig> LevelConfigs { get => listLevel; set => listLevel = value; }
    public int NumKeyADay { get => numKeyADay; set => numKeyADay = value; }

    public DungeonConfig()
    {
        DType = DungeonType.NONE;
        NumKeyADay = 0;
        LevelConfigs = new();
    }

    public DungeonLevelConfig GetDungeonLevelConfigByLevel(int level)
    {
        return LevelConfigs[level - 1];
    }
    public int GetLastLevel()
    {
        return LevelConfigs[^1].Level;
    }
}
[System.Serializable]
public class DungeonLevelConfig
{
    public int lv;
    public string des;
    public ResourceRewardPackage giftPackage;

    public int Level { get => lv; set => lv = value; }
    public ResourceRewardPackage Gift { get => giftPackage; set => giftPackage = value; }
    public string Description { get => des; set => des = value; }
}