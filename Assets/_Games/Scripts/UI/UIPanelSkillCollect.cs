using Sirenix.OdinInspector;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelSkillCollect:UIHeroTab {

    public GameObject m_PanelListSkill;
    public GameObject m_PanelSkillOnChange;
    public UISkill m_UISkillOnChange;

    public List<UISkillLineUpSlot> m_UISkillSlot = new List<UISkillLineUpSlot>();
    public Transform m_UISkillContent;
    public GameObject m_UISkillPrefab;

    public Button m_ButtonEndChange;

    private MiniPool<UISkill> m_UISkillPools = new MiniPool<UISkill>();
    private Dictionary<int,UISkill> m_UISkillDic = new Dictionary<int, UISkill>();
    private int m_SkillIDChange;
    private void Awake() {
        //m_ButtonEndChange.onClick.AddListener(OnEndChangeSkill);
    }
    private void Start() {
        
    }
    public override void Setup() {
        base.Setup();
       // UpdateUISkill();
    }
    // public void UpdateUISkill() {
    //     m_UISkillDic.Clear();
    //     m_PanelListSkill.SetActive(true);
    //     m_PanelSkillOnChange.SetActive(false);
    //     List<SkillDataConfig> skillList = GameData.Ins.GetSkillDataConfigList();
    //     int count = skillList.Count;
    //     m_UISkillPools.Init(count,m_UISkillPrefab,m_UISkillContent);
    //     for(int i = 0;i < count;i++) {
    //         UISkill ui = m_UISkillPools[i];
    //         if(i < count) {
    //             SkillDataConfig skillData = skillList[i];
    //             ui.gameObject.SetActive(true);
    //             ui.Setup(skillData.id);
    //             ui.m_OnSelectCallBack = OnSellectSkill;
    //             ui.m_OnEquipCallBack = OnEquipSkill;
    //             ui.m_OnRemoveCallBack = OnRemoveSkill;
    //             m_UISkillDic.Add(skillData.id,ui);
    //         } else {
    //             ui.gameObject.SetActive(false);
    //         }
    //     }
    //     SetupLineUpSkillSlot();
    // }
    // public void SetupLineUpSkillSlot(bool isChange = false) {
    //     List<LineUp> list = ProfileManager.PlayerData.GetSkillLineUp();
    //     for(int i = 0;i < list.Count;i++) {
    //         LineUp slot = list[i];
    //         UISkillLineUpSlot uiSlot = m_UISkillSlot[i];
    //         if(!slot.IsUnlocked()) {
    //             //uiSlot.Lock();
    //         } else {
    //             if(slot.IsFree()) {
    //                 //uiSlot.FreeSlot();
    //             } else {
    //                 //uiSlot.SetUp(slot.id,isChange);
    //                 if(isChange) {
    //                     uiSlot.m_OnSelectCallback = OnChangeSkillToSlot;
    //                 } else {
    //                     uiSlot.m_OnSelectCallback = OnSellectSkill;
    //                     uiSlot.m_OnRemoveCallback = OnRemoveSkill;
    //                 }
    //             }
    //         }
    //     }
    // }
    // public void OnSellectSkill(int skillID) {
    //     //UIPanelSkillInfo panel = UIManager.Ins.OpenUI<UIPanelSkillInfo>();
    //     //panel.Setup(skillID);
    //     //panel.OnUpgrageCallback = OnUpgradeSkill;
    //     //panel.OnEquipCallback = OnEquipSkill;
    //     //panel.OnRemoveCallback = OnRemoveSkill;
    // }
    // public void OnUpgradeSkill(int id) {
    //     ProfileManager.PlayerData.LevelUpSkill(id);
    //     GetUISkill(id).UpdateUI();
    //     SetupLineUpSkillSlot();
    // }
    // public void OnEquipSkill(int id) {
    //     OnExecuteAddSkillToSlot(id);
    // }
    // public void OnRemoveSkill(int id) {
    //     if(IngameManager.Ins.SkillManager.RemoveSkill(id)) {
    //         ProfileManager.PlayerData.RemoveSkill(id);
    //         GetUISkill(id).UpdateUIEquip();
    //         SetupLineUpSkillSlot();
    //     }
    // }
    // public void OnExecuteAddSkillToSlot(int skillID) {
    //     LineUp freeSlot = ProfileManager.PlayerData.GetSkillFreeSlot();
    //     if(freeSlot != null) {
    //         AddSkillToFreeSlot(skillID,freeSlot.sid);
    //     } else {
    //         SetupChangeSkillToSlot(skillID);
    //     }
    // }
    // private void SetupChangeSkillToSlot(int skillID) {
    //     m_PanelListSkill.SetActive(false);
    //     m_PanelSkillOnChange.SetActive(true);
    //     m_UISkillOnChange.Setup(skillID);
    //     m_UISkillOnChange.SetOnChange();
    //     m_SkillIDChange = skillID;
    //     SetupLineUpSkillSlot(true);
    // }
    // public void AddSkillToFreeSlot(int skillID,int slotID) {
    //     ProfileManager.PlayerData.EquipSkill(slotID,skillID);
    //     GetUISkill(skillID).UpdateUIEquip();
    //     SetupLineUpSkillSlot();
    //     //IngameManager.Ins.m_SkillManager.AddSkillToLineUp(skillID,slotID);
    // }
    // public void OnChangeSkillToSlot(int skillID) {
    //     int removeSkill = skillID;
    //     int currentSlot = ProfileManager.PlayerData.GetSkillData(skillID).GetSlotID();
    //
    //     OnRemoveSkill(removeSkill);
    //     AddSkillToFreeSlot(m_SkillIDChange,currentSlot);
    //     OnEndChangeSkill();
    // }
    // public void OnEndChangeSkill() {
    //     m_PanelListSkill.SetActive(true);
    //     m_PanelSkillOnChange.SetActive(false);
    //     SetupLineUpSkillSlot();
    // }
    // public UISkill GetUISkill(int id) {
    //     return m_UISkillDic[id];
    // }
}
