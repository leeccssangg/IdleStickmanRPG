using Pextension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RookieQuestTab { NONE = -1,D1,D2,D3,D4,D5,D6,D7 };
public class UICRockieQuest : UICanvas
{
    [SerializeField] private UIRockieQuestTab m_RockieQuestTab;
    [SerializeField] private Button m_BtnClose;
    [SerializeField] private ScrollRect m_Scroll;
    [SerializeField] private Image m_ImgLock;
    [SerializeField] private Image m_ImgProcess;

    [SerializeField] private TabGroupButton m_TabButton;
    private RookieQuestTab m_CurrentTab = RookieQuestTab.NONE;

    private void Awake()
    {
        m_BtnClose.onClick.AddListener(OnClickButtonClose);
        SetupButton();
        m_RockieQuestTab.AwakeInGame();
    }
    private void SetupButton()
    {
        m_TabButton.Setup<RookieQuestTab>(OnOpenTab);
    }
    //private void OnEnable()
    //{
    //    OnOpenTabDay(0);
    //    UpdateLevelProcess();
    //}
    private void OnClickButtonClose()
    {
        //UIManager.Instance.OpenUI<UICGamePlay>();
        UIManager.Ins.CloseUI<UICRockieQuest>();
    }
    public void OpenTab(RookieQuestTab tab = RookieQuestTab.NONE)
    {
        if (tab == RookieQuestTab.NONE) tab = m_CurrentTab;
        if (m_CurrentTab == RookieQuestTab.NONE) tab = RookieQuestTab.D1;
        m_TabButton.OnClickButton(tab);
    }
    private void OnOpenTab(RookieQuestTab tab)
    {
        if (m_CurrentTab == tab) return;
        Debug.Log("OnOpenTab: " + tab);
        m_CurrentTab = tab;
        if (tab != RookieQuestTab.NONE)
        OnOpenTabDay((int)tab+1);
    }
    private void OnOpenTabDay(int day)
    {
        Debug.Log(day);
        m_RockieQuestTab.InitData(day);
        m_ImgLock.gameObject.SetActive(day> RockieQuestManager.Ins.GetDayNum());
        m_Scroll.verticalNormalizedPosition = 1;
    }
    private void UpdateLevelProcess()
    {
        m_ImgProcess.fillAmount = RockieQuestManager.Ins.GetPointProcess();
    }

}
