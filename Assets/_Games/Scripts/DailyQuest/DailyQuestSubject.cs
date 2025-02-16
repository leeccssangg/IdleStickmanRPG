using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DailyQuestSubject : Subject<MissionTarget>
{
    [SerializeField] private List<DailyQuest> m_DailyQuests = new();

    #region Load & Save Data
    public void LoadData(string jsonData = "")
    {
        if (!string.IsNullOrEmpty(jsonData))
            LoadOldData(jsonData);
        else
            LoadNewData();
    }
    private void LoadNewData()
    {
        Debug.Log("Load New Daily Quest Data");
        for (int i = 1; i <= DailyQuestManager.Ins.GetNumDailyQuestConfig(); i++)
        {
            QuestConfig questConfig = DailyQuestManager.Ins.GetDailyQuestConfig(i);
            DailyQuest dailyQuest = GenerateDailyQuest(questConfig.missionTarget);
            dailyQuest.Init(questConfig.id);
            m_DailyQuests.Add(dailyQuest);
            AddObserver(dailyQuest);
        }
        SaveData();
    }
    private void LoadOldData(string jsonData)
    {
        Debug.Log("Load Old Daily Quest Data");
        List<DailyQuest> rawList = JsonConvert.DeserializeObject<List<DailyQuest>>(jsonData);
        for (int i = 0; i < rawList.Count; i++)
        {
            DailyQuest dailyQuest = rawList[i];
            dailyQuest.InitQuestConfig();
            string rawString = JsonConvert.SerializeObject(dailyQuest);
            DailyQuest newQuest = DeserializeQuest(rawString, dailyQuest.GetMissionTarget());
            newQuest.InitQuestConfig();
            m_DailyQuests.Add(newQuest);
            AddObserver(newQuest);
        }
    }
    public string SaveData()
    {
        return JsonConvert.SerializeObject(m_DailyQuests);
    }
    public DailyQuest GenerateDailyQuest(MissionTarget missionTarget)
    {
        DailyQuest newDailyQuest = null;
        switch (missionTarget)
        {
            case MissionTarget.DEFEAT_ENEMY:
                {
                    DailyQuest_DefeatEnemy newQuest = new();
                    newDailyQuest = newQuest;
                }
                break;
            case MissionTarget.CLEAR_STAGE:
                {
                    DailyQuest_ClearStage newQuest = new();
                    newDailyQuest = newQuest;
                }
                break;
            case MissionTarget.CLEAR_DUNGEON_BOSS_RUSH:
                {
                    DailyQuest_ClearDungeonBossRush newQuest = new();
                    newDailyQuest = newQuest;
                }
                break;
            case MissionTarget.CLEAR_DUNGEON_GOLD_RUSH:
                {
                    DailyQuest_ClearDungeonGoldRush newQuest = new();
                    newDailyQuest = newQuest;
                }
                break;
            case MissionTarget.CLEAR_DUNGEON_MONSTER_NEST:
                {
                    DailyQuest_ClearDungeonMonsterNest newQuest = new();
                    newDailyQuest = newQuest;
                }
                break;
            case MissionTarget.CLEAR_DUNGEON_CHALLENGE_KING:
                {
                    DailyQuest_ClearDungeonChallengeKing newQuest = new();
                    newDailyQuest = newQuest;
                }
                break;
            case MissionTarget.SUMMON:
                {
                    DailyQuest_Summon newQuest = new();
                    newDailyQuest = newQuest;
                }
                break;
            case MissionTarget.WATCH_ADS:
                {
                    DailyQuest_WatchAds newQuest = new();
                    newDailyQuest = newQuest;
                }
                break;
            case MissionTarget.COMPLETE_ALL_DAILY_QUEST:
                {
                    DailyQuest_CompleteAllDailyQuest newQuest = new();
                    newDailyQuest = newQuest;
                }
                break;
        }
        return newDailyQuest;
    }
    public DailyQuest DeserializeQuest(string json, MissionTarget missionTarget)
    {
        DailyQuest dailyQuest = null;
        switch (missionTarget)
        {
            case MissionTarget.DEFEAT_ENEMY:
                {
                    dailyQuest = JsonConvert.DeserializeObject<DailyQuest_DefeatEnemy>(json);
                }
                break;
            case MissionTarget.CLEAR_STAGE:
                {
                    dailyQuest = JsonConvert.DeserializeObject<DailyQuest_ClearStage>(json);
                }
                break;
            case MissionTarget.CLEAR_DUNGEON_BOSS_RUSH:
                {
                    dailyQuest = JsonConvert.DeserializeObject<DailyQuest_ClearDungeonBossRush>(json);
                }
                break;
            case MissionTarget.CLEAR_DUNGEON_GOLD_RUSH:
                {
                    dailyQuest = JsonConvert.DeserializeObject<DailyQuest_ClearDungeonGoldRush>(json);
                }
                break;
            case MissionTarget.CLEAR_DUNGEON_MONSTER_NEST:
                {
                    dailyQuest = JsonConvert.DeserializeObject<DailyQuest_ClearDungeonMonsterNest>(json);
                }
                break;
            case MissionTarget.CLEAR_DUNGEON_CHALLENGE_KING:
                {
                    dailyQuest = JsonConvert.DeserializeObject<DailyQuest_ClearDungeonChallengeKing>(json);
                }
                break;
            case MissionTarget.SUMMON:
                {
                    dailyQuest = JsonConvert.DeserializeObject<DailyQuest_Summon>(json);
                }
                break;
            case MissionTarget.WATCH_ADS:
                {
                    dailyQuest = JsonConvert.DeserializeObject<DailyQuest_WatchAds>(json);
                }
                break;
            case MissionTarget.COMPLETE_ALL_DAILY_QUEST:
                {
                    dailyQuest = JsonConvert.DeserializeObject<DailyQuest_CompleteAllDailyQuest>(json);
                }
                break;
        }
        return dailyQuest;
    }
    #endregion
    #region Daily Quest Function
    public override void Notify(MissionTarget id, string info)
    {
        base.Notify(id, info);
        SaveData();
    }
    public void ClaimQuest(DailyQuest dailyQuest)
    {
        QuestConfig questConfig = dailyQuest.GetQuestConfig();
        AddReward(questConfig.reward);
        dailyQuest.OnClaim();
    }
    public void AddReward(ResourceRewardPackage itemPackage)
    {
        ProfileManager.PlayerData.AddGameResource(itemPackage);
    }
    public List<DailyQuest> GetListDailyQuests()
    {
        return new List<DailyQuest>(m_DailyQuests);
    }
    #endregion

}
