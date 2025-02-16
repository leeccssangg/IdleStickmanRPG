using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPanelSkillInfo:UICanvas {

    public UISkilll m_ImageSkill;
    public SkillInfo m_SkillInfo;

    public Text m_TextSkillName;
    public Text m_TextLevel;
    public Text m_TextRarity;
    public Text m_TextDescription;
    public Text m_TextCooldowntime;
    public Text m_TextOwnedEffect;

    public UIButton m_ButtonClose;
    public UIButton m_ButtonEquip;
    public UIButton m_ButtonRemove;
    public UIButton m_ButtonEnhance;

    public UISlider m_UIProgressPiece;
    
    public UnityAction<SkillInfo> OnUpgrageCallback;
    public UnityAction<SkillInfo> OnEquipCallback;
    public UnityAction<SkillInfo> OnRemoveCallback;

    private int m_Id;
    private void Awake() {
        m_ButtonClose.onClick.AddListener(OnClose);
        m_ButtonEnhance.onClick.AddListener(OnUpgrage);
        m_ButtonEquip.onClick.AddListener(OnEquip);
        m_ButtonRemove.onClick.AddListener(OnRemove);
    }
    public void Setup(SkillInfo SkillInfo) {
        m_SkillInfo = SkillInfo;
        //m_SkillData = ProfileManager.PlayerData.GetSkillData(m_Id);
        //m_SkillDataConfig = GameData.Ins.GetSkillDataConfig(m_Id);

        //Icon
        m_TextSkillName.text = m_SkillInfo.Name;
        m_TextRarity.text = m_SkillInfo.Rarity.ToString();
        m_TextCooldowntime.text = m_SkillInfo.CoolDownTime.ToString() +"seconds";
        UpdateUI();
    }
    private void UpdateUI() {
        m_ImageSkill.Setup(m_SkillInfo);
        m_TextLevel.text = "LV." + m_SkillInfo.Level.ToString();
        m_TextDescription.text = "Skill Des";
        m_TextOwnedEffect.text = $"ATK +{m_SkillInfo.GetOwnerEffect().ToString3()}%";

        m_UIProgressPiece.SetProgress(m_SkillInfo.GetLevelProgress());
        m_UIProgressPiece.SetTextProgress(m_SkillInfo.GetLevelProgressString());

        m_ButtonEnhance.interactable = m_SkillInfo.UpgradeAble();

        bool isLock = m_SkillInfo.IsLock();
        m_ButtonEquip.interactable = !isLock;
        m_ButtonRemove.gameObject.SetActive(m_SkillInfo.IsEquipped());
        m_ButtonEquip.gameObject.SetActive(!m_SkillInfo.IsEquipped());
    }
    public void OnUpgrage() {
        OnUpgrageCallback?.Invoke(m_SkillInfo);
        UpdateUI();
    }
    public void OnEquip() {
        OnEquipCallback?.Invoke(m_SkillInfo);
        OnClose();
    }
    public void OnRemove() {
        OnRemoveCallback?.Invoke(m_SkillInfo);
        OnClose();
    }
    public void OnClose() {
        base.Close(0);
    }
}
