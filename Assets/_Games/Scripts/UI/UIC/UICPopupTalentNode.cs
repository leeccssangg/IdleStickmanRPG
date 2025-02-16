using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class UICPopupTalentNode : UICanvas
{
    private static string highlightColor = "#fcf803";
    [SerializeField]
    private TextMeshProUGUI m_TalentName;
    [SerializeField]
    private Image m_CurrentIconImage;
    [SerializeField]
    private TextMeshProUGUI m_CurrentTalentLevel;
    [SerializeField]
    private TextMeshProUGUI m_CurrentTalentInfor;
    [SerializeField]
    private Image m_NextIconImage;
    [SerializeField]
    private TextMeshProUGUI m_NextTalentLevel;
    [SerializeField]
    private TextMeshProUGUI m_NextTalentInfor;
    [SerializeField]
    private TextMeshProUGUI m_TalentRequirePoint;
    [SerializeField]
    private TextMeshProUGUI m_TalentRequireNode;
    [SerializeField]
    private Button m_ButtonUpgrade;
    [SerializeField]
    private GameObject m_ButtonLayer;
    [SerializeField]
    private GameObject m_NextTalentLevelLayer;
    [SerializeField]
    private GameObject m_MaxTalentLevelLayer;
    [SerializeField]
    private List<GameObject> m_RequirementLevelLayer;
    [SerializeField] private Button m_BtnClose;
    [SerializeField] private TextMeshProUGUI m_TxtCurrentPoint;


    [SerializeField]
    private Color m_ColorAccept;
    [SerializeField]
    private Color m_ColorRefuse;

    private TalentNode m_TalentNode;
    private TalentTreeDynamicData m_TalentTreeDynamicData;
    private UnityAction m_OnUpgradeSuccessCallback;
    private void Awake()
    {
        m_BtnClose.onClick.AddListener(OnClose);
    }
    private void OnClose()
    {
        UIManager.Ins.CloseUI<UICPopupTalentNode>();
    }
    public void Setup(TalentNode talentNode, TalentTreeDynamicData talentTreeDynamicData, UnityAction onUpgradeSuccessCallback)
    {
        m_TalentNode = talentNode;
        m_TalentTreeDynamicData = talentTreeDynamicData;
        m_OnUpgradeSuccessCallback = onUpgradeSuccessCallback;
        UpdateTalentNodeState();
    }
    public void UpdateTalentNodeState()
    {
        int currentLevel = m_TalentTreeDynamicData.GetCurrentTalentNodeLevel(m_TalentNode);
        m_TalentName.text = m_TalentNode.m_NodeName;
        m_CurrentIconImage.sprite = m_TalentNode.m_IconImage;
        m_CurrentTalentLevel.text = string.Format("Current: {0} Level {1}", m_TalentNode.m_NodeName, currentLevel);
        m_CurrentTalentInfor.text = m_TalentNode.m_TalentLevelDatas[currentLevel].m_Description;
        m_TxtCurrentPoint.text = string.Format("Points : {0}", m_TalentTreeDynamicData.GetCurrentSkillPoint());
        bool isTalentMaxLevel = m_TalentTreeDynamicData.IsTalentMaxLevel(m_TalentNode);
        m_NextTalentLevelLayer.SetActive(!isTalentMaxLevel);
        m_MaxTalentLevelLayer.SetActive(isTalentMaxLevel);
        m_ButtonLayer.SetActive(!isTalentMaxLevel);
        for (int i = 0; i < m_RequirementLevelLayer.Count; i++)
        {
            m_RequirementLevelLayer[i].SetActive(!isTalentMaxLevel);
        }
        if (!isTalentMaxLevel)
        {
            m_NextIconImage.sprite = m_TalentNode.m_IconImage;
            m_NextTalentLevel.text = string.Format("Next: {0} Level {1}", m_TalentNode.m_NodeName, currentLevel + 1);
            m_NextTalentInfor.text = m_TalentNode.m_TalentLevelDatas[currentLevel + 1].m_Description;

            m_TalentRequirePoint.text = string.Format("- Requires <color={1}> {0} </color>  points", m_TalentNode.m_NodePointRequire, highlightColor);
            m_TalentRequirePoint.color = m_TalentTreeDynamicData.HaveEnoughPoint(m_TalentNode) ? m_ColorAccept : m_ColorRefuse;

            m_TalentRequireNode.gameObject.SetActive(m_TalentNode.m_RequireNode.Count > 0);
            if (m_TalentNode.m_RequireNode.Count > 0)
            {
                string res = string.Format("- Requires <color={1}> Max Level </color> in <color={1}> {0} </color>", m_TalentNode.m_RequireNode[0].m_NodeName, highlightColor);
                for (int i = 1; i < m_TalentNode.m_RequireNode.Count; i++)
                {
                    res += string.Format(" and <color={1}> {0} </color>", m_TalentNode.m_RequireNode[i].m_NodeName, highlightColor);
                }
                m_TalentRequireNode.text = res;

                Color tempColor = m_ColorRefuse;
                for (int i = 0; i < m_TalentNode.m_RequireNode.Count; i++)
                {
                    if (m_TalentTreeDynamicData.CanUpgradeTalent(m_TalentNode))
                    {
                        tempColor = m_ColorAccept;
                        break;
                    }
                }
                m_TalentRequireNode.color = tempColor;
            }

            m_ButtonUpgrade.interactable = (m_TalentTreeDynamicData.CanUpgradeTalent(m_TalentNode) && m_TalentTreeDynamicData.HaveEnoughPoint(m_TalentNode));
        }
    }
    public IEnumerator CoUpdateTalentNodeState()
    {
        yield return new WaitForEndOfFrame();
        UpdateTalentNodeState();
    }
    public void OnClickButtonClose()
    {
        Close(0f);

    }
    public void OnClickButtonUpgrade()
    {
        m_TalentTreeDynamicData.OnSuccessUpgradeTalentNode(m_TalentNode);
        UpdateTalentNodeState();
        m_OnUpgradeSuccessCallback?.Invoke();
    }
}
