using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "MineResearchDynamicData", menuName = "GlobalConfigs/MineResearchDynamicData")]
[GlobalConfig("Assets/Resources/MineResearchTalentTree")]
[System.Serializable]

public class MineResearchDynamicData : GlobalConfig<MineResearchDynamicData>
{
    private static int m_MaxLevel = 12;
    private static int m_MaxIndex = 3;
    [SerializeField]
    public List<int> m_TalentNodeLines;
    public void OnInitNewData()
    {
        Debug.Log("init");
        //m_CurrentMaxSkillPoint = 1000000;
        m_TalentNodeLines = new List<int>();
        for (int i = 0; i < m_MaxLevel; i++)
        {
            for (int j = 0; j < m_MaxIndex; j++)
            {
                m_TalentNodeLines.Add(0);
            }
        }
        MineResearchManager.Ins.SaveData();
    }
    public void OnLoadData(string jsonString)
    {
        if (string.Compare(jsonString, "") == 0)
        {
            OnInitNewData();
        }
        else
        {
            MineResearchTalentTreeSaveData data = JsonUtility.FromJson<MineResearchTalentTreeSaveData>(jsonString);
            m_TalentNodeLines = data.TalentNodeLines;
        }
    }
    public string OnSaveData()
    {
        //ObscuredPrefs.Set<string>(name, ConvertObjectToJson());
        MineResearchTalentTreeSaveData data = new(this);
        return JsonUtility.ToJson(data);
    }
    public bool IsResearchAble(MineResearchNode talentNode)
    {
        return CanUpgradeTalent(talentNode) && HaveEnoughPoint(talentNode);
    }
    public void UpgradeTalentNode(MineResearchNode talentNode)
    {
        //if (!CanUpgradeTalent(talentNode)) return;
        //if (!HaveEnoughPoint(talentNode)) return;
        //if (!MineResearchManager.Ins.IsResearchAble()) return;
        OnUpgradeTalentNode(talentNode);
    }
    public void OnUpgradeTalentNode(MineResearchNode talentNode)
    {
        MineResearchManager.Ins.StartResearch(talentNode);
        MineResearchManager.Ins.SaveData();
    }
    public void OnSuccessUpgradeTalentNode(int level, int index)
    {
        m_TalentNodeLines[level * m_MaxIndex + index]++;

        MineResearchManager.Ins.SaveData();
    }
    public void OnSuccessUpgradeTalentNode(MineResearchNode talentNode)
    {
        int lv = GetCurrentTalentNodeLevel(talentNode);
        ProfileManager.PlayerData.ConsumeGameResource(ResourceData.ResourceType.MINE_STONE ,talentNode.m_TalentLevelDatas[lv].m_Cost);
        m_TalentNodeLines[talentNode.m_NodeLine * m_MaxIndex + talentNode.m_NodeIndex]++;
        MineResearchManager.Ins.SaveData();
    }
    public void OnSuccessUpgradeMaxSkillPoint(int value)
    {
        //m_CurrentMaxSkillPoint += value;
        MineResearchManager.Ins.SaveData();
    }
    public BigNumber GetCurrentSkillPoint()
    {
        return ProfileManager.PlayerData.GetResoureValue(ResourceData.ResourceType.MINE_STONE);
    }
    public int GetCurrentTalentNodeLevel(int level, int index)
    {
        return m_TalentNodeLines[level * m_MaxIndex + index];
    }
    public int GetCurrentTalentNodeLevel(MineResearchNode talentNode)
    {
        Debug.Log(talentNode.m_NodeLine);
        Debug.Log(m_MaxIndex);
        Debug.Log(talentNode.m_NodeIndex);
        return m_TalentNodeLines[talentNode.m_NodeLine * m_MaxIndex + talentNode.m_NodeIndex];
    }
    public bool IsTalentMaxLevel(MineResearchNode talentNode)
    {
        return !(GetCurrentTalentNodeLevel(talentNode) < talentNode.GetTalentMaxLevelUpgrade());
    }
    public bool HaveEnoughPoint(MineResearchNode talentNode)
    {
        int lv = GetCurrentTalentNodeLevel(talentNode);
        return ProfileManager.PlayerData.IsEnoughGameResource(ResourceData.ResourceType.MINE_STONE, new BigNumber(talentNode.m_TalentLevelDatas[lv].m_Cost));
        //return m_CurrentSkillPoint >= talentNode.m_TalentLevelDatas[lv].m_Cost;
    }
    public bool CanUpgradeTalent(MineResearchNode talentNode)
    {
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
