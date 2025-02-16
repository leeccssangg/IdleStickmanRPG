using System.Collections;
using System.Collections.Generic;
using TMPro;    
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIGear:MonoBehaviour {
    private WeaponGearInfo m_WeaponGearInfo;
    private ArmorGearInfo m_ArmorGearInfo;
    // private HelmetGearInfo m_HelmetGearInfo;
    // private GlovesGearInfo m_GlovesGearInfo;
    
    public UISlider m_PieceProgress;
    //Inamge
    public Image m_GearImage;
    public Image m_RarityBg;
    public Image m_RarityBorder;
    public Image m_IconType;
    // Text
    public TextMeshProUGUI m_TextLevel;

    public GameObject m_PanelLock;
    public GameObject m_PanelEquiped;
    public GameObject m_ArrowUpgrade;

    public GameObject m_Select;
    public Button m_ButtonSelect;
    // private UnityAction<GearInfo> m_SelectCallback;
    private UnityAction<WeaponGearInfo> m_SelectCallback;
    private void Awake() {
        m_ButtonSelect?.onClick.AddListener(OnSelcet);
    }
    public void SetupWeaponGear(WeaponGearInfo weaponInfo) {
        m_WeaponGearInfo = weaponInfo;
    }
    public void UpdateUIWeaponGear() {
       // if(!m_WeaponGearInfo) return;
       // Setup(m_WeaponGearInfo);
        //UpdateUI();
    }
    
    
    
    
    
    
    
    public UIGear Setup(GearInfo gear,UnityAction<GearInfo> callback = null) {
        //m_Gear = gear; 
       //m_SelectCallback = callback;
        SetSprite();
        return this;
    }
    private void SetSprite(){
        ///if(m_GearImage) m_GearImage.sprite = m_Gear.GetIcon();
        //if(m_IconType) m_IconType.sprite = m_Gear.GetIconType();
    }
    public void UpdateUI() {
        //var sprite = SpriteManager.Ins.GetItemRaritySprite(m_Gear.Rarity);
       // m_RarityBg.sprite = sprite.background;
       // m_RarityBorder.sprite = sprite.border;
        SetLock();
        UpdateLevelText();
        UpdateUIPiece();
    }
    private void SetLock() {
        if(!m_PanelLock) return;
        //bool isUnlocked = !m_Gear.IsLock();
        //m_PanelLock.SetActive(!isUnlocked);
        //m_TextLevel.gameObject.SetActive(isUnlocked);
    }
    private void UpdateUIPiece() {
        ///if(!m_PieceProgress || !m_PieceProgress.gameObject.activeSelf) return;
        //m_PieceProgress.SetProgress(m_Gear.GetLevelProgress());
       // m_PieceProgress.SetTextProgress(m_Gear.GetLevelProgressString());
       // m_ArrowUpgrade.SetActive(m_Gear.UpgradeAble());
    }
    public void UpdateLevelText() {
       //if(m_TextLevel) m_TextLevel.text = $"LV.{m_Gear.Level}";
    }
    public void SetSelect(bool select) {
        m_Select.SetActive(select);
    }
    public void OnSelcet() {
       // m_SelectCallback?.Invoke(m_Gear);
    }
    public void SetEquiped(bool equiped) {
        m_PanelEquiped.SetActive(equiped);
    }
}
