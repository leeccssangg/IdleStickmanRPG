using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class UIPanelGear : MonoBehaviour
{
     [SerializeField] private UIGearSlot m_WeaponGearSlot;
     [SerializeField] private UIGearSlot m_ArmorGearSlot;
     [SerializeField] private UIGearSlot m_HelmetGearSlot;
     [SerializeField] private UIGearSlot m_GlovesGearSlot;

     public void Setup()
     {
         //Setup Weapon Gear Slot;
         SetupWeaponGearSlot();
         
         //Setup Armor Gear Slot;
         SetupArmorGearSlot();

     }
     private void SetupWeaponGearSlot()
    { 
        bool isUnlocked = GearManager.Ins.IsUnlocked(GearType.WEAPON);
        if (isUnlocked)
        {
            WeaponGearManager weaponGearManager = GearManager.Ins.WeaponGearManager;
            if (weaponGearManager.HasEquipped())
            {
                WeaponGearInfo weaponEquipped = weaponGearManager.GetEquippedWeaponInfo();
                m_WeaponGearSlot.SetupWeaponGearSlot(weaponEquipped).SetEquipped(true);
            }else{
                m_WeaponGearSlot.SetEquipped(false);
            }
        }
        m_WeaponGearSlot.SetLock(!isUnlocked)
                         .SetSelectCallback(isUnlocked ? OpenUIWeaponGearInfo : 
                                                         OnShowPopupNotificationUnlock);
    }
    private void SetupArmorGearSlot()
    {
        bool isUnlocked = GearManager.Ins.IsUnlocked(GearType.ARMOR);
        if (isUnlocked)
        {
            ArmorGearManager armorGearManager = GearManager.Ins.ArmorGearManager;
            if (armorGearManager.HasEquipped())
            {
                ArmorGearInfo armorEquipped = armorGearManager.GetEquippedArmorInfo();
                m_ArmorGearSlot.SetupArmorGearSlot(armorEquipped).SetEquipped(true);
            }else{
                m_ArmorGearSlot.SetEquipped(false);
            }
        }else{
            m_ArmorGearSlot.SetEquipped(false);
        }
        m_ArmorGearSlot.SetLock(!isUnlocked)
                       .SetSelectCallback(isUnlocked ? OpenUIArmorGearInfo : 
                                              OnShowPopupNotificationUnlock);
    }
    
    private void OpenUIWeaponGearInfo()
    {
        UIManager.Ins.OpenUI<UIWeaponGearInfomation>().Setup(SetupWeaponGearSlot);
    }
    private void OpenUIArmorGearInfo()
    {
        UIManager.Ins.OpenUI<UIArmorGearInfomation>().Setup(SetupArmorGearSlot);
    }

    private void OnShowPopupNotificationUnlock()
    {
        Debug.Log("Unlock At Level ..............");
    }
}
