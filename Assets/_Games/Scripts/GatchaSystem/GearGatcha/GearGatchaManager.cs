using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class GearGatchaManager 
{
    public const int TIME_WAIT_ADS_FREE = 60;
    [SerializeField] private GearGatchaGlobalConfig m_GlobalConfig;
    [SerializeField] private GearGatchaData m_GatchaData;
    [SerializeField] private LevelGearGatchaData m_LevelGatchaData;
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

    #region SaveLoad
    public void LoadData(GearGatchaData data)
    {
        m_GlobalConfig = GearGatchaGlobalConfig.Instance;
        m_NumOpenBox1 = m_GlobalConfig.numBox1;
        m_NumOpenBox2 = m_GlobalConfig.numBox2;
        m_MaxGatchaLevel = m_GlobalConfig.levelConfig[^1].level;
        m_GatchaData = data;
        m_LevelGatchaData = m_GatchaData.LevelGatchaData;
        m_GatchaButtonData = m_GatchaData.ButtonData;
        m_NextGatchaDay = Convert.ToDateTime(m_LevelGatchaData.NextDay, CultureInfo.InvariantCulture);
        if (IsNextDay())
            InitDataOnNewDay();
        else
            InitDataOnOldDay();
        m_GatchaLevel = m_LevelGatchaData.Level;
        m_CurrentPoint = m_LevelGatchaData.Point;
        UpdateNextDay();
    }
    public void InitDataOnNewDay()
    {
        m_AdsFree = 5;
        m_NextFreeAdsTime = DateTime.Now;
    }
    public void InitDataOnOldDay()
    {
        m_AdsFree = m_GatchaButtonData.AdsFree;
        m_NextFreeAdsTime = Convert.ToDateTime(m_GatchaButtonData.NextGatchaTime, CultureInfo.InvariantCulture);
    }
    public GearGatchaData SaveData()
    {
        GatchaButtonData gatchaButtonData = new GatchaButtonData()
        {
            AdsFree = m_AdsFree,
            NextGatchaTime = m_NextFreeAdsTime.ToString(CultureInfo.InvariantCulture),
        };
        LevelGearGatchaData levelGatchaData = new LevelGearGatchaData
        {
            Level = m_GatchaLevel,
            Point = m_CurrentPoint,
            NextDay = m_NextGatchaDay.ToString(CultureInfo.InvariantCulture),
        };
        GearGatchaData cardGatchaData = new GearGatchaData(levelGatchaData, gatchaButtonData);
        return cardGatchaData;
    }
    #endregion
    #region Manager functions
    #region Update data
    public void UpdateNextDay()
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
    public List<GearConfig> GoGatcha(int num, GatchaButtonType btnType)
    {
        List<GearConfig> list = new();
        for (int i = 0; i < num; i++)
        {
            Rarity rarity = m_GlobalConfig.levelConfig[m_GatchaLevel-1].rarityConfig.GetRandomItem();
            Debug.Log(rarity);
            GearConfig gearConfig = GearManager.Ins.GetRandomGearByRarity(rarity);
            list.Add(gearConfig);
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
    private void AddGatchaData(List<GearConfig> list)
    {
        GearManager.Ins.AddGearPiece(list);
    }
    public void ReduceAdsFree()
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
        else if (m_CurrentPoint >= m_GlobalConfig.levelConfig[m_GatchaLevel-1].upgradeNeeded)
        {
            m_GatchaLevel++;
            //m_CurrentUpgraded = 0;
        }
    }
    #endregion
    #region Get data
    public bool IsNextDay()
    {
        return (m_NextGatchaDay.Date <= DateTime.Now.Date);
    }
    public bool IsFree()
    {
        TimeSpan timeSpan = DateTime.Now - m_NextFreeAdsTime;
        return m_AdsFree > 0 && timeSpan.TotalSeconds >= 0;
    }
    public GearGatchaLevelConfig GetCurrentLevelConfig()
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
    #endregion
    #endregion
}

[System.Serializable]
public class GearGatchaData
{
    public LevelGearGatchaData lD;
    public GatchaButtonData bD;

    public LevelGearGatchaData LevelGatchaData { get => lD; set => lD = value; }
    public GatchaButtonData ButtonData { get => bD; set => bD = value; }
    public GearGatchaData()
    {
        lD = new LevelGearGatchaData();
        bD = new GatchaButtonData();
    }
    public GearGatchaData(LevelGearGatchaData lv, GatchaButtonData btn)
    {
        LevelGatchaData = lv;
        ButtonData = btn;
    }
}
[System.Serializable]
public class LevelGearGatchaData
{
    public int lv;
    public int pt;
    public string nD;

    public int Level { get => lv; set => lv = value; }
    public int Point { get => pt; set => pt = value; }
    public string NextDay { get => nD; set => nD = value; }

    public LevelGearGatchaData()
    {
        lv = 1;
        pt = 0;
        nD = DateTime.Now.ToString(CultureInfo.InvariantCulture);
    }
}

