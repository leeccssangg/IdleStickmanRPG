using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Base talent tree
/// </summary>

public class TalentTreeStaticData : ScriptableObject
{
    public List<TalentNode> m_TalentNode;

    public void Initialize()
    {
        m_TalentNode = new List<TalentNode>();
    }
    public TalentNode GetTalentNode(int level, int index)
    {
        for (int i = 0; i < m_TalentNode.Count; i++)
        {
            if (m_TalentNode[i].m_NodeLine == level && m_TalentNode[i].m_NodeIndex == index)
            {
                return m_TalentNode[i];
            }
        }
        return null;
    }
}
