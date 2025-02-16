using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISkilll : MonoBehaviour
{
    private SkillInfo m_Skill;
    public UISlider m_PieceProgress;
    //Inamge
    public Image m_SkillImage;
    public Image m_RarityBg;
    public Image m_RarityBorder;
    // Text
    public TextMeshProUGUI m_TextLevel;

    public GameObject m_PanelLock;
    public GameObject m_PanelEquiped;
    public GameObject m_ArrowUpgrade;

    public UIButton m_ButtonSelect;
    public UIButton m_ButtonEquip;
    public UIButton m_ButtonUnequip;
    private UnityAction<SkillInfo> m_SelectCallback;
    public UnityAction<SkillInfo> m_OnEquipCallBack;
    public UnityAction<SkillInfo> m_OnRemoveCallBack;

    private void Awake() {
        m_ButtonSelect?.onClick.AddListener(OnSelcet);
        m_ButtonEquip?.onClick.AddListener(OnEquip);
        m_ButtonUnequip?.onClick.AddListener(OnRemove);
    }
    public UISkilll Setup(SkillInfo skill,UnityAction<SkillInfo> selectCallback = null,UnityAction<SkillInfo> EquipCallback = null,UnityAction<SkillInfo> RemoveCallback = null) {
        m_Skill = skill;
        m_SelectCallback = selectCallback;
        m_OnEquipCallBack = EquipCallback;
        m_OnRemoveCallBack = RemoveCallback;
        UpdateUI();
        return this;
    }
    public void UpdateUI() {
        ItemRaritySprite sprite = SpriteManager.Ins.GetItemRaritySprite(m_Skill.Rarity);
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
        bool isUnlocked = !m_Skill.IsLock();
        m_PanelLock.SetActive(!isUnlocked);
        if(m_TextLevel.gameObject.activeSelf) m_TextLevel.enabled = (isUnlocked);
    }
    private void UpdateUIPiece() {
        if(!m_PieceProgress || !m_PieceProgress.gameObject.activeSelf) return;
        m_PieceProgress.SetProgress(m_Skill.GetLevelProgress());
        m_PieceProgress.SetTextProgress(m_Skill.GetLevelProgressString());
        m_ArrowUpgrade.SetActive(m_Skill.UpgradeAble());
    }
    private void UpdateEquipStatus() {
        if(m_PanelEquiped) m_PanelEquiped.SetActive(m_Skill.IsEquipped());
    }
    public void UpdateLevelText() {
        if(m_TextLevel)
            m_TextLevel.text = $"LV.{m_Skill.Level}";
    }
    public void UpdateButton() {
        m_ButtonEquip.gameObject.SetActive(m_Skill.EquipAble());
        m_ButtonUnequip.gameObject.SetActive(m_Skill.IsEquipped());
    }
    public void OnSelcet() {
        m_SelectCallback?.Invoke(m_Skill);
    }
    public void OnEquip() {
        m_OnEquipCallBack?.Invoke(m_Skill);
    }
    public void OnRemove() {
        m_OnRemoveCallBack?.Invoke(m_Skill);
    }
}
