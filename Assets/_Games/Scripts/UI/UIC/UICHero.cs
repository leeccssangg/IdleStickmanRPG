using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pextension;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public enum HeroTab {NONE =-1,HERO,SKILL,MASTERY,TRAIT}
public class UICHero:UICanvas {
    [Header("UI Panels")]
    [SerializeField] private UIHeroAndGear m_UIHeroAndGear;
    [SerializeField] private UIPlayerStatsTalentTree m_UIPlayerStatsTalentTree;
    [SerializeField] private UIArtifacts m_UIArtifacts;
    [SerializeField] private UISkillCollection m_UISkillCollection;
    
    [Header("DoTween")]
    [SerializeField] private Transform m_ContentTransform;
    [SerializeField] private DOTweenAnimation m_DoTweenAnimation;

    [Header("Buttons")]
    //[SerializeField] private List<Button> m_ButtonList;

    [SerializeField] private Button m_BtnHero;
    [SerializeField] private Button m_BtnSkill;
    [SerializeField] private Button m_BtnMastery;
    [SerializeField] private Button m_BtnTrait;

    [SerializeField] private TabGroupButton m_TabButton;
    private HeroTab m_CurrentTab = HeroTab.NONE;
    private void OnEnable(){
        m_ContentTransform.localPosition = new Vector3(0,-2000,0);
    }
    private void Awake() {
        m_UIArtifacts.AwakeInGame();
        m_UISkillCollection.Setup();
        SetupButton();
    }
    private void SetupButton(){
        m_TabButton.Setup<HeroTab>(OnOpenTab);
    }
    public override void Setup() {
        m_UIPlayerStatsTalentTree.Setup();
        m_UIArtifacts.Setup();
    }
    public void OpenTab(HeroTab tab = HeroTab.NONE) {
        if(tab == HeroTab.NONE ){
            if(m_CurrentTab == HeroTab.NONE){
                OnClickTabButton(HeroTab.HERO);
            }else{
                tab = m_CurrentTab;
                m_CurrentTab = HeroTab.NONE;
                OnClickTabButton(tab);
            }
        }else{
            OnClickTabButton(tab);
        }
        m_ContentTransform.GetComponent<RectTransform>().DOAnchorPosY(1040, 0.45f).SetEase(Ease.OutBack);
        // m_DoTweenAnimation.tween.Restart();
    }
    private void OnClickTabButton(HeroTab tab){
        m_TabButton.OnClickButton(tab);
    }
    private void OnOpenTab(HeroTab tab){
        if (m_CurrentTab == tab) return;
        Debug.Log("OnOpenTab: " + tab);
        CloseAllPanel();
        m_CurrentTab = tab;
        switch (tab){
            case HeroTab.HERO:
                OpenPanelHero();
                break;
            case HeroTab.SKILL:
                OpenPanelSkill();
                break;
            case HeroTab.MASTERY:
                OpenPanelMastery();
                break;
            case HeroTab.TRAIT:
                OpenPanelTrait();
                break;
            case HeroTab.NONE:
            default:
                break;
        }
    }
    private void OpenPanelMastery()
    {
        m_UIPlayerStatsTalentTree.gameObject.SetActive(true);
    }
    private void OpenPanelHero() {
        m_UIHeroAndGear.gameObject.SetActive(true);
        m_UIHeroAndGear.Setup();
    }
    private void OpenPanelTrait() {
        m_UIArtifacts.gameObject.SetActive(true);
    }
    private void OpenPanelSkill() {
        m_UISkillCollection.gameObject.SetActive(true);
        m_UISkillCollection.UpdateUI();
    }
    private void CloseAllPanel() {
        m_UIHeroAndGear.gameObject.SetActive(false);
        m_UIPlayerStatsTalentTree.gameObject.SetActive(false);
        m_UIArtifacts.gameObject.SetActive(false);
        m_UISkillCollection.gameObject.SetActive(false);
    }
}
