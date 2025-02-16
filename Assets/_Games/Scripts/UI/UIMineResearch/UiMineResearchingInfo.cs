using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiMineResearchingInfo : MonoBehaviour
{
    [SerializeField] private MineResearchNode m_Node;
    [SerializeField] private TextMeshProUGUI m_TxtResearchName;
    [SerializeField] private TextMeshProUGUI m_TxtTimeResearch;
    [SerializeField] private Button m_BtnSkipTime;
    [SerializeField] private Button m_BtnComplete;

    private void Awake()
    {
        m_BtnComplete.onClick.AddListener(OnButtonResearchDoneClick);
        m_BtnSkipTime.onClick.AddListener(OnButtonSkipTimeClick);
    }
    public void Setup()
    {
        if (!MineResearchManager.Ins.IsResearching())
            return;
        m_Node = MineResearchManager.Ins.GetCurrentResearchingNode();
        m_TxtResearchName.text = "Researching " + m_Node.m_NodeName.ToString();
        m_BtnComplete.interactable = MineResearchManager.Ins.IsHaveEnoughGemToComplete();
    }
    private void OnButtonSkipTimeClick()
    {
        Debug.Log("Skiptime");
        MineResearchManager.Ins.DecreaseResearchTime();
    }
    private void OnButtonResearchDoneClick()
    {
        MineResearchManager.Ins.CompleteResearchByGem();
        this.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (isActiveAndEnabled && m_Node != null && MineResearchManager.Ins.IsResearching())
            m_TxtTimeResearch.text = Static.GetTimeStringFromSecond(MineResearchManager.Ins.GetTimeResearchLeft().TotalSeconds); 
    }
}
