using Pextension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillSlot:MonoBehaviour {

    public SkillSlotData slotData;
    public int skillID;
    public Image m_SkillIcon;
    public UISkillSlider m_CooldownPanel;
    public UISkillSlider m_AttackingPanel;
    public GameObject m_PanelLock;
    public GameObject m_PanelFree;
    public GameObject m_PanelReady;
    public Button m_ButtonAttack;
    public Button m_ButtonAddSkill;
    private Skill m_Skill;



    private bool m_IsAttacking;
    private bool m_IsCooldown;

    public int SlotID { get => slotData.i;}


    private void Awake() {
        m_ButtonAttack.onClick.AddListener(OnUsing);
        m_ButtonAddSkill.onClick.AddListener(OnClickAdd);
    }
    private void Update() {
        if(m_Skill != null) {
            switch(m_Skill.State) {
                case SkillState.COOLDOWN:
                    CoolDown();
                    break;
                case SkillState.ATTACK:
                    Attack();
                    break;
                case SkillState.WAIT:
                    ReadyAttack();
                    break;
                default:
                    break;
            }
        }
    }
    public void Setup(SkillSlotData data) {
        slotData = data;
        UpdateUI();
    }
    public void UpdateUI() {
        if(slotData == null) return;
        if(slotData.u == 0) {
            Lock();
        } else {
            if(slotData.si == -1) {
                Free();
            }
        }
    }
    public void SetupSkill(Skill skill) {
        m_Skill = skill;
        m_PanelReady.SetActive(false);
        m_PanelFree.SetActive(false);
        m_IsAttacking = false;
        m_IsCooldown = false;
        m_CooldownPanel.SetupSlider(m_Skill.CooldownTime);
        m_AttackingPanel.SetupSlider(m_Skill.GetAttackTime());
        m_SkillIcon.gameObject.SetActive(true);
    }
    public void CoolDown() {
        //if(m_IsCooldown) return;
        //m_IsCooldown = true;
        //m_IsAttacking = false;
        m_CooldownPanel.SetActive(true);
        if(m_AttackingPanel.gameObject.activeSelf) m_AttackingPanel.SetActive(false);
        m_CooldownPanel.UpdateSlider(m_Skill.CurrentCountingTime);
    }
    public void Attack() {
        //if(m_IsAttacking) return;
        //m_IsAttacking = true;
        //m_IsCooldown = false;
        m_AttackingPanel.SetActive(true);
        m_PanelReady.SetActive(false);
        m_CooldownPanel.SetActive(false);
        m_AttackingPanel.UpdateSlider(m_Skill.CurrentCountingTime);
    }
    public void ReadyAttack() {
        m_CooldownPanel.SetActive(false);
        m_AttackingPanel.SetActive(false);
        m_PanelReady.SetActive(true);
    }
    public void OnUsing() {
        m_PanelReady.SetActive(false);
        SkillManager.Ins.TriggerAttackSkill(m_Skill);
    }
    public void OnClickAdd() {
        if(IngameManager.Ins.CurrentGameMode != GameMode.CAMPAIGN) return;
        UIManager.Ins.GetUI<UIPanelMainButton>().OpenTabHero(HeroTab.SKILL);
    }
    public void Lock() {
        m_Skill = null;
        m_CooldownPanel.SetActive(false);
        m_AttackingPanel.SetActive(false);
        m_PanelFree.SetActive(false);
        m_SkillIcon.gameObject.SetActive(false);
        m_PanelReady.SetActive(false);
        m_PanelLock.SetActive(true);
    }
    public void Free() {
        m_Skill = null;
        m_CooldownPanel.SetActive(false);
        m_AttackingPanel.SetActive(false);
        m_PanelLock.SetActive(false);
        m_SkillIcon.gameObject.SetActive(false);
        m_PanelReady.SetActive(false);
        m_PanelFree.SetActive(true);
    }
}
