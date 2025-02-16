using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "TalentTreeDynamicData", menuName = "GlobalConfigs/TalentTreeDynamicData")]
[GlobalConfig("Assets/Resources/TalentTreeGlobalConfig/PLAYER_STATS")]
[System.Serializable]

public class TalentTreeDynamicData : GlobalConfig<TalentTreeDynamicData>
{
    private static int m_MaxLevel = 10;
    private static int m_MaxIndex = 3;
    public int m_CurrentSkillPoint ;
    public int m_CurrentMaxSkillPoint = 10000000;
    public int m_CurrentUsedSkillPoint;
    [SerializeField]
    public List<int> m_TalentNodeLines;
    public void OnInitNewData()
    {
        Debug.Log("init");
        //m_CurrentMaxSkillPoint = 1000000;
        m_CurrentUsedSkillPoint = 0;
        m_CurrentSkillPoint = 1000;
        m_TalentNodeLines = new List<int>();
        for (int i = 0; i < m_MaxLevel; i++)
        {
            for (int j = 0; j < m_MaxIndex; j++)
            {
                m_TalentNodeLines.Add(0);
            }
        }
        TalentTreeManager.Ins.SaveData();
    }
    public void OnLoadData(string jsonString)
    {
        if (string.Compare(jsonString, "") == 0)
        {
            OnInitNewData();
        }
        else
        {
            PlayerStatsTalentTreeSaveData data = JsonUtility.FromJson<PlayerStatsTalentTreeSaveData>(jsonString);
            m_CurrentSkillPoint = data.CurrentSkillPoint;
            m_CurrentUsedSkillPoint = data.UsedSkillPoint;
            m_TalentNodeLines = data.TalentNodeLines;
        }
        //string json = ObscuredPrefs.Get<string>(name, "");
        //if (string.Compare(json, "") == 0)
        //{
        //    OnInitNewData();
        //}
        //else
        //{
        //    ConvertJsonToObject(json);
        //}
    }
    public string OnSaveData()
    {
        //ObscuredPrefs.Set<string>(name, ConvertObjectToJson());
        PlayerStatsTalentTreeSaveData data = new(this);
        return JsonUtility.ToJson(data);
    }
    public void UpgradeTalentNode(TalentNode talentNode)
    {
        if (!CanUpgradeTalent(talentNode)) return;
        if (!HaveEnoughPoint(talentNode)) return;
        OnSuccessUpgradeTalentNode(talentNode);
    }
    public void OnSuccessUpgradeTalentNode(int level, int index)
    {
        if (m_CurrentUsedSkillPoint >= m_CurrentMaxSkillPoint) return;
        m_CurrentUsedSkillPoint++;
        m_TalentNodeLines[level * m_MaxIndex + index]++;

        TalentTreeManager.Ins.SaveData();
    }
    public void OnSuccessUpgradeTalentNode(TalentNode talentNode)
    {
        m_CurrentSkillPoint -= talentNode.m_NodePointRequire;
        m_CurrentUsedSkillPoint += talentNode.m_NodePointRequire;
        m_TalentNodeLines[talentNode.m_NodeLine * m_MaxIndex + talentNode.m_NodeIndex]++;
        TalentTreeManager.Ins.SaveData();
    }
    public void OnSuccessUpgradeMaxSkillPoint(int value)
    {
        m_CurrentMaxSkillPoint += value;
        TalentTreeManager.Ins.SaveData();
    }
    public void OnSuccessResetUsedSkillPoint()
    {
        m_CurrentSkillPoint += m_CurrentUsedSkillPoint;
        m_CurrentUsedSkillPoint = 0;
        m_TalentNodeLines.Clear();
        for (int i = 0; i < m_MaxLevel; i++)
        {
            for (int j = 0; j < m_MaxIndex; j++)
            {
                m_TalentNodeLines.Add(0);
            }
        }
        TalentTreeManager.Ins.SaveData();
    }
    public int GetCurrentSkillPoint()
    {
        return m_CurrentSkillPoint;
    }
    public int GetCurrentTalentNodeLevel(int level, int index)
    {
        return m_TalentNodeLines[level * m_MaxIndex + index];
    }
    public int GetCurrentTalentNodeLevel(TalentNode talentNode)
    {
        return m_TalentNodeLines[talentNode.m_NodeLine * m_MaxIndex + talentNode.m_NodeIndex];
    }
    public bool IsResetable()
    {
        return m_CurrentUsedSkillPoint > 0;
    }
    public bool IsTalentMaxLevel(TalentNode talentNode)
    {
        return !(GetCurrentTalentNodeLevel(talentNode) < talentNode.GetTalentMaxLevelUpgrade());
    }
    public bool HaveEnoughPoint(TalentNode talentNode)
    {
        return m_CurrentSkillPoint >= talentNode.m_NodePointRequire;
    }
    public bool CanUpgradeTalent(TalentNode talentNode)
    {
        if (m_CurrentUsedSkillPoint >= m_CurrentMaxSkillPoint) 
            return false;
        //if (m_CurrentSkillPoint < talentNode.m_NodePointRequire)
        //    return false;
        for (int i = 0; i < talentNode.m_RequireNode.Count; i++)
        {
            if (!IsTalentMaxLevel(talentNode.m_RequireNode[i]))
            {
                return false;
            }
        }
        return true;
    }
    private string ConvertObjectToJson()
    {
        return JsonUtility.ToJson(this);
    }
    private void ConvertJsonToObject(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
}
