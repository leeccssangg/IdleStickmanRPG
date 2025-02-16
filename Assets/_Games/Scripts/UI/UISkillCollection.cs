using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UISkillCollection:UICanvas {
    // GearList
    public UISkilll m_UISkillPrefab;
    public Transform m_UIContainer;
    public List<UISkillLineUpSlot> m_UISlots = new();
    public List<SkillInfo> m_SkillList = new List<SkillInfo>();
    private Pextension.MiniPool<UISkilll> m_UISkillPool = new();
    private Dictionary<SkillInfo,UISkilll> m_UISkillDic = new();
    //Text
    [SerializeField] private TextMeshProUGUI m_TextOwnedEffect;
    
    //Button
    [SerializeField] private UIButton m_ButtonEnhanceAll;
    [SerializeField] private UIButton m_ButtonSummon;
    private void Awake(){
        m_ButtonEnhanceAll.onClick.AddListener(OnEnhanceAll);
        
    }
    public override void Open() {
        base.Open();
        transform.DOKill();
        transform.DOScale(1,0.2f).From(0).SetEase(Ease.OutBack);
    }
    public override void Setup() {
        m_UISkillPool.Release();
        m_SkillList = SkillManager.Ins.m_SkillInfoList;
        m_UISkillPool.OnInit(m_UISkillPrefab,m_SkillList.Count,m_UIContainer);
        m_UISkillDic.Clear();
        int count = m_SkillList.Count;
        for(int i = 0;i < count;i++) {
            SkillInfo skill = m_SkillList[i];
            UISkilll ui = m_UISkillPool.Spawn(m_UIContainer.position,Quaternion.identity);
            ui.Setup(skill,SelectSkill,OnEquipSkill,OnRemoveSkill);
            m_UISkillDic.Add(skill,ui);
        }
        //m_AutoScroll.AutoScrollToTaget(Mathf.CeilToInt((float)count / 5),m_GearList.IndexOf(GearManager.Ins.GetGearEquipedByType(m_GearType)));
        //UpdateUI();
    }
    public void UpdateUI() {
        foreach(var ui in m_UISkillDic.Values) {
            ui.UpdateUI();
        }
        UpdateTextOwnedEffect();
        UpdateUISlot();
    }
    public void UpdateUISlot() {
        List<SkillSlotData> data = SkillManager.Ins.SkillsData.SlotsData;
        for(int i = 0;i < m_UISlots.Count;i++) {
            UISkillLineUpSlot uiSlot = m_UISlots[i];
            uiSlot.SetUp(data[i],SelectSkill,OnRemoveSkill);
        }
    }
    private void UpdateTextOwnedEffect(){
        var Value = SkillManager.Ins.GetSkillOwnedEffect().ToString3();
        m_TextOwnedEffect.text = $"Owned Effect: + <color=yellow>ATK +{Value}%</color>";
    }
    private void UpgradeSkill(SkillInfo skill) {
        SkillManager.Ins.UpgradeSkill(skill);
        m_UISkillDic[skill].UpdateUI();
        UpdateUISlot();
        UpdateTextOwnedEffect();
    }
    private void OnEquipSkill(SkillInfo skill) {
        if(SkillManager.Ins.HasSlotFree()) {

        } else {

        }
        SkillManager.Ins.EquipSkill(skill);
        m_UISkillDic[skill].UpdateUI();
        UpdateUISlot();
    }
    private void OnRemoveSkill(SkillInfo skill) {
        SkillManager.Ins.RemoveSkill(skill);
        m_UISkillDic[skill].UpdateUI();
        UpdateUISlot();
    }
    private void SelectSkill(SkillInfo skillInfor) {
        UIPanelSkillInfo panel = UIManager.Ins.OpenUI<UIPanelSkillInfo>();
        panel.Setup(skillInfor);
        panel.OnUpgrageCallback = UpgradeSkill;
        panel.OnEquipCallback = OnEquipSkill;
        panel.OnRemoveCallback = OnRemoveSkill;
    }
    
    private void OnEnhanceAll(){
        SkillManager.Ins.EnhanceAllSkill();
        UpdateUI();
    }
}
