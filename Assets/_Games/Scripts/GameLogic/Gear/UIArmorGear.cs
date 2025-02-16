using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIArmorGear : MonoBehaviour
{
    [SerializeField] private UIItem m_UIItem;
    [SerializeField] private GameObject m_SelectPanel;
    [SerializeField] private UIButton m_SelectButton;
    
    private ArmorGearInfo GearInfo;
    private UnityAction<ArmorGearInfo> m_SelectCallback;
    
    private void Awake()
    {
        m_SelectButton?.onClick.AddListener(OnSelect);
    }
    public UIArmorGear Setup(ArmorGearInfo GearInfo,UnityAction<ArmorGearInfo> selectCallback = null)
    {
        this.GearInfo = GearInfo;
        m_SelectCallback = selectCallback;
        UpdateIcon();
        UpdateRarityImage();
        return this;
    }
    public void UpdateUI()
    {
        UpdateUIOnUpgrade();
        UpdateUIOnEquip();
    }
    private void UpdateIcon()
    {
        m_UIItem.UpdateImageIcon(GearInfo.GetIcon());
    }
    private void UpdateRarityImage()
    {
        m_UIItem.UpdateImageRarity(GearInfo.Rarity);
    }
    public void UpdateUIOnUpgrade()
    {
        UpdateLevel();
        UpdatePanelLock(GearInfo.IsLock());
        UpdateArrowUpgrade();
    }
    public void UpdateUIOnEquip()
    {
        UpdatePanelEquipped();
    }
    private void UpdateLevel()
    {
        m_UIItem.UpdateTextLevel(GearInfo.Level);
        m_UIItem.UpdateUIPiece(GearInfo.GetLevelProgress(), GearInfo.GetLevelProgressString());
    }
    private void UpdatePanelLock(bool isLock)
    {
        m_UIItem.UpdatePanelLock(isLock);
    }

    private void UpdatePanelEquipped()
    {
        bool isEquipped = GearInfo.IsEquipped();
        m_UIItem.UpdatePanelEquipped(isEquipped);
    }

    private void UpdateArrowUpgrade()
    {
        m_UIItem.UpdateArrowUpgrade(GearInfo.UpgradeAble());
    }
    public void SetSelect(bool isSelect)
    {
        if(m_SelectPanel) m_SelectPanel.SetActive(isSelect);
    }
    public void OnSelect()
    {
        m_SelectCallback?.Invoke(GearInfo);
    }
}
