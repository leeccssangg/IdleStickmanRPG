using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
/// <summary>
/// A base node form talent tree
/// </summary>

public class TalentNode : ScriptableObject
{
    [OnValueChanged("OnValueChange")]
    public string m_NodeName;

    [OnValueChanged("OnValueChange")]
    public int m_NodeLine;

    [OnValueChanged("OnValueChange")]
    public int m_NodeIndex;

    [OnValueChanged("OnValueChange")]
    public int m_NodePointRequire;

    [OnValueChanged("OnValueChange")]
    public Sprite m_IconImage;

    [OnValueChanged("OnValueChange")]
    public List<TalentLevelData> m_TalentLevelDatas;

    [OnValueChanged("OnValueChange")]
    public List<TalentNode> m_RequireNode;

    #region Get Set Functions
    public int GetTalentMaxLevelUpgrade()
    {
        return m_TalentLevelDatas.Count - 1;
    }
    public virtual System.Enum GetNodeType()
    {
        return null;
    }
    public virtual void SetNodeType(System.Enum value)
    {

    }
    #endregion


    #region Editor Functions
#if UNITY_EDITOR
    [HideInInspector]
    public Vector2 m_Position;
    public UnityAction m_OnValueChange;
#endif

    public void OnValueChange()
    {
#if UNITY_EDITOR
        m_OnValueChange?.Invoke();
#endif
    }
    #endregion

}

[System.Serializable]
public class TalentLevelData
{
    public int m_Value;
    [TextArea]
    public string m_Description;

}
