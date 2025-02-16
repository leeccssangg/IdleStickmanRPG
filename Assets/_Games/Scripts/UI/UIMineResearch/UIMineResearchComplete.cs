using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIMineResearchComplete : MonoBehaviour
{
    [SerializeField] private UIMineResearchNode m_UINode;
    [SerializeField] private MineResearchNode m_Node;
    [SerializeField] private TextMeshProUGUI m_TxtResearchName;
    [SerializeField] private TextMeshProUGUI m_TxtCurrentLevelDes;
    [SerializeField] private Button m_BtnClose;

    private void Awake()
    {
        m_BtnClose.onClick.AddListener(OnClickButtonClose);
    }
    public void Setup(MineResearchNode node)
    {
        if (node == null)
            return;
        m_Node = node;
        //m_UINode.PlayerStatsTalentNodeEditorSetup(m_Node, null);
        m_UINode.UpdateNodeState();
        m_TxtResearchName.text = m_Node.m_NodeName.ToString();
        int level = MineResearchDynamicData.Instance.GetCurrentTalentNodeLevel(m_Node);
        m_TxtCurrentLevelDes.text = string.Format(m_Node.m_TalentLevelDatas[level - 1].m_Description, m_Node.m_TalentLevelDatas[level - 1].m_Value);
    }
    private void OnClickButtonClose()
    {
        MineResearchManager.Ins.ResetLastNode();
        this.gameObject.SetActive(false);
    }
}
