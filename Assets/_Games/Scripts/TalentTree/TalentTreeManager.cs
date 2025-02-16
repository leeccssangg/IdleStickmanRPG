using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class TalentTreeManager : Pextension.Singleton<TalentTreeManager>
{
    [SerializeField] private TalentTreeSaveData m_TalentTreeSaveData;

    public void LoadData(TalentTreeSaveData data)
    {
        m_TalentTreeSaveData = data;
        TalentTreeDynamicData.Instance.OnLoadData(data.DataPlayerStatsTalentTree);
    }
    public void SaveData()
    {
        m_TalentTreeSaveData = new()
        {
            DataPlayerStatsTalentTree = TalentTreeDynamicData.Instance.OnSaveData(),
        };
        ProfileManager.Ins.SavePlayerStatsTalentTreeData(m_TalentTreeSaveData);
    }
}
[System.Serializable]
public class TalentTreeSaveData
{
    public string pstd;

    public string DataPlayerStatsTalentTree { get => pstd; set => pstd = value; }

    public TalentTreeSaveData()
    {
        DataPlayerStatsTalentTree = "";
    }
}
[System.Serializable]
public class PlayerStatsTalentTreeSaveData
{
    public int csp;
    public int usp;
    public List<int> tnl;

    public int CurrentSkillPoint { get => csp; set => csp = value; }
    public int UsedSkillPoint { get => usp; set => usp = value; }
    public List<int> TalentNodeLines { get => tnl; set => tnl = value; }
    public PlayerStatsTalentTreeSaveData()
    {
        
    }
    public PlayerStatsTalentTreeSaveData(TalentTreeDynamicData data)
    {
        CurrentSkillPoint = data.m_CurrentSkillPoint;
        UsedSkillPoint = data.m_CurrentUsedSkillPoint;
        TalentNodeLines = data.m_TalentNodeLines;
    }
}
