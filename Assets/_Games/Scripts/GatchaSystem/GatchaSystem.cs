using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class GatchaSystem : Singleton<GatchaSystem>
{
    [SerializeField] private int m_GatchaLevel;
    [SerializeField] private int m_CurrentUpgraded;
    [SerializeField] private int m_MaxGatchaLevel;
    private DateTime m_NextGatchaDay;
    
    public virtual void LoadData()
    {

    }
    public virtual void SaveData()
    {

    }
    public virtual void UpdateNextDay()
    {
        //m_NextGatchaDay = DateTime.Now;
        m_NextGatchaDay = Static.GetNextDate();
    }
    public string GetTimeToNextDay()
    {
        DateTime now = DateTime.Now;
        TimeSpan timeSpan = m_NextGatchaDay - now;
        string time = Static.GetTimeStringFromSecond(timeSpan.TotalSeconds);
        return time;
    }
    public bool IsFree()
    {
        TimeSpan timeSpan = DateTime.Now - m_NextGatchaDay;
        return timeSpan.TotalSeconds >= 0;
    }
    public int GetGatchaLevel()
    {
        return m_GatchaLevel;
    }
    public int GetCurrentUpgraded()
    {
        return m_CurrentUpgraded;
    }
    public int GetMaxLevel()
    {
        return m_MaxGatchaLevel;
    }
}
