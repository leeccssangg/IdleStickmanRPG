using Spine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UISkillPanel:UICanvas {
    public List<UISkillSlot> m_UISkillSlot = new();
    public Button m_ButtonAuto;
    public GameObject m_AutoOn;
    public GameObject m_AutoOff;
    private void Awake() {
        m_ButtonAuto.onClick.AddListener(OnClickAuto);
    }
    public override void Setup() {
        List<SkillSlotData> slotData = SkillManager.Ins.SkillsData.SlotsData;
        for(int i = 0;i < m_UISkillSlot.Count;i++) {
            UISkillSlot ui = m_UISkillSlot[i];
            SkillSlotData data = slotData[i];
            ui.Setup(slotData[i]);
            ui.UpdateUI();
        }
        UpdateAutoButton(SkillManager.Ins.IsAutoAttack());
    }
    public void UpdateUIskillSlot(int id){
        m_UISkillSlot[id-1].UpdateUI();
    }
    public void AddSkillTolineUp(Skill skill,int slotID) {
        UISkillSlot UISkillSlot = GetSlot(slotID);
        UISkillSlot.SetupSkill(skill);
    }
    public void RemoveSkill(int slotID,bool active = true) {
        UISkillSlot UISkillSlot = GetSlot(slotID);
        UISkillSlot.Free();
    }
    public void RemoveSkill(int skillID) {
        //int slotID = ProfileManager.PlayerData.GetSkillData(skillID).GetSlotID();
       // GetSlot(slotID).Free();
    }
    public void UpdateSlot(int slotID,Skill skill) {
        UISkillSlot ui = GetSlot(slotID);
    }
    public UISkillSlot GetSlot(int id) {
        return m_UISkillSlot.FirstOrDefault(x => x.SlotID == id);
    }
    
    public void OnClickAuto() {
        bool isAuto = SkillManager.Ins.ChangeAutoAttack();
        UpdateAutoButton(isAuto);
    }
    private void UpdateAutoButton(bool auto) {
        m_AutoOn.SetActive(auto);
        m_AutoOff.SetActive(!auto);
    }
}
