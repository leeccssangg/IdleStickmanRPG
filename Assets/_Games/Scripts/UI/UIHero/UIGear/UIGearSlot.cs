using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIGearSlot:MonoBehaviour {
    //Image
    [SerializeField] private Image m_Icon;
    [SerializeField] private Image m_IconType;
    [SerializeField] private Image m_ImageRarity1;
    [SerializeField] private Image m_ImageRarity2;
    //Text
    [SerializeField] private TextMeshProUGUI m_TextLevel;
    //Button
    [SerializeField] private UIButton m_ButtonSelect;
    //GameObject
    [SerializeField] private GameObject m_PanelEquipped;
    [SerializeField] private GameObject m_PanelLock;
    [SerializeField] private GameObject m_Noti;
    
    private UnityAction m_SelectCallback;
    
    private void Awake() {
        m_ButtonSelect?.onClick.AddListener(OnSelect);
    }
    public UIGearSlot SetupWeaponGearSlot(WeaponGearInfo weaponInfo)
    {
        UpdateRarity(weaponInfo.Rarity);
        UpdateIcon(weaponInfo.GetIcon());
        UpdateIconType(weaponInfo.GetIconType()) ;
        UpdateLevelText(weaponInfo.Level);
        return this;
    }
    public UIGearSlot SetupArmorGearSlot(ArmorGearInfo armorInfo)
    {
        UpdateRarity(armorInfo.Rarity);
        UpdateIcon(armorInfo.GetIcon());
        UpdateIconType(armorInfo.GetIconType()) ;
        UpdateLevelText(armorInfo.Level);
        
        return this;
    }
    // public UIGearSlot SetupHelmetGearSlot(HelmetGearInfo helmetInfo)
    // {
    //     UpdateRarity(helmetInfo.Rarity);
    //     UpdateIcon(helmetInfo.GetIcon());
    //     UpdateIconType(helmetInfo.GetIconType()) ;
    //     UpdateLevelText(helmetInfo.Level);
    //     return this;
    // }
    // public UIGearSlot SetupGlovesGearSlot(GlovesGearInfo glovesInfo)
    // {
    //     UpdateRarity(glovesInfo.Rarity);
    //     UpdateIcon(glovesInfo.GetIcon());
    //     UpdateIconType(glovesInfo.GetIconType()) ;
    //     UpdateLevelText(glovesInfo.Level);
    //     return this;
    // }
    
    private void UpdateRarity(Rarity rarity) {
        m_ImageRarity1.sprite = SpriteManager.Ins.GetItemRaritySprite(rarity).background;
        m_ImageRarity2.sprite = SpriteManager.Ins.GetItemRaritySprite(rarity).border;
    }

    private void UpdateIcon(Sprite sprite)
    {
        m_Icon.sprite = sprite;
    }
    private void UpdateIconType(Sprite sprite)
    {
        m_IconType.sprite = sprite;
    }
    private void UpdateLevelText(int level)
    {
        m_TextLevel.text = $"LV.{level}";
    }
    public void SetEquipped(bool isEquipped)
    {
        m_PanelEquipped.SetActive(isEquipped);
    }   
    public UIGearSlot SetLock(bool isLock)
    {
        m_PanelLock.SetActive(isLock);
        return this;
    }
    public void SetNotiActive(bool isNoti)
    {
        m_Noti.SetActive(isNoti);
    }
    public void SetSelectCallback(UnityAction callback)
    {
        m_SelectCallback = callback;
    }
    private void OnSelect(){
        m_SelectCallback?.Invoke();
    }
    // 
    //
    // public UIGear m_UIGear;
    // public TextMeshProUGUI m_LevelText;
    //
    //
    // public GameObject m_PanelLock;
    // public GameObject m_Noti;
    //
    // public Button m_SelectButton;
    // private GearsData m_Data;
    // public void Awake() {
    //     //m_SelectButton.onClick.AddListener(OpenPopupGearDetail);
    // }
    // public void Setup() {
    //     // m_Data = GearManager.Ins.GetGearsDataByType(m_GearType);
    //     // UpdateUIGearSlot();
    // }
    // private void UpdateUIGearSlot(){
    //     // if(m_Data.IsUnlocked()) {
    //     //     m_PanelLock.SetActive(false);
    //     //     if(m_Data.HasEquipped()) {
    //     //         UpdateUIEquipGear();
    //     //     } else {
    //     //         m_UIGear.gameObject.SetActive(false);
    //     //     }
    //     // } else {
    //     //     m_UIGear.gameObject.SetActive(false);
    //     //     m_PanelLock.SetActive(true);
    //     // }
    // }
    // public void UpdateUIEquipGear() {
    //     // GearInfo gear = GearManager.Ins.GetGearEquipedByType(m_GearType);
    //     // if(gear != null){
    //     //     m_UIGear.gameObject.SetActive(true);
    //     //     m_UIGear.Setup(gear).UpdateUI();
    //     // }else{
    //     //     m_UIGear.gameObject.SetActive(false);
    //     // }
    //     
    // }
    // public void OpenPopupGearDetail() {
    //     // if(m_Data.IsUnlocked()) {
    //     //     UIManager.Ins.OpenUI<UIWeaponGearInfomation>().Setup(m_GearType,UpdateUIGearSlot);
    //     // } else {
    //     //     //TODO: noti
    //     // }
    // }
    //
    //
    // public void SetupWeaponGearSlot(WeaponGearInfo weaponInfo)
    // {
    //     // m_UIGear.Setup(weaponInfo);
    //     // m_LevelText.text = weaponInfo.Level.ToString();
    // }
}
