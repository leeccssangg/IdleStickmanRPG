using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Globalization;

public class MineResearchManager : Pextension.Singleton<MineResearchManager>
{
    [SerializeField] private MineResearchStaticData m_TalenTreeStaticData;
    [SerializeField] private MineResearchDynamicData m_TalenTreeDynamicData;
    [SerializeField] private MineResearchSaveData m_TalentTreeSaveData;
    [SerializeField] private DateTime m_StartTime;
    [SerializeField] private int m_ResearchLine;
    [SerializeField] private int m_ResearchId;

    [SerializeField] private int m_CompleteResearchCost;
    [SerializeField] private int m_TimeDecreaseAds;
    [SerializeField] private MineResearchNode m_LastResearchNode;

    public void LoadData(MineResearchSaveData data)
    {
        m_TalenTreeDynamicData = MineResearchDynamicData.Instance;
        m_TalentTreeSaveData = data;
        m_ResearchLine = m_TalentTreeSaveData.ResearchLine;
        m_ResearchId = m_TalentTreeSaveData.ResearchId;
        m_StartTime = Convert.ToDateTime(m_TalentTreeSaveData.StartTime, CultureInfo.InvariantCulture);
        m_TalenTreeDynamicData.OnLoadData(m_TalentTreeSaveData.MineResearchData);
    }
    public void SaveData()
    {
        m_TalentTreeSaveData = new()
        {
            MineResearchData = m_TalenTreeDynamicData.OnSaveData(),
            ResearchLine = m_ResearchLine,
            ResearchId = m_ResearchId,
            StartTime = m_StartTime.ToString(CultureInfo.InvariantCulture),
        };
        ProfileManager.Ins.SaveMineResearchData(m_TalentTreeSaveData);
        Debug.Log("save mine research");
    }
    public MineResearchNode GetCurrentResearchingNode()
    {
        return m_TalenTreeStaticData.GetTalentNode(m_ResearchLine, m_ResearchId);
    }
    public bool IsResearching()
    {
        return m_ResearchLine >= 0 && m_ResearchId >= 0;
    }
    public bool IsResearchAble(MineResearchNode talentNode)
    {
        return !IsResearching() &&
            !m_TalenTreeDynamicData.IsTalentMaxLevel(talentNode) &&
            m_TalenTreeDynamicData.IsResearchAble(talentNode); 
    }
    public bool IsHaveEnoughGemToComplete()
    {
        return ProfileManager.PlayerData.IsEnoughGameResource(ResourceData.ResourceType.GEM, new BigNumber(m_CompleteResearchCost));
    }
    public void StartResearch(MineResearchNode talentNode)
    {
        m_ResearchLine = talentNode.m_NodeLine;
        m_ResearchId = talentNode.m_NodeIndex;
        m_StartTime = DateTime.Now;
    }
    public void ResearchDone()
    {
        MineResearchNode node = m_TalenTreeStaticData.GetTalentNode(m_ResearchLine, m_ResearchId);
        m_ResearchLine = -1;
        m_ResearchId = -1;
        m_TalenTreeDynamicData.OnSuccessUpgradeTalentNode(node);
        m_LastResearchNode = node;
        EventManager.TriggerEvent("UpdateMineNode");
        //Pextension.UIGame.Ins.GetUI<UICMineResearch>().SetupUIResearchInfo(node);
    }
    public void DecreaseResearchTime()
    {
        Debug.Log(m_StartTime);
        TimeSpan ts = TimeSpan.FromSeconds(m_TimeDecreaseAds);
        Debug.Log(ts);
        m_StartTime = m_StartTime.Add(-ts);
        Debug.Log(m_StartTime);
        SaveData();
    }
    public TimeSpan GetTimeResearchLeft()
    {
        MineResearchNode node = GetCurrentResearchingNode();
        int lv = m_TalenTreeDynamicData.GetCurrentTalentNodeLevel(node);
        DateTime endTime = m_StartTime.AddSeconds(node.m_TalentLevelDatas[lv].m_TimeResearch);
        TimeSpan ts = endTime - DateTime.Now;
        return ts;
    }
    private int GetTimeResearch()
    {
        MineResearchNode node = m_TalenTreeStaticData.GetTalentNode(m_ResearchLine, m_ResearchId);
        int lv = m_TalenTreeDynamicData.GetCurrentTalentNodeLevel(node);
        int researchTime = node.m_TalentLevelDatas[lv].m_TimeResearch;
        return researchTime;
    }
    private bool CheckResearchDone()
    {
        if (m_ResearchLine < 0 || m_ResearchId < 0)
            return false;
        int researchTime = GetTimeResearch();
        DateTime now = DateTime.Now;
        TimeSpan ts = now - m_StartTime;
        if (ts.TotalSeconds >= researchTime)
            return true;
        return false;
    }
    public float GetResearchProcess()
    {
        if (m_ResearchLine < 0 || m_ResearchId < 0)
            return 0;
        int researchTime = GetTimeResearch();
        DateTime now = DateTime.Now;
        TimeSpan ts = now - m_StartTime;
        double passedTime = ts.TotalSeconds;
        return (float)(passedTime / (double)researchTime);
    }
    public void CompleteResearchByGem()
    {
        ProfileManager.PlayerData.ConsumeGameResource(ResourceData.ResourceType.GEM, m_CompleteResearchCost);
        ResearchDone();
    }
    public MineResearchNode GetLastResearchNode()
    {
        return m_LastResearchNode;
    }
    public void ResetLastNode()
    {
        m_LastResearchNode = null;
    }
    private void Update()
    {
        if (CheckResearchDone())
            ResearchDone();
    }
}

[System.Serializable]
public class MineResearchSaveData
{
    public string mrd;
    public int rl;
    public int rid;
    public string crt;

    public int ResearchLine { get => rl; set => rl = value; }
    public int ResearchId { get => rid; set => rid = value; }
    public string StartTime { get => crt; set => crt = value; } 
    public string MineResearchData { get => mrd; set => mrd = value; }

    public MineResearchSaveData()
    {
        MineResearchData = "";
        ResearchLine = -1;
        ResearchId = -1;
        StartTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
    }
}

[System.Serializable]
public class MineResearchTalentTreeSaveData
{
    public List<int> tnl;

    public List<int> TalentNodeLines { get => tnl; set => tnl = value; }
    public MineResearchTalentTreeSaveData()
    {

    }
    public MineResearchTalentTreeSaveData(MineResearchDynamicData data)
    {
        TalentNodeLines = data.m_TalentNodeLines;
    }
}
