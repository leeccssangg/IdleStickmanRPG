using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIWeaponGear : MonoBehaviour
{
    [SerializeField] private UIItem m_UIItem;
    [SerializeField] private GameObject m_SelectPanel;
    [SerializeField] private UIButton m_SelectButton;
    
    private WeaponGearInfo m_WeaponGearInfo;
    private UnityAction<WeaponGearInfo> m_SelectCallback;
    
    private void Awake()
    {
        m_SelectButton?.onClick.AddListener(OnSelect);
    }
    public UIWeaponGear Setup(WeaponGearInfo weaponGearInfo,UnityAction<WeaponGearInfo> selectCallback = null)
    {
        m_WeaponGearInfo = weaponGearInfo;
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
        m_UIItem.UpdateImageIcon(m_WeaponGearInfo.GetIcon());
    }
    private void UpdateRarityImage()
    {
        m_UIItem.UpdateImageRarity(m_WeaponGearInfo.Rarity);
    }
    public void UpdateUIOnUpgrade()
    {
        UpdateLevel();
        UpdatePanelLock(m_WeaponGearInfo.IsLock());
        UpdateArrowUpgrade();
    }
    public void UpdateUIOnEquip()
    {
        UpdatePanelEquipped();
    }
    private void UpdateLevel()
    {
        m_UIItem.UpdateTextLevel(m_WeaponGearInfo.Level);
        m_UIItem.UpdateUIPiece(m_WeaponGearInfo.GetLevelProgress(), m_WeaponGearInfo.GetLevelProgressString());
    }
    private void UpdatePanelLock(bool isLock)
    {
        m_UIItem.UpdatePanelLock(isLock);
    }

    private void UpdatePanelEquipped()
    {
        bool isEquipped = m_WeaponGearInfo.IsEquipped();
        m_UIItem.UpdatePanelEquipped(isEquipped);
    }

    private void UpdateArrowUpgrade()
    {
        m_UIItem.UpdateArrowUpgrade(m_WeaponGearInfo.UpgradeAble());
    }
    public void SetSelect(bool isSelect)
    {
        if(m_SelectPanel) m_SelectPanel.SetActive(isSelect);
    }
    public void OnSelect()
    {
        m_SelectCallback?.Invoke(m_WeaponGearInfo);
    }
}
