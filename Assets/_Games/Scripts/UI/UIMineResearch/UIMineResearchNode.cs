using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIMineResearchNode : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private MineResearchDynamicData m_TalentTreeDynamicData;
    [SerializeField]
    private MineResearchNode m_TalentNode;
    public MineResearchNode MineResearchNode { get => m_TalentNode; }
    [SerializeField]
    private CanvasGroup m_CanvasGroup;
    [SerializeField]
    private Image m_IconImage;
    [SerializeField]
    private TextMeshProUGUI m_TalentLevelText;
    [SerializeField]
    private GameObject m_ImageLock;
    [SerializeField]
    private GameObject m_ImageUnlock;
    [SerializeField]
    private GameObject m_ImageSelect;
    [SerializeField]
    private GameObject m_ImageMaxLv;
    [SerializeField]
    private List<UIEdge> m_UIEdges;
    [SerializeField]
    private bool m_IsUnlockNode;
    [SerializeField]
    private Button m_Btn;

    private UnityAction<MineResearchNode> m_OnButtonClickCallBack;
    private UnityAction<UIMineResearchNode> m_OnSelectedCallBack;

    private void Awake()
    {
        m_Btn.onClick.AddListener(OnClickUINode);
    }

    public void Setup(UnityAction<UIMineResearchNode> onSelectedUICallBack)
    {
        m_OnSelectedCallBack = onSelectedUICallBack;
    }
    public void UpdateNodeState()
    {
        if (m_TalentNode == null) return;
        m_IsUnlockNode = false;
        m_TalentLevelText.text = string.Format("{0}/{1}", m_TalentTreeDynamicData.GetCurrentTalentNodeLevel(m_TalentNode), m_TalentNode.GetTalentMaxLevelUpgrade());
        //m_IsUnlockNode = m_TalentNode.m_RequireNode.Count == 0;
        if (m_TalentTreeDynamicData.IsTalentMaxLevel(m_TalentNode) || m_TalentTreeDynamicData.CanUpgradeTalent(m_TalentNode))
            m_IsUnlockNode = true;
        //Debug.Log(m_TalentTreeDynamicData.IsTalentMaxLevel(m_TalentNode) || m_TalentTreeDynamicData.CanUpgradeTalent(m_TalentNode));
        //Debug.Log(m_IsUnlockNode);
        //m_IsUnlockNode = m_TalentTreeDynamicData.CanUpgradeTalent(m_TalentNode);
        if(m_UIEdges.Count > 0)
        {
            for (int i = 0; i < m_TalentNode.m_RequireNode.Count; i++)
            {
                bool isFillEdge = m_TalentTreeDynamicData.GetCurrentTalentNodeLevel(m_TalentNode.m_RequireNode[i]) > 0;
                //bool isFillEdge = m_TalentTreeDynamicData.CanUpgradeTalent(m_TalentNode.m_RequireNode[i]);
                m_UIEdges[i].SetFill(isFillEdge);
                //if (isFillEdge)
                //{
                //    m_IsUnlockNode = true;
                //}
            }
        }
        m_ImageLock.SetActive(!m_IsUnlockNode);
        m_ImageUnlock.SetActive(m_IsUnlockNode);
        m_ImageMaxLv.SetActive(m_TalentTreeDynamicData.IsTalentMaxLevel(m_TalentNode));
    }
    public void OnClickUINode()
    {
        Debug.Log("Click");
        if(m_OnSelectedCallBack != null)
            m_OnSelectedCallBack.Invoke(this);
    }
    public MineResearchNode GetNode()
    {
        return m_TalentNode;
    }
    public void SetImgSelected(bool isSelected)
    {
        if (m_TalentNode != null)
            m_ImageSelect.gameObject.SetActive(isSelected);
        else
            m_ImageSelect.gameObject.SetActive(false);
    }

    #region Editor
#if UNITY_EDITOR
    public void PlayerStatsTalentNodeEditorSetup(MineResearchNode talentNode, UICMineResearch uiTalentTree)
    {
        m_TalentTreeDynamicData = MineResearchDynamicData.Instance;
        m_TalentNode = talentNode;
        EditorUtility.SetDirty(m_CanvasGroup);
        m_CanvasGroup.alpha = m_TalentNode == null ? 0 : 1;
        m_CanvasGroup.interactable = m_TalentNode != null;
        m_CanvasGroup.blocksRaycasts = m_TalentNode != null;

        if (m_TalentNode == null) return;
        EditorUtility.SetDirty(m_IconImage);
        EditorUtility.SetDirty(m_TalentNode);
        m_IconImage.sprite = m_TalentNode.m_IconImage;
        m_TalentLevelText.text = string.Format("0/{0}", talentNode.GetTalentMaxLevelUpgrade());
        m_UIEdges.Clear();
        if (uiTalentTree == null) return;
        for (int i = 0; i < m_TalentNode.m_RequireNode.Count; i++)
        {
            m_UIEdges.Add(uiTalentTree.GeneratePlayerStatsUIEdge(transform, uiTalentTree.GetUIPlayerStatsTalentNode(m_TalentNode.m_RequireNode[i]).transform));
        }
    }
#endif
    #endregion
}
