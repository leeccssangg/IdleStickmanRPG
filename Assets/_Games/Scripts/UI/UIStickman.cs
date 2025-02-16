using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UIStickman:MonoBehaviour {
    public StickmanInfo m_Stickman;

    public UISlider m_PieceProgress;

    //Inamge
    public Image m_StickmanImage;
    public Image m_RarityBg;
    public Image m_RarityBorder;

    public GameObject m_PanelLock;
    public GameObject m_PanelEquiped;
    public GameObject m_ArrowUpgrade;
    // Text
    public TextMeshProUGUI m_TextLevel;
   //Button
    public UIButton m_ButtonSelect;
    public UIButton m_ButtonEquip;
    public UIButton m_ButtonRemove;

    public UnityAction<StickmanInfo> m_OnSelectCallBack;
    public UnityAction<StickmanInfo> m_OnEquipCallBack;
    public UnityAction<StickmanInfo> m_OnRemoveCallBack;


    private void Awake() {
        m_ButtonSelect.onClick.AddListener(OnSelect);
        m_ButtonEquip.onClick.AddListener(OnEquip);
        m_ButtonRemove.onClick.AddListener(OnUnequip);
    }
    public void Setup(StickmanInfo info,UnityAction<StickmanInfo> selectCallback = null,UnityAction<StickmanInfo> EquipCallback = null,UnityAction<StickmanInfo> RemoveCallback = null) {
        m_Stickman = info;
        m_OnSelectCallBack = selectCallback;
        m_OnEquipCallBack = EquipCallback;
        m_OnRemoveCallBack = RemoveCallback;
        m_StickmanImage.sprite = m_Stickman.GetIcon();
        UpdateUI();
    }
    public void UpdateUI() {
        ItemRaritySprite sprite = SpriteManager.Ins.GetItemRaritySprite(m_Stickman.Rarity);
        m_RarityBg.sprite = sprite.background;
        m_RarityBorder.sprite = sprite.border;

        SetLock();
        UpdateLevelText();
        UpdateUIPiece();
        UpdateEquipStatus();
        UpdateButton();
    }
    private void SetLock() {
        if(!m_PanelLock) return;
        bool isUnlocked = !m_Stickman.IsLock();
        m_PanelLock.SetActive(!isUnlocked);
        if(m_TextLevel.gameObject.activeSelf) m_TextLevel.enabled = (isUnlocked);
    }
    private void UpdateUIPiece() {
        if(!m_PieceProgress || !m_PieceProgress.gameObject.activeSelf) return;
        m_PieceProgress.SetProgress(m_Stickman.GetLevelProgress());
        m_PieceProgress.SetTextProgress(m_Stickman.GetLevelProgressString());
        m_ArrowUpgrade.SetActive(m_Stickman.UpgradeAble());
    }
    public void UpdateLevelText() {
        if(m_TextLevel)
            m_TextLevel.text = $"LV.{m_Stickman.Level}";
    }
    private void UpdateEquipStatus() {
        if(m_PanelEquiped)
            m_PanelEquiped.SetActive(m_Stickman.IsEquiped());
    }
    public void UpdateButton() {
        m_ButtonEquip.gameObject.SetActive(m_Stickman.EquipAble());
        m_ButtonRemove.gameObject.SetActive(m_Stickman.IsEquiped());
    }
    public void OnSelect() {
        m_OnSelectCallBack?.Invoke(m_Stickman);
    }
    public void OnEquip() {
        m_OnEquipCallBack?.Invoke(m_Stickman);
    }
    public void OnUnequip() {
        m_OnRemoveCallBack?.Invoke(m_Stickman);
    }
}
