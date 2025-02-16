using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMineResearchInfo : MonoBehaviour
{
    [SerializeField] private UIMineResearchNode m_UINode;
    [SerializeField] private MineResearchNode m_Node;
    [SerializeField] private Button m_BtnResearch;
    [SerializeField] private Button m_BtnMax;
    [SerializeField] private Button m_BtnLock;
    [SerializeField] private TextMeshProUGUI m_TxtTimeResearch;
    [SerializeField] private TextMeshProUGUI m_TxtCurrentLevelDes;
    [SerializeField] private TextMeshProUGUI m_TxtNextLevelDes;
    [SerializeField] private TextMeshProUGUI m_TxtCost;
    [SerializeField] private GameObject m_PanelNextLevel;

    [SerializeField] private UiMineResearchingInfo m_UIResearchingInfo;

    private void Awake()
    {
        //EventManager.StartListening("UpdateMineNode", ()=> Setup(m_Node));
        m_BtnResearch.onClick.AddListener(OnClickButtonResearch);
    }

    public void Setup(MineResearchNode talentNode)
    {
        m_Node = talentNode;
       // m_UINode.PlayerStatsTalentNodeEditorSetup(m_Node, null);
        m_UINode.UpdateNodeState();
        if (!MineResearchDynamicData.Instance.IsTalentMaxLevel(m_Node))
            UpdateUIResearchAble();
        else
            UpdateUIMaxLevel();
        UpdateUIResearchingInfo();
    }
    private void UpdateUIResearchingInfo()
    {
        m_UIResearchingInfo.gameObject.SetActive(MineResearchManager.Ins.IsResearching());
        m_UIResearchingInfo.Setup();
    }
    private void UpdateUIResearchAble()
    {
        m_PanelNextLevel.gameObject.SetActive(true);
        m_BtnMax.gameObject.SetActive(false);
        if (!MineResearchDynamicData.Instance.CanUpgradeTalent(m_Node))
        {
            m_BtnLock.gameObject.SetActive(true);
            m_BtnResearch.gameObject.SetActive(false);
        }
        else
        {
            m_BtnLock.gameObject.SetActive(false);
            m_BtnResearch.gameObject.SetActive(true);
            m_BtnResearch.interactable = MineResearchManager.Ins.IsResearchAble(m_Node);
        }
        int level = MineResearchDynamicData.Instance.GetCurrentTalentNodeLevel(m_Node);
        Debug.Log(level);
        m_TxtTimeResearch.gameObject.SetActive(true);
        m_TxtTimeResearch.text = m_Node.m_TalentLevelDatas[level].m_TimeResearch.ToString()+"s";
        m_TxtCost.text = m_Node.m_TalentLevelDatas[level].m_Cost.ToString();
        if (level == 0)
            m_TxtCurrentLevelDes.text = string.Format(m_Node.m_TalentLevelDatas[level].m_Description, 0);
        else
            m_TxtCurrentLevelDes.text = string.Format(m_Node.m_TalentLevelDatas[level-1].m_Description, m_Node.m_TalentLevelDatas[level-1].m_Value);
        m_TxtNextLevelDes.text = string.Format(m_Node.m_TalentLevelDatas[level].m_Description, m_Node.m_TalentLevelDatas[level].m_Value);
    }
    private void UpdateUIMaxLevel()
    {
        m_PanelNextLevel.gameObject.SetActive(false);
        m_BtnMax.gameObject.SetActive(true);
        m_BtnResearch.gameObject.SetActive(false);
        m_BtnLock.gameObject.SetActive(false);
        m_TxtTimeResearch.gameObject.SetActive(false);
        int level = MineResearchDynamicData.Instance.GetCurrentTalentNodeLevel(m_Node);
        m_TxtCurrentLevelDes.text = string.Format(m_Node.m_TalentLevelDatas[level-1].m_Description, m_Node.m_TalentLevelDatas[level - 1].m_Value);
    }
    private void OnClickButtonResearch()
    {
        MineResearchDynamicData.Instance.UpgradeTalentNode(m_Node);
        Setup(m_Node);
    }
}
