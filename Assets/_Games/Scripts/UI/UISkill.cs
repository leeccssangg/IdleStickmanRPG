using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISkill:MonoBehaviour {

    public UISlider m_PieceProgressbar;

    public Image m_IconSkill;
    public Image m_ImageRarity;

    public Text m_LevelText;

    public Button m_ButtonSelect;
    public Button m_ButtonEquip;
    public Button m_ButtonRemove;

    public GameObject m_ArrowUpgrade;
    public GameObject m_UILocked;
    public GameObject m_UIUnlocked;
    public GameObject m_UIEquiped;

    public CollectorData m_SkillData;
    public SkillDataConfig m_SkillDataConfig;

    public UnityAction<int> m_OnSelectCallBack;
    public UnityAction<int> m_OnEquipCallBack;
    public UnityAction<int> m_OnRemoveCallBack;
    public int m_ID;

    private void Awake() {
        m_ButtonSelect.onClick.AddListener(OnSelect);
        m_ButtonEquip.onClick.AddListener(OnEquip);
        m_ButtonRemove.onClick.AddListener(OnUnequip);
    }

    public void Setup(int id) {
        m_ID = id;
       // m_SkillData = ProfileManager.PlayerData.GetSkillData(m_ID);
        m_SkillDataConfig = GameData.Ins.GetSkillDataConfig(m_ID);
        // Icon
        // ImageRarity

        UpdateUI();
    }
    public void UpdateUI() {
        int level = 1;
        int piece = 0;
        if(IsUnlocked()) {
            m_UILocked.SetActive(false);
            m_UIUnlocked.SetActive(true);
            level = m_SkillData.GetLevel();
            piece = m_SkillData.GetPiece();
            UpdateUIEquip();

        } else {
            m_UILocked.SetActive(true);
            m_UIUnlocked.SetActive(false);
        }
        m_LevelText.text = $"LV.{level}";
        int requiredPiece = m_SkillDataConfig.GetLevelRequirePiece(level + 1);
        float p = (float)piece / requiredPiece;
        m_PieceProgressbar.SetProgress(p);
        m_PieceProgressbar.SetTextProgress($"{piece}/{requiredPiece}");

        m_ArrowUpgrade.SetActive(p >= 1);
    }
    public void UpdateUIEquip() {
        bool isEquiped = m_SkillData.IsEquiped();
        m_UIEquiped.SetActive(isEquiped);
        m_ButtonEquip.gameObject.SetActive(!isEquiped);
    }
    public void SetOnChange() {
        m_UIEquiped.SetActive(false);
        m_ArrowUpgrade.SetActive(false);
        m_ButtonEquip.gameObject.SetActive(false);
    }
    private bool IsUnlocked() {
        return m_SkillData != null && !m_SkillData.isLocked();
    }
    public void OnSelect() {
        m_OnSelectCallBack?.Invoke(m_ID);
    }
    public void OnEquip() {
        m_OnEquipCallBack?.Invoke(m_ID);
    }
    public void OnUnequip() {
        m_OnRemoveCallBack?.Invoke(m_ID);
    }
}
