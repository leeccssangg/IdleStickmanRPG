using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities;
using Sirenix.OdinInspector;

[GUIColor("@MyExtension.EditorExtension.GetColor(\"DailyQuest\", (int)$value)")]
public enum QuestType
{
    DAILY,
    ROOKIE,
    ACHIEVEMENT,
    MAINQUEST,
    NONE = -1,
}
[GUIColor("@MyExtension.EditorExtension.GetColor(\"DailyQuest\", (int)$value)")]
public enum QuestCollectType
{
    FREE,
    ADS,
    NONE = -1,
}
[GUIColor("@MyExtension.EditorExtension.GetColorById((int)$value,50)")]
public enum MissionTarget
{
    NONE,
    DEFEAT_ENEMY,
    CLEAR_STAGE,
    CLEAR_DUNGEON_BOSS_RUSH,
    CLEAR_DUNGEON_GOLD_RUSH,
    CLEAR_DUNGEON_MONSTER_NEST,
    CLEAR_DUNGEON_CHALLENGE_KING,
    SUMMON,
    WATCH_ADS,
    COMPLETE_ALL_DAILY_QUEST,
    DAILY_LOGIN,
    PASS_LEVEL,
    UPGRADE_ATK,
    UPGRADE_HP,
    UPGRADE_HP_REGEN,
    UPGRADE_ASPD,
    SUMMON_EQUIPMENT,
    SUMMON_SKILL,
    SUMMON_STICKMAN,
    OPEN_SUPPLY_BOX,
    USE_PICKAXE,
    USE_DRILL,
    USE_BOMB,
    RESEARCH_COMPLETE,
    MINE_DEEP,
    OBTAIN_IDLE_REWARD,
    USE_BOOSTER,
    ENHANCE_RING,
    DISMANTLE_RING,
    CLEAR_DUNGEON_RING_RAID,
    UPGRADE_CRIT_CHANCE,
    UPGRADE_CRIT_DMG,
    CLEAR_DUNGEON_STONE_RAID,
    OBTAIN_HEXCORE,
    UPGRADE_HEXCORE,
    LEVELUP_HERO,

}

[CreateAssetMenu(fileName = "DailyQuestConfigs", menuName = "GlobalConfigs/DailyQuestConfigs")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
[System.Serializable]
public class DailyQuestConfigs : GlobalConfig<DailyQuestConfigs>
{
    public List<QuestConfig> dailyQuestConfigs = new List<QuestConfig>();

    public int GetNumDailyQuestConfig()
    {
        return dailyQuestConfigs.Count;
    }
    public QuestConfig GetDailyQuestConfig(int id)
    {
        for (int i = 0; i < dailyQuestConfigs.Count; i++)
        {
            QuestConfig config = dailyQuestConfigs[i];
            if (config.id == id)
            {
                return config;
            }
        }
        return null;
    }
    public QuestConfig GetDailyQuestConfig(MissionTarget missionTarget)
    {
        for (int i = 0; i < dailyQuestConfigs.Count; i++)
        {
            QuestConfig config = dailyQuestConfigs[i];
            if (config.missionTarget == missionTarget)
            {
                return config;
            }
        }
        return null;
    }
    public List<QuestConfig> GetDailyQuestConfigs()
    {
        return new List<QuestConfig>(dailyQuestConfigs);
    }
}
[System.Serializable]
public class QuestConfig
{
    public int id;
    public QuestCollectType type;
    public MissionTarget missionTarget;
    public int targetAmount;
    public ResourceRewardPackage reward;
    public string description;

    public string GetDescription()
    {
        return "";
    }
}
[System.Serializable]
public class QuestData
{
    public int id;
    public int cl;

    public int Id { get => id; set => id = value; }
    public int Complete { get => cl; set => cl = value; }

    public QuestData()
    {

    }
    public QuestData(QuestConfig questConfig)
    {
        Id = questConfig.id;
        Complete = -1;
    }
    public QuestData(QuestConfig questConfig, int completed)
    {
        Id = questConfig.id;
        Complete = completed;
    }
}
