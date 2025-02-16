using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "RockieQuestGlobalConfig", menuName = "GlobalConfigs/RockieQuestGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
[System.Serializable]
public class RockieQuestGlobalConfig : GlobalConfig<RockieQuestGlobalConfig>
{
    public List<RockieQuestConfig> rockieQuests = new();
    public RockieQuestConfig GetRockieQuestConfig(int id)
    {
        for (int i = 0; i < rockieQuests.Count; i++)
        {
            if (rockieQuests[i].questConfig.id == id)
                return rockieQuests[i];
        }
        return null;
    }
    public QuestConfig GetRockieQuestConfig(int day, MissionTarget target)
    {
        for (int i = 0; i < rockieQuests.Count; i++)
        {
            if (rockieQuests[i].day == day && rockieQuests[i].questConfig.missionTarget == target)
                return rockieQuests[i].questConfig;
        }
        return null;
    }
    public int GetNumRockieQuest()
    {
        return rockieQuests.Count;
    }
    public List<RockieQuestConfig> GetRockieQuestsByDay(int day)
    {
        List<RockieQuestConfig> list = new();
        for (int i = 0; i < rockieQuests.Count; i++)
        {
            if (rockieQuests[i].day == day)
                list.Add(rockieQuests[i]);
        }
        return list;
    }
}
//[System.Serializable]
//public class RockieQuestEachDayConfig
//{
//    public int day;
//    public List<QuestConfig> rockieQuestConfigs = new();
//}
[System.Serializable]
public class RockieQuestConfig
{
    public int day;
    public QuestConfig questConfig;
}

