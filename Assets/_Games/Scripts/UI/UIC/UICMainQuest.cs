using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Pextension;
using Unity.VisualScripting;

public enum MainQuestTab { NONE = -1, DAILYQUEST, ACHIEVEMENT, }
public class UICMainQuest : UICanvas
{
    [SerializeField] private UIPanelDailyQuest m_UIPanelDailyQuest;
    [SerializeField] private UIPanelAchievement m_UIPanelAchievement;
    
    //[SerializeField] private CanvasGroup m_ContentCanvasGroup;
    //[SerializeField] private RectTransform m_ContentRectTransform;
    [SerializeField] private Image m_ImgBg;

    [SerializeField] private Button m_BtnClose;

    [SerializeField] private TabGroupButton m_TabButton;
    private MainQuestTab m_CurrentTab = MainQuestTab.NONE;
    private void Awake()
    {
        m_UIPanelDailyQuest.AwakeInGame();
        m_UIPanelAchievement.AwakeInGame();
        m_BtnClose.onClick.AddListener(OnClose);
        SetupButton();
    }
    private void SetupButton()
    {
        m_TabButton.Setup<MainQuestTab>(OnOpenTab);
    }
    public void OpenTab(MainQuestTab tab = MainQuestTab.NONE)
    {
        if (tab == MainQuestTab.NONE) tab = m_CurrentTab;
        if (m_CurrentTab == MainQuestTab.NONE) tab = MainQuestTab.DAILYQUEST;
        m_TabButton.OnClickButton(tab);
    }
    private void OnOpenTab(MainQuestTab tab)
    {
        if (m_CurrentTab == tab) return;
        Debug.Log("OnOpenTab: " + tab);
        CloseAllPanel();
        m_CurrentTab = tab;
        switch (tab)
        {
            case MainQuestTab.DAILYQUEST:
                OpenPanelDailyQuest();
                break;
            case MainQuestTab.ACHIEVEMENT:
                OpenPanelAchivement();
                break;
            case MainQuestTab.NONE:
            default:
                break;
        }
    }
    private void OnEnable()
    {
        //m_ContentRectTransform.DOScale(0, 0);
        m_ImgBg.DOFade(1f, 0.25f);
        //m_ContentCanvasGroup.DOFade(1, 0.15f).OnComplete(() => {
        //    m_ContentRectTransform.DOScale(1.05f, 0.15f).OnComplete(() => {
        //        m_ContentRectTransform.DOScale(1, 0.15f).OnComplete(() => {
        //        });
        //    });
        //});
    }
    private void OnClose()
    {
        //UIManager.Instance.OpenUI<UICGamePlay>();
        m_ImgBg.DOFade(0f, 0.25f);
        //m_ContentRectTransform.DOScale(0, 0.25f).OnComplete(() =>
        //{
        //    m_ContentCanvasGroup.DOFade(0, 0.25f);
        UIManager.Ins.CloseUI<UICMainQuest>();
        //});
    } 
    private void OpenPanelDailyQuest()
    {
        m_UIPanelDailyQuest.gameObject.SetActive(true);
        m_UIPanelDailyQuest.Setup();
    }
    private void OpenPanelAchivement()
    {
        m_UIPanelAchievement.gameObject.SetActive(true);
    }
    private void CloseAllPanel()
    {
        m_UIPanelDailyQuest.gameObject.SetActive(false);
        m_UIPanelAchievement.gameObject.SetActive(false);
    }
}
