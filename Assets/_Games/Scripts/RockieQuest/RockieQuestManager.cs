using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Globalization;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

public class RockieQuestManager : Pextension.Singleton<RockieQuestManager>
{
    public const int MAX_DAY_ROCKIE_QUEST = 20;
    [SerializeField] private RockieQuestData m_RockieQuestData;
    [SerializeField] private RockieQuestGlobalConfig m_RockieQuestGlobalConfig;
    [SerializeField] private RockieQuestSubject m_RockieQuestSubject;
    [SerializeField] private int m_DayLeft;
    [SerializeField] private int m_LevelGift;
    [SerializeField] private int m_CompletedPoint;
    //[SerializeField] private DateTime m_NextDay;
    [SerializeField] private DateTime m_LastDay;


    #region Save & Load Data
    public void LoadData(RockieQuestData data)
    {
        m_RockieQuestGlobalConfig = RockieQuestGlobalConfig.Instance;
        m_RockieQuestData = data;
        m_LevelGift = m_RockieQuestData.LevelGift;
        m_CompletedPoint = m_RockieQuestData.CompletedPoint;
        if(m_RockieQuestData.LastDay == null)
            StartRockieQuest();
        m_LastDay = Convert.ToDateTime(m_RockieQuestData.LastDay, CultureInfo.InvariantCulture).Date;
        //m_NextDay = Convert.ToDateTime(m_RockieQuestData.NextDay, CultureInfo.InvariantCulture).Date;
        m_DayLeft = (int)(m_LastDay - DateTime.Now.Date).TotalDays;
        if (m_DayLeft < 0)
            return;
        LoadQuests(m_RockieQuestData.QuestDatas);
        //if(!isFirstTime)
        //    UpdateNextDay();
    }
    public void LoadQuests(string jsonString = "")
    {
        m_RockieQuestSubject.LoadData(jsonString);
    }
    public void SaveData()
    {
        RockieQuestData data = new()
        {
            //NextDay = m_NextDay.ToString(CultureInfo.InvariantCulture),
            LastDay = m_LastDay.ToString(CultureInfo.InvariantCulture),
            QuestDatas = m_RockieQuestSubject.SaveData(),
        };
        ProfileManager.Ins.SaveRockieQuestData(data);
    }
    //private void UpdateNextDay()
    //{
    //    m_NextDay = Static.GetNextDate();
    //    //m_NextDay = m_NextDay.AddDays(1);
    //}
    #endregion
    #region Manager Function
    #region Quests
    public void StartRockieQuest()
    {
        RockieQuestData data = new()
        {
            LevelGift = m_LevelGift,
            CompletedPoint = m_CompletedPoint,
            //NextDay = DateTime.Now.Date.ToString(),
            LastDay = DateTime.Now.AddDays(MAX_DAY_ROCKIE_QUEST).Date.ToString(),
            QuestDatas = "",
        };
        LoadData(data);
        SaveData();
    }
    public int GetDayNum()
    {
        return MAX_DAY_ROCKIE_QUEST - m_DayLeft + 1;
    }
    //private bool IsNextDay()
    //{
    //    TimeSpan t = DateTime.Now - m_NextDay.Date;
    //    return t.TotalSeconds >= 0;
    //}
    public RockieQuestConfig GetRockieQuestConfig(int id)
    {
        return m_RockieQuestGlobalConfig.GetRockieQuestConfig(id);
    }
    public int GetNumRockieQuestConfig()
    {
        return m_RockieQuestGlobalConfig.GetNumRockieQuest();
    }
    public void NotifyQuest(MissionTarget questType, string info)
    {
        m_RockieQuestSubject.Notify(questType, info);
    }
    public void NotifyQuest(MissionTarget questType, int amount)
    {
        m_RockieQuestSubject.Notify(questType, amount.ToString());
    }
    public void ClaimQuest(RockieQuest quest)
    {
        m_RockieQuestSubject.ClaimQuest(quest);
        m_CompletedPoint++;
        SaveData();
    }
    public List<RockieQuest> GetListDailyQuestsByDay(int day)
    {
        return new List<RockieQuest>(m_RockieQuestSubject.GetListDailyQuestsByDay(day));
    }
    #endregion
    #region Checkpoint
    public int GetLevelGift()
    {
        return m_LevelGift;
    }
    public float GetPointProcess()
    {
        return (float)m_CompletedPoint / (float)RockieQuestCheckpointRewardGlobalConfig.Instance.GetMaxPointNeeded();
    }
    public bool IsReadyClaimLevelGift(int lv)
    {
        return m_CompletedPoint >= RockieQuestCheckpointRewardGlobalConfig.Instance.GetNeededPoint(lv)
            && m_LevelGift < lv;
    }
    public List<ResourceRewardPackage> ClaimLevelGift(int lv)
    {
        List<ResourceRewardPackage> list = new();
        for(int i = m_LevelGift+1;i<= lv; i++)
        {
            ResourceRewardPackage reward = RockieQuestCheckpointRewardGlobalConfig.Instance.GetReward(i);
            list.Add(reward);
            ProfileManager.PlayerData.AddGameResource(reward);
        }
        m_LevelGift = lv;
        return list;
    }
    #endregion
    #endregion
    #region Quest functions
    public void Notify(MissionTarget id, string info)
    {

    }
    #endregion
    #region Editor
#if UNITY_EDITOR
    [Button]
    private void SaveDataEditor()
    {
        SaveData();
    }
    public TextAsset m_TextCSV;
    [Button]
    public void ImportCSVData()
    {
        ImportRockieQuestCSV();
        //ImportCollectQuestCSV();
        EditorUtility.SetDirty(gameObject);
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
    }
    private void ImportRockieQuestCSV()
    {
        m_RockieQuestGlobalConfig = RockieQuestGlobalConfig.Instance;
        EditorUtility.SetDirty(m_RockieQuestGlobalConfig);
        m_RockieQuestGlobalConfig.rockieQuests.Clear();
        List<Dictionary<string, string>> datas = CSVReader.Read(m_TextCSV);
        for (int i = 0; i < datas.Count; i++)
        {
            RockieQuestConfig rockieQuestConfig = new();
            rockieQuestConfig.day = int.Parse(datas[i]["Day"]);
            rockieQuestConfig.questConfig = new()
            {
                type = QuestCollectType.FREE,
                id = int.Parse(datas[i]["ID"]),
                missionTarget = (MissionTarget)System.Enum.Parse(typeof(MissionTarget), datas[i]["QuestName"]),
                targetAmount = int.Parse(datas[i]["Target"]),
                reward = new()
                {
                    rewardType = (ResourceData.ResourceType)System.Enum.Parse(typeof(ResourceData.ResourceType), datas[i]["Reward"]),
                    amount = int.Parse(datas[i]["RewardAmount"]),
                },
                description = datas[i]["Description"],
            };
            m_RockieQuestGlobalConfig.rockieQuests.Add(rockieQuestConfig);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

#endif
    #endregion
}
[System.Serializable] 
public class RockieQuestData
{
    //public string nd;
    public string ld;
    public string qd;
    public int lg;
    public int cp;

    public int LevelGift { get => lg; set => lg = value; }
    public int CompletedPoint { get => cp; set => cp = value; }
    public string LastDay { get => ld; set => ld = value; }
    //public string NextDay { get => nd; set => nd = value; }
    public string QuestDatas { get => qd; set => qd = value; }

    public RockieQuestData()
    {
        LevelGift = 0;
        CompletedPoint = 0;
        LastDay = null;
        //NextDay = null;
        QuestDatas = "";
    }
}
