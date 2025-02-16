using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public enum ButtonTabType{
    NONE,
    HERO,
    STICKMAN,
    DUNGEON,
    MONOPOLY,
    SHOP
}
public class UIPanelMainButton : UICanvas
{
    [SerializeField] private ButtonMainTab[] m_ButtonMainTabs;
    private UICanvas m_CurrentUITab;
    private ButtonTabType m_CurrentTabType;
    public override void Setup(){
        base.Setup();
        m_CurrentTabType = ButtonTabType.NONE;
        foreach(var t in m_ButtonMainTabs){
            t.Setup(OnOpenMainTab);
        }
    }
    
    public void OpenTabHero(HeroTab heroTab = HeroTab.HERO){
        OnOpenMainTab(ButtonTabType.HERO);
        if(m_CurrentUITab) m_CurrentUITab.GetComponent<UICHero>().OpenTab(heroTab);
    }
    public void OpenTabStickman(){
        OnOpenMainTab(ButtonTabType.STICKMAN);
    }
    public void OpenTabDungeon(){
        OnOpenMainTab(ButtonTabType.DUNGEON);
    }
    public void OpenTabMine(){
        OnOpenMainTab(ButtonTabType.MONOPOLY);
    }
    public void OpenTabShop(){
        OnOpenMainTab(ButtonTabType.SHOP);
    }
    public void OnOpenMainTab(ButtonTabType tab){
        UpdateTabButton(tab);
        if(m_CurrentTabType != ButtonTabType.NONE){
            OpenTab(tab);
        }
    }
    private void UpdateTabButton(ButtonTabType tab){
        if(m_CurrentTabType == ButtonTabType.NONE){
            GetButtonMainTab(tab)?.OnSelect();
            m_CurrentTabType = tab; 
            return;
        }
        if(m_CurrentTabType == tab){
            GetButtonMainTab(tab)?.OnDeselect();
            m_CurrentTabType = ButtonTabType.NONE;
        }else{
            GetButtonMainTab(m_CurrentTabType)?.OnDeselect();
            GetButtonMainTab(tab)?.OnSelect();
            m_CurrentTabType = tab;
        }
        if(m_CurrentUITab) m_CurrentUITab.CloseDirectly();
    }
    private void OpenTab(ButtonTabType tab){
        switch(tab){
            case ButtonTabType.HERO:
                OpenHeroTab();
                break;
            case ButtonTabType.STICKMAN:
                OpenStickmanTab();
                break;
            case ButtonTabType.DUNGEON:
                OpenDungeonTab();
                break;
            case ButtonTabType.MONOPOLY:
                OpenMonopolyTab(); 
                break;
            case ButtonTabType.SHOP:
                OpenShopTab();
                break;
            case ButtonTabType.NONE:
                break;
            default:
                break;
        }
    }

    private void OpenHeroTab(){
        m_CurrentUITab = UIManager.Ins.OpenUI<UICHero>();
        m_CurrentUITab.Setup();
        m_CurrentUITab.GetComponent<UICHero>().OpenTab();
    }
    private void OpenStickmanTab(){
        m_CurrentUITab = UIManager.Ins.OpenUI<UIPanelStickmanCollect>().UpdateUIStickmanCollection();
    }
    private void OpenDungeonTab(){
        m_CurrentUITab = UIManager.Ins.OpenUI<UICDungeon>();
        m_CurrentUITab.Setup();
    }
    private void OpenMonopolyTab(){
        m_CurrentUITab = UIManager.Ins.OpenUI<CvMonopoly>();
    }
    private void OpenShopTab(){
        m_CurrentUITab = UIManager.Ins.OpenUI<UICShop>();
        m_CurrentUITab.GetComponent<UICShop>().OpenTab();
    }
    private ButtonMainTab GetButtonMainTab(ButtonTabType tab){
        return m_ButtonMainTabs.FirstOrDefault(button => button.ButtonTabTabType == tab);
    }
}
