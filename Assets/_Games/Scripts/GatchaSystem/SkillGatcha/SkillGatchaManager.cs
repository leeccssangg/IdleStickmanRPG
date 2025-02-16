using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using Sirenix.Utilities;
using System.Globalization;

public class SkillGatchaManager
{
    public const int TIME_WAIT_ADS_FREE = 60;
    [SerializeField] private SkillGatchaGlobalConfig m_GlobalConfig;
    [SerializeField] private SkillGatchaData m_GatchaData;
    [SerializeField] private LevelSkillGatchaData m_LevelGatchaData;
    [SerializeField] private GatchaButtonData m_GatchaButtonData; 
    [SerializeField] private int m_GatchaLevel;
    [SerializeField] private int m_CurrentPoint;
    [SerializeField] private int m_MaxGatchaLevel;
    [SerializeField] private int m_AdsFree;
    [SerializeField] private const int m_MaxAdsFree = 5;
    [SerializeField] private int m_NumOpenBox1;
    [SerializeField] private int m_NumOpenBox2;
    private DateTime m_NextGatchaDay;
    private DateTime m_NextFreeAdsTime;

    public void LoadData(SkillGatchaData data)
    {
        m_GlobalConfig = SkillGatchaGlobalConfig.Instance;
        m_NumOpenBox1 = m_GlobalConfig.numBox1;
        m_NumOpenBox2 = m_GlobalConfig.numBox2;
        m_MaxGatchaLevel = m_GlobalConfig.levelConfig[^1].level;
        m_GatchaData = data;
        m_LevelGatchaData = m_GatchaData.LevelGatchaData;
        m_GatchaButtonData = m_GatchaData.ButtonData;
        m_NextGatchaDay = Convert.ToDateTime(m_LevelGatchaData.NextDay,CultureInfo.InvariantCulture);
        if (IsNextDay())
            InitDataOnNewDay();
        else
            InitDataOnOldDay();
        m_GatchaLevel = m_LevelGatchaData.Level;
        m_CurrentPoint = m_LevelGatchaData.Point;
        UpdateNextDay();
    }
    private void InitDataOnNewDay()
    {
        m_AdsFree = 5;
        m_NextFreeAdsTime = DateTime.Now;
    }
    private void InitDataOnOldDay()
    {
        m_AdsFree = m_GatchaButtonData.AdsFree;
        m_NextFreeAdsTime = Convert.ToDateTime(m_GatchaButtonData.NextGatchaTime,CultureInfo.InvariantCulture);
    }
    public SkillGatchaData SaveData()
    {
        GatchaButtonData gatchaButtonData = new GatchaButtonData()
        {
            AdsFree = m_AdsFree,
            NextGatchaTime = m_NextFreeAdsTime.ToString(CultureInfo.InvariantCulture),
        };
        LevelSkillGatchaData levelGatchaData = new LevelSkillGatchaData()
        {
            Level = m_GatchaLevel,
            Point = m_CurrentPoint,
            NextDay = m_NextGatchaDay.ToString(CultureInfo.InvariantCulture),
        };
        SkillGatchaData skillGatchaData = new SkillGatchaData(levelGatchaData, gatchaButtonData);
        return skillGatchaData;
    }
    private void UpdateNextDay()
    {
        m_NextGatchaDay = Static.GetNextDate();
    }
    private void UpdateNextFreeTime()
    {
        m_NextFreeAdsTime = DateTime.Now.AddSeconds(TIME_WAIT_ADS_FREE);
    }
    public string GetTimeToNextFree()
    {
        DateTime now = DateTime.Now;
        TimeSpan timeSpan = m_AdsFree > 0 ? m_NextFreeAdsTime - now : m_NextGatchaDay - now;
        string time = Static.GetTimeStringFromSecond(timeSpan.TotalSeconds);
        return time;
    }
    private bool IsNextDay()
    {
        return (m_NextGatchaDay.Date <= DateTime.Now.Date);
    }
    public bool IsFree()
    {
        TimeSpan timeSpan = DateTime.Now - m_NextFreeAdsTime;
        return m_AdsFree > 0 && timeSpan.TotalSeconds >= 0;
    }
    public List<SkillConfig> GoGatcha(int num, GatchaButtonType btnType)
    {
        List<SkillConfig> list = new();
        for (int i = 0; i < num; i++)
        {
            Rarity rarity = m_GlobalConfig.levelConfig[m_GatchaLevel - 1].rarityConfig.GetRandomItem();
            Debug.Log(rarity);
            SkillConfig skillConfig = SkillManager.Ins.GetRandomSkillConfigWithRarity(rarity);
            list.Add(skillConfig);
            UpgradeGatcha();
        }
        //List<CardConfig> bonusList = BonusCard(btnType);
        //for (int j = 0; j < bonusList.Count; j++)
        //{
        //    list.Add(bonusList[j]);
        //}
        if (btnType == GatchaButtonType.Ads)
        {
            ReduceAdsFree();
            UpdateNextFreeTime();
        }
        AddGatchaData(list);
        return list;
    }
    private void AddGatchaData(List<SkillConfig> list)
    {
        SkillManager.Ins.AddSkillPiece(list);
    }
    private void ReduceAdsFree()
    {
        m_AdsFree--;
        if (m_AdsFree < 0)
            m_AdsFree = 0;
    }
    //public List<CardConfig> BonusCard(GatchaButtonType type)
    //{
    //    List<CardConfig> list = new();
    //    switch (type)
    //    {
    //        case GatchaButtonType.Free:
    //        case GatchaButtonType.Ads:
    //            list.Clear();
    //            break;
    //        case GatchaButtonType.Rare:
    //            for(int j = 0;j< 3; j++)
    //            {
    //                list.Add(CardCollectionManager1.Ins.GetRandomCardWithType(GameRarity.Type.Rare));
    //                UpgradeGatcha();
    //            }
    //            break;
    //        case GatchaButtonType.Epic:
    //            for (int j = 0; j < 5; j++)
    //            {
    //                list.Add(CardCollectionManager1.Ins.GetRandomCardWithType(GameRarity.Type.Epic));
    //                UpgradeGatcha();
    //            }
    //            break;
    //    }
    //    return list;
    //}
    private void UpgradeGatcha()
    {
        m_CurrentPoint++;
        if (m_GatchaLevel >= m_MaxGatchaLevel)
            return;
        if (m_CurrentPoint >= m_GlobalConfig.levelConfig[m_GatchaLevel - 1].upgradeNeeded)
        {
            m_GatchaLevel++;
            //m_CurrentUpgraded = 0;
        }
    }
    public SkillGatchaLevelConfig GetCurrentLevelConfig()
    {
        return m_GlobalConfig.levelConfig[m_GatchaLevel];
    }
    public GatchaButtonData GetGatchaButtonDatas()
    {
        return m_GatchaButtonData;
    }
    public int GetGatchaLevel()
    {
        return m_GatchaLevel;
    }
    public int GetCurrentPoint()
    {
        return m_CurrentPoint;
    }
    public int GetCurrentUpgradeNeed()
    {
        return m_GlobalConfig.levelConfig[m_GatchaLevel - 1].upgradeNeeded;
    }
    public float GetProcessUpgraded()
    {
        return ((float)m_CurrentPoint / (float)m_GlobalConfig.levelConfig[m_GatchaLevel - 1].upgradeNeeded);
    }  
    public int GetNumOpenBox1()
    {
        return m_NumOpenBox1;
    }
    public int GetNumOpenBox2()
    {
        return m_NumOpenBox2;
    }
    public int GetAdsLeft()
    {
        return m_AdsFree;
    }
    public int GetMaxAds()
    {
        return m_MaxAdsFree;
    }
}
[System.Serializable]
public class SkillGatchaData
{
    public LevelSkillGatchaData lD;
    public GatchaButtonData bD;

    public LevelSkillGatchaData LevelGatchaData { get => lD; set => lD = value; }
    public GatchaButtonData ButtonData { get => bD; set => bD = value; }
    public SkillGatchaData()
    {
        lD = new LevelSkillGatchaData();
        bD = new GatchaButtonData();
    }
    public SkillGatchaData(LevelSkillGatchaData lv, GatchaButtonData btn)
    {
        LevelGatchaData = lv;
        ButtonData = btn;
    }
}
[System.Serializable]
public class LevelSkillGatchaData
{
    public int lv;
    public int pt;
    public string nD;

    public int Level { get => lv; set => lv = value; }
    public int Point { get => pt; set => pt = value; }
    public string NextDay { get => nD; set => nD = value; }

    public LevelSkillGatchaData()
    {
        lv = 1;
        pt = 0;
        nD = DateTime.Now.ToString(CultureInfo.InvariantCulture);
    }
}
