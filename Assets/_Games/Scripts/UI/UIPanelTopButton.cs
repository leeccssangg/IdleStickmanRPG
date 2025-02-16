using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelTopButton : UICanvas
{
    public Button m_BtnDailyGift;
    public Button m_BtnSpinWheel;
    public Button m_BtnDailyQuest;
    public Button m_BtnRockieQuest;

    private void Awake()
    {
        m_BtnDailyGift.onClick.AddListener(OpenUICDailyGift);
        m_BtnSpinWheel.onClick.AddListener(OpenUICSpinWheel);
        m_BtnDailyQuest.onClick.AddListener(OpenUICDailyQuest);
        m_BtnRockieQuest.onClick.AddListener(OpenUICRockieQuest);
    }
    private void OpenUICDailyGift()
    {
        UIManager.Ins.OpenUI<UICDailyGift>();
    }
    private void OpenUICSpinWheel()
    {
        UIManager.Ins.OpenUI<UICSpinWheel>();
    }
    private void OpenUICDailyQuest()
    {
        UIManager.Ins.OpenUI<UICMainQuest>().OpenTab();
    }
    private void OpenUICRockieQuest()
    {
        UIManager.Ins.OpenUI<UICRockieQuest>().OpenTab();
    }
}
