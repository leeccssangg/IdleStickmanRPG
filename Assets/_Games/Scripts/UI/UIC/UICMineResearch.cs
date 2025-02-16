using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UICMineResearch : UICanvas
{
    [SerializeField]
    private MineResearchStaticData m_PlayerStatsTalentTreeStaticData;
    [SerializeField]
    private MineResearchDynamicData m_PlayerStatsTalentTreeDynamicData;
    [SerializeField]
    private TextMeshProUGUI m_PlayerStatsTalentPoint;
    public List<UIMineResearchNode> m_UIPlayerStatsTalentNode;
    //[SerializeField] private Button m_BtnClose;
    [SerializeField] private UIMineResearchInfo m_UIMineResearchInfo;
    [SerializeField] private UIMineResearchComplete m_UIMineResearchComplete;
    private void Awake()
    {
        //m_BtnClose.onClick.AddListener(OnClickButtonClose);
        EventManager.StartListening("UpdateMineStone", UpdateMineStone);
        EventManager.StartListening("UpdateMineNode", UpdatePlayerStatsTalentTreeState);
        EventManager.StartListening("UpdateMineNode", UpdateUIResearchComplete);
    }

    public override void Setup()
    {
        base.Setup();
        m_PlayerStatsTalentTreeDynamicData = MineResearchDynamicData.Instance;
        for (int i = 0; i < m_UIPlayerStatsTalentNode.Count; i++)
        {
            m_UIPlayerStatsTalentNode[i].Setup(SetupUIResearchInfo);
        }
        UpdatePlayerStatsTalentTreeState();
        UpdateMineStone();
        UpdateUIResearchInfo();
        UpdateUIResearchComplete();
    }
    private void UpdateUIResearchInfo()
    {
        if (!MineResearchManager.Ins.IsResearching())
            SetupUIResearchInfo(GetCorrespondingUINode(m_PlayerStatsTalentTreeStaticData.GetFirstNode()));
        else
            SetupUIResearchInfo(GetCorrespondingUINode(MineResearchManager.Ins.GetCurrentResearchingNode()));
    }
    private void UpdateUIResearchComplete()
    {
        m_UIMineResearchComplete.gameObject.SetActive(MineResearchManager.Ins.GetLastResearchNode() != null);
        m_UIMineResearchComplete.Setup(MineResearchManager.Ins.GetLastResearchNode());
    }
    public void SetupUIResearchInfo(UIMineResearchNode uiNode)
    {
        OnSelectUIMineResearch(uiNode);
        MineResearchNode node = uiNode.GetNode();
        m_UIMineResearchInfo.Setup(node);
    }
    public void SetupUIResearchInfo(MineResearchNode node)
    {
        UIMineResearchNode ui = GetCorrespondingUINode(node);
        OnSelectUIMineResearch(ui);
        m_UIMineResearchInfo.Setup(node);
    }
    public void OnSelectUIMineResearch(UIMineResearchNode uiNode)
    {
        for(int i = 0; i < m_UIPlayerStatsTalentNode.Count; i++)
        {
            m_UIPlayerStatsTalentNode[i].SetImgSelected(m_UIPlayerStatsTalentNode[i] == uiNode);
        }
    }
    private UIMineResearchNode GetCorrespondingUINode(MineResearchNode node)
    {
        UIMineResearchNode ui = null;
        for(int i = 0; i < m_UIPlayerStatsTalentNode.Count; i++)
        {
            if (m_UIPlayerStatsTalentNode[i].GetNode() == node)
            {
                ui = m_UIPlayerStatsTalentNode[i];
                break;
            }
        }
        return ui;
    }
    #region CombatTalentTree Functions
    public void UpdatePlayerStatsTalentTreeState()
    {
        for (int i = 0; i < m_UIPlayerStatsTalentNode.Count; i++)
        {
            m_UIPlayerStatsTalentNode[i].UpdateNodeState();
        }
        //m_BtnReset.interactable = m_PlayerStatsTalentTreeDynamicData.IsResetable();
    }
    private void UpdateMineStone()
    {
        m_PlayerStatsTalentPoint.text = string.Format("Mine Stone: {0}", m_PlayerStatsTalentTreeDynamicData.GetCurrentSkillPoint());
    }
    #endregion

    public void OnClickButtonClose()
    {
        UIManager.Ins.OpenUI<UICGamePlay>();
        UIManager.Ins.CloseUI<UICMineResearch>();
    }

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
        m_PlayerStatsTalentTreeContent.GetComponentsInChildren<UIMineResearchNode>(true, m_UIPlayerStatsTalentNode);
        for (int i = 0; i < m_UIPlayerStatsTalentNode.Count; i++)
        {
            Debug.Log(i);
            int line = i / 3;
            int index = i % 3;
            m_UIPlayerStatsTalentNode[i].PlayerStatsTalentNodeEditorSetup(m_PlayerStatsTalentTreeStaticData.GetTalentNode(line, index), this);
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
    public UIMineResearchNode GetUIPlayerStatsTalentNode(MineResearchNode talentNode)
    {
        for (int i = 0; i < m_UIPlayerStatsTalentNode.Count; i++)
        {
            if (m_UIPlayerStatsTalentNode[i].MineResearchNode == talentNode)
            {
                return m_UIPlayerStatsTalentNode[i];
            }
        }
        return null;
    }
#endif
    #endregion
}
