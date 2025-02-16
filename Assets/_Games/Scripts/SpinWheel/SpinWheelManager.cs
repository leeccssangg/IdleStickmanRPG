using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using EasyUI.PickerWheelUI;
using System;
using System.Globalization;

public class SpinWheelManager : Pextension.Singleton<SpinWheelManager>
{
    
    [SerializeField] private SpinWheelConfigs m_SpinWheelConfigs;
    [SerializeField] private SpinWheelData m_SpinWheelData;
    [SerializeField] private int m_WheelLevel;
    [SerializeField] private int m_CurrentUpgraded;
    private DateTime m_NextDay;
    private DateTime m_NextSpinDay;

    public void LoadData(SpinWheelData spinWheelData)
    {
        m_SpinWheelConfigs = SpinWheelConfigs.Instance;
        m_SpinWheelData = spinWheelData;
        m_WheelLevel = m_SpinWheelData.lv;
        m_CurrentUpgraded = m_SpinWheelData.up;
        m_NextSpinDay = Convert.ToDateTime(m_SpinWheelData.nD, CultureInfo.InvariantCulture);
        UpdateNextDay();
    }
    public void SaveData()
    {
        SpinWheelData spinWheelData = new SpinWheelData {
            Level = m_WheelLevel,
            Uprgade = m_CurrentUpgraded,
            Date = m_NextSpinDay.ToString(CultureInfo.InvariantCulture),
        }
        ;
        ProfileManager.Ins.SaveSpinWheelData(spinWheelData);
    }
    private void UpdateNextDay()
    {
        m_NextDay = Static.GetNextDate();
    }
    public string GetTimeToNextDay()
    {
        DateTime now = DateTime.Now;
        TimeSpan timeSpan = m_NextDay - now;
        string time = Static.GetTimeStringFromSecond(timeSpan.TotalSeconds);
        return time;
    }
    public bool IsFree()
    {
        TimeSpan timeSpan = DateTime.Now - m_NextSpinDay;
        return timeSpan.TotalSeconds >= 0;
    }
    public void Upgrade()
    {
        m_CurrentUpgraded = 0;
        m_WheelLevel++;
    }
    public int GetSpinWheelLevel()
    {
        return m_WheelLevel;
    }
    public int GetCurrentUpgraded()
    {
        return m_CurrentUpgraded;
    }
    public SpinWheelConfigs GetSpinWheelConfigs()
    {
        return m_SpinWheelConfigs;
    }
    public float GetProcessUpgraded(int upgraded)
    {
        m_CurrentUpgraded = upgraded;
        return ((float)m_CurrentUpgraded / (float)m_SpinWheelConfigs.SpinWheelDataConfigs[m_WheelLevel].m_UpgradeRequire);
    }
    public int GetNumUpgradedNeed()
    {
        return (m_SpinWheelConfigs.SpinWheelDataConfigs[m_WheelLevel].m_UpgradeRequire);
    }
    public void ClaimReward(WheelDataConfigs<GiftType> rewardData , bool isFree)
    {
        Debug.Log(rewardData.m_ItemType);
        Debug.Log(rewardData.m_Amount);
        if(isFree)
            m_NextSpinDay = m_NextDay;
            //m_NextSpinDay = DateTime.Now;
    }
}
[System.Serializable]
public class SpinWheelData
{
    public int lv ;
    public int up ;
    public string nD;

    public int Level { get => lv; set => lv = value; }
    public int Uprgade { get => up; set => up = value; }
    public string Date { get => nD; set => nD = value; }

    public SpinWheelData()
    {
        lv = 0;
        up = 0;
        nD = DateTime.Now.ToString();
    }
    public SpinWheelData(int level, int upgraded)
    {
        lv = level;
        up = upgraded;
    }
}
[System.Serializable]
public class SpinWheelDataConfigs<T>
{
    public int m_WheelLevel;
    public int m_UpgradeRequire;
    public int m_GemPerSpin;
    public List<WheelDataConfigs<T>> m_WheelDatasConfigs;
}

[System.Serializable]
public class WheelDataConfigs<T>
{
    public Sprite m_Icon;
    public T m_ItemType;
    public float m_Amount;
    [Range(0f,100f)]
    public float m_Chance;
    [HideInInspector] public int m_Index;
    [HideInInspector] public double _weight = 0f;
}
