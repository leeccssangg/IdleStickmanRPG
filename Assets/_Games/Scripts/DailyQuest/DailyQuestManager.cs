using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Globalization;

public class DailyQuestManager : Pextension.Singleton<DailyQuestManager>
{
    [SerializeField] private DailyQuestData m_DailyQuestData;
    [SerializeField] private DailyQuestConfigs m_DailyQuestConfigs;
    [SerializeField] private DailyQuestSubject m_DailyQuestSubject;
    private DateTime m_NextDay;

    #region Save & Load Data
    public void LoadData(DailyQuestData data)
    {
        m_DailyQuestConfigs = DailyQuestConfigs.Instance;
        m_DailyQuestData = data;
        m_NextDay = Convert.ToDateTime(m_DailyQuestData.NextDay, CultureInfo.InvariantCulture);
        if (IsNextDay())
            LoadQuests();
        else
            LoadQuests(m_DailyQuestData.QuestDatas);
        UpdateNextDay();
    }
    public void LoadQuests(string jsonString = "")
    {
        m_DailyQuestSubject.LoadData(jsonString);
    }
    public void SaveData()
    {
        DailyQuestData data = new()
        {
            NextDay = m_NextDay.ToString(CultureInfo.InvariantCulture),
            QuestDatas = m_DailyQuestSubject.SaveData(),
        };
        ProfileManager.Ins.SaveDailyQuestData(data);
    }
    #endregion
    #region Manager Function
    private bool IsNextDay()
    {
        TimeSpan t = DateTime.Now - m_NextDay.Date;
        return t.TotalSeconds >= 0;
    }
    private void UpdateNextDay()
    {
        m_NextDay = Static.GetNextDate();
    }
    public QuestConfig GetDailyQuestConfig(int id)
    {
        return m_DailyQuestConfigs.GetDailyQuestConfig(id);
    }
    public int GetNumDailyQuestConfig()
    {
        return m_DailyQuestConfigs.GetNumDailyQuestConfig();
    }
    public void NotifyQuest(MissionTarget questType, string info)
    {
        m_DailyQuestSubject.Notify(questType, info);
    }
    public void NotifyQuest(MissionTarget questType, int amount)
    {
        m_DailyQuestSubject.Notify(questType, amount.ToString());
    }
    public void ClaimQuest(DailyQuest quest)
    {
        m_DailyQuestSubject.ClaimQuest(quest);
        SaveData();
    }
    public List<DailyQuest> GetListDailyQuests()
    {
        return m_DailyQuestSubject.GetListDailyQuests();
    }
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
#endif
    #endregion
}
[System.Serializable]
public class DailyQuestData
{
    public string qdl;
    public string nd;

    public string QuestDatas { get => qdl; set => qdl = value; }
    public string NextDay { get => nd; set => nd = value; }

    public DailyQuestData()
    {
        QuestDatas = "";
        NextDay = DateTime.Now.ToString(CultureInfo.InvariantCulture);
    }

}
