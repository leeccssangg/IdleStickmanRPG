using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UIArmorGearInfomation :UICanvas {
    //GearInfo
    public UIArmorGear m_InfoImage;
    //Text
    [BoxGroup("Text")]
    public TextMeshProUGUI m_GearName;
    [BoxGroup("Text")]
    public TextMeshProUGUI m_GearTextRarity;
    [BoxGroup("Text")]
    public TextMeshProUGUI m_GearTextLevel;
    
    [BoxGroup("Text")]
    public TextMeshProUGUI m_TextOwnerEffect;
    [BoxGroup("Text")]
    public TextMeshProUGUI m_TextOwnerEffectValue;
    [BoxGroup("Text")]
    public TextMeshProUGUI m_TextEquippedEffect;
    [BoxGroup("Text")]
    public TextMeshProUGUI m_TextEquippedEffectValue;
    
    [BoxGroup("Text")]
    public TextMeshProUGUI m_TextTotalOwnerEffect;
    //Slider
    public UISlider m_PieceProcess;
    // GearList
    public UIArmorGear m_UIGearPrefab;
    private Pextension.MiniPool<UIArmorGear> m_UIGearPool = new ();
    public Transform m_UIContainer;
    private List<ArmorGearInfo> m_GearInfoList = new List<ArmorGearInfo>();
    private readonly Dictionary<ArmorGearInfo,UIArmorGear> m_UIGearDic =new();
    
    
    
    //Button
    [BoxGroup("Button")]
    public UIButton m_ButtonEquip;
    [BoxGroup("Button")]
    public UIButton m_ButtonEnhance;
    [BoxGroup("Button")]
    public UIButton m_ButtonEnhanceAll;
    [BoxGroup("Button")]
    public UIButton m_ButtonSummon;
    [BoxGroup("Button")]
    public UIButton m_ButtonClose;

    public AutoScroll m_AutoScroll;

    private GearType m_GearType;
    private ArmorGearInfo m_GearSelect;
    private UnityAction m_EquipCallback;

    private void Awake() {
        m_ButtonEquip.onClick.AddListener(OnEquip);
        m_ButtonEnhance.onClick.AddListener(OnUpgrade);
        m_ButtonClose.onClick.AddListener(OnClose);
        m_ButtonEnhanceAll.onClick.AddListener(OnEnhanceAll);
    }
    public override void Open() {
        base.Open();
        
    }
    public void Setup(UnityAction equipCallback) {
        m_EquipCallback = equipCallback;
        m_UIGearPool.Release();
        m_GearInfoList = GearManager.Ins.ArmorGearManager.InfoList;
        m_UIGearPool.OnInit(m_UIGearPrefab,m_GearInfoList.Count,m_UIContainer);
        m_UIGearDic.Clear();
        int count = m_GearInfoList.Count;
        for(int i = 0;i < count;i++) {
            ArmorGearInfo gear = m_GearInfoList[i];
            UIArmorGear ui = m_UIGearPool.Spawn(m_UIContainer.position,Quaternion.identity);
            ui.Setup(gear,SelectGear);
            m_UIGearDic.Add(gear,ui);
        }
        m_AutoScroll.AutoScrollToTaget(Mathf.CeilToInt((float)count/5),m_GearInfoList.IndexOf(GearManager.Ins.ArmorGearManager.GetEquippedInfo()));
        transform.DOKill();
        transform.DOScale(1,0.3f).From(0).SetEase(Ease.OutBack);
        SetupText();
        UpdateGearSelect();
        UpdateUIGear();

    }
    private void SetupText() {
        m_TextOwnerEffect.text = "Owner Effect";
        m_TextEquippedEffect.text = "Equipped Effect";
    }

    private void UpdateUIGear() {
        foreach(ArmorGearInfo gear in m_UIGearDic.Keys) {
            UIArmorGear ui = m_UIGearDic[gear];
            ui.UpdateUI();
        }
    }
    private void UpdateUIPanel() {
        UpdateUIGearEquippedInfo();
        UpdateButton();
    }
    private void UpdateGearSelect() {
        ArmorGearInfo gearEquipped = GearManager.Ins.ArmorGearManager.GetEquippedInfo();
        SelectGear(gearEquipped);
    }
    private void SelectGear(ArmorGearInfo gear) {
        foreach(UIArmorGear ui in m_UIGearDic.Values) {
            ui.SetSelect(false);
        }
        if(gear == null) {
            gear = m_GearInfoList[0];
            if(GearManager.Ins.ArmorGearManager.CheckGearEquippedAble(gear)){
                m_GearSelect = gear;
                OnEquip();
                return;
            }
        }
        m_GearSelect = gear;
        m_UIGearDic[m_GearSelect].SetSelect(true);
        UpdateUIPanel();
    }

    private void UpdateUIGearEquippedInfo(){
        m_InfoImage.Setup(m_GearSelect);
        m_GearName.text = m_GearSelect.ItemName;
        m_GearTextLevel.text = "Level "+ m_GearSelect.Level;
        m_GearTextRarity.text = m_GearSelect.Rarity.ToString();
        m_PieceProcess.SetProgress(m_GearSelect.GetLevelProgress());
        m_PieceProcess.SetTextProgress(m_GearSelect.GetLevelProgressString());
        
        BigNumber ownedEffectStat = m_GearSelect.GetOwnedEffect();
        string ownedEffectValue = ownedEffectStat.ToString3();
        m_TextOwnerEffectValue.text = $"HP +<color=green>{ownedEffectValue}%</color>";
        
        BigNumber equippedEffectStat = m_GearSelect.GetEquippedEffect();
        string equippedEffectValue = equippedEffectStat.ToString3();
        m_TextEquippedEffectValue.text = $"HP +<color=green>{equippedEffectValue}%</color>";
        
        UpdateOwnerEffect();
        
    }

    private void UpdateButton() {
        bool equipAble = GearManager.Ins.ArmorGearManager.CheckGearEquippedAble(m_GearSelect);
        bool upgradeAble = m_GearSelect.UpgradeAble();
        m_ButtonEquip.interactable = equipAble;
        m_ButtonEnhance.interactable = upgradeAble;
    }
    private void UpdateOwnerEffect() {
        BigNumber ownedEffectStat = GearManager.Ins.ArmorGearManager.GetHpOwnedEffect();
        const string s1 = "Owner Effect";
        string sValue = ownedEffectStat.ToString3();
        var s = $"{s1}: <color=yellow>HP+{sValue}%</color>";
        m_TextTotalOwnerEffect.text = s;
    }
    private void OnEquip() {
        GearManager.Ins.ArmorGearManager.EquipGear(m_GearSelect);
        UpdateUIGear();
        UpdateUIPanel();
        m_EquipCallback?.Invoke();
    }

    private void OnUpgrade() {
        GearManager.Ins.ArmorGearManager.UpgradeGear(m_GearSelect);
        UpdateUIGearEquippedInfo();
        m_UIGearDic[m_GearSelect].UpdateUIOnUpgrade();
        m_EquipCallback?.Invoke();
    }

    private void OnEnhanceAll(){
        GearManager.Ins.EnhanceAllGear(GearType.ARMOR);
        UpdateUIPanel();
        m_EquipCallback?.Invoke();
    }

    private void OnClose() {
        transform.DOKill();
        transform.DOScale(0,0.2f).SetEase(Ease.InBack).OnComplete(()=>base.Close(0));
    }
}
