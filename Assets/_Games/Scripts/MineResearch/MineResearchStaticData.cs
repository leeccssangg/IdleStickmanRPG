using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineResearchStaticData : ScriptableObject
{
    public List<MineResearchNode> m_TalentNode;

    public void Initialize()
    {
        m_TalentNode = new List<MineResearchNode>();
    }
    public MineResearchNode GetFirstNode()
    {
        return m_TalentNode[0];
    }
    public MineResearchNode GetTalentNode(int line, int index)
    {
        for (int i = 0; i < m_TalentNode.Count; i++)
        {
            if (m_TalentNode[i].m_NodeLine == line && m_TalentNode[i].m_NodeIndex == index)
            {
                return m_TalentNode[i];
            }
        }
        return null;
    }
    public string GetTimeResearch(int line, int index, int level)
    {
        return GetTalentNode(line, index).m_TalentLevelDatas[level].m_TimeResearch.ToString();
    }
}
