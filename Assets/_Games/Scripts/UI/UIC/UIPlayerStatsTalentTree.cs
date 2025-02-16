using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIPlayerStatsTalentTree : UICanvas
{
    //[Header("Player Stats Talent Tree")]
    //[SerializeField]
    //public GameObject m_CombatTalentTreeTab;
    [SerializeField]
    private TalentTreeStaticData m_PlayerStatsTalentTreeStaticData;
    [SerializeField]
    private TalentTreeDynamicData m_PlayerStatsTalentTreeDynamicData;
    [SerializeField]
    private TextMeshProUGUI m_PlayerStatsTalentPoint;
    public List<UITalentNode> m_UIPlayerStatsTalentNode;
    //[SerializeField] private Button m_BtnClose;
    [SerializeField] private Button m_BtnReset;
    private void Awake()
    {
        //m_BtnClose.onClick.AddListener(OnClickButtonClose);
        m_BtnReset.onClick.AddListener(OnClickButtonResetPlayerStatsTalentTree);
    }

    public override void Setup()
    {
        base.Setup();
        m_PlayerStatsTalentTreeDynamicData = TalentTreeDynamicData.Instance;
        for (int i = 0; i < m_UIPlayerStatsTalentNode.Count; i++)
        {
            m_UIPlayerStatsTalentNode[i].Setup(UpdatePlayerStatsTalentTreeState);
        }
        UpdatePlayerStatsTalentTreeState();
    }
    #region CombatTalentTree Functions
    public void UpdatePlayerStatsTalentTreeState()
    {
        m_PlayerStatsTalentPoint.text = string.Format("Talent Point: {0}", m_PlayerStatsTalentTreeDynamicData.GetCurrentSkillPoint());
        for (int i = 0; i < m_UIPlayerStatsTalentNode.Count; i++)
        {
            m_UIPlayerStatsTalentNode[i].UpdateNodeState();
        }
        m_BtnReset.interactable = m_PlayerStatsTalentTreeDynamicData.IsResetable();
    }
    public void OnClickButtonResetPlayerStatsTalentTree()
    {
        m_PlayerStatsTalentTreeDynamicData.OnSuccessResetUsedSkillPoint();
        UpdatePlayerStatsTalentTreeState();
    }
    #endregion

    //public void OnClickButtonClose()
    //{
    //    UIManager.Instance.OpenUI<UICGamePlay>();
    //    UIManager.Instance.CloseUI<UIPlayerStatsTalentTree>();
    //}

    #region Editor
#if UNITY_EDITOR
    [Header("Editor")]
    public UIEdge m_UIEdge;
    public Transform m_PlayerStatsTalentTreeContent;
    public Transform m_PlayerStatsTalentTreeEdgeContent;
    [Button]
    public void GeneratelayerStatsTalentTree()
    {
        while (m_PlayerStatsTalentTreeEdgeContent.childCount != 0)
        {
            DestroyImmediate(m_PlayerStatsTalentTreeEdgeContent.GetChild(0).gameObject);
        }
        m_PlayerStatsTalentTreeContent.GetComponentsInChildren<UITalentNode>(true, m_UIPlayerStatsTalentNode);
        for (int i = 0; i < m_UIPlayerStatsTalentNode.Count; i++)
        {
            Debug.Log(i);
            int line = i / 3;
            int index = i % 3;
            m_UIPlayerStatsTalentNode[i].PlayerStatsTalentNodeEditorSetup(m_PlayerStatsTalentTreeDynamicData, m_PlayerStatsTalentTreeStaticData.GetTalentNode(line, index), this);
        }
    }
    public UIEdge GeneratePlayerStatsUIEdge(Transform startTarget, Transform endTarget)
    {
        UIEdge go = (UIEdge)PrefabUtility.InstantiatePrefab(m_UIEdge);
        go.transform.SetParent(m_PlayerStatsTalentTreeEdgeContent);
        UIEdge uIEdge = go.GetComponent<UIEdge>();
        uIEdge.Setup(startTarget, endTarget);
        return uIEdge;
    }
    public UITalentNode GetUIPlayerStatsTalentNode(TalentNode talentNode)
    {
        for (int i = 0; i < m_UIPlayerStatsTalentNode.Count; i++)
        {
            if (m_UIPlayerStatsTalentNode[i].TalentNode == talentNode)
            {
                return m_UIPlayerStatsTalentNode[i];
            }
        }
        return null;
    }
#endif
    #endregion
}
