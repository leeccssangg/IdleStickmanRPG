using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPanelHero:UIMainTab {
    public List<ButtonTab> m_ButtonTab;
    public List<UIHeroTab> m_UIpanelTab;

    private int m_CurrentTab;

    private void Awake() {
        EventManager.StartListening("OpenSkillTab",()=> SetlectTab(2));
    }
    private void Start() {
    }
    public override void Setup() {
        base.Setup();
        SetupButtonTab();
    }
    public void SetupButtonTab() {
        for(int i = 0;i < m_ButtonTab.Count;i++) {
            ButtonTab tab = m_ButtonTab[i];
            tab.Setup(i,SetlectTab);
        }
        SetlectTab(m_CurrentTab);
    }


    public void SetlectTab(int tabID) {
        m_CurrentTab = tabID;
        UpdateTabState();
    }
    public void UpdateTabState() {
        UpdateTabButton();
        UpdateTabPanel();
    }
    public void UpdateTabPanel() {
        for(int i = 0;i < m_UIpanelTab.Count;i++) {
            UIHeroTab panel = m_UIpanelTab[i];
            if(i == m_CurrentTab) {
                panel.gameObject.SetActive(true);
                panel.Setup();
            } else
                panel.gameObject.SetActive(false);
        }
    }
    public void UpdateTabButton() {
        for(int i = 0;i < m_ButtonTab.Count;i++) {
            ButtonTab tab = m_ButtonTab[i];
            if(i == m_CurrentTab) {
                tab.Active(false);
            }else tab.Active(true);
        }
    }
}

