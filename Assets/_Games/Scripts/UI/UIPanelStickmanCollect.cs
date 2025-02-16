using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelStickmanCollect :UICanvas {
    public GameObject m_PanelListStickman;
    public GameObject m_PanelStickmanOnChange;
    public UIStickman m_UIStickmanOnChange;

    public List<UIStickmanLineUpSlot> m_UIStickmanSlot = new List<UIStickmanLineUpSlot>();
    public Transform m_UIStickmanContent;
    public UIStickman m_UIStickmanPrefab;
//Button
    [SerializeField] private Button m_ButtonEndChange;
    [SerializeField] private UIButton m_Button_EnhanceAll;
    [SerializeField] private UIButton m_ButtonSummon;
//Text
    [SerializeField] private TextMeshProUGUI m_TextOwnedEffect;
    
    
    private Pextension.MiniPool<UIStickman> m_UIStickmanPools = new();
    private Dictionary<StickmanInfo,UIStickman> m_UIStickmanDic = new();
    private void Awake(){
        m_Button_EnhanceAll.onClick.AddListener(OnEnhanceAll);
    }
    
    public void SetupPanel() {
        m_UIStickmanDic.Clear();
        List<StickmanInfo > list = StickManManager.Ins.StickmanInfoList;
        int count = list.Count;
        m_UIStickmanPools.Release();
        m_UIStickmanPools.OnInit(m_UIStickmanPrefab,count,m_UIStickmanContent);
        for(int i = 0;i < count;i++) {
            StickmanInfo smif = list[i];
            UIStickman ui = m_UIStickmanPools.Spawn(m_UIStickmanContent.position,Quaternion.identity);
            ui.Setup(smif,OnSellectStickman,OnEquipStickman,OnRemoveStickman);
            m_UIStickmanDic.Add(smif,ui);
        }
    }
    public UICanvas UpdateUIStickmanCollection() {
        foreach(var ui in m_UIStickmanDic.Values) {
            ui.UpdateUI();
        }
        UpdateUISlot();
        UpdateTextOwnedEffect();
        return this;
    }
    public void UpdateUISlot() {
        List<StickmanSlotData> slotData = StickManManager.Ins.Data.SlotsData;
        for(int i = 0;i < m_UIStickmanSlot.Count;i++) {
            UIStickmanLineUpSlot uiSlot = m_UIStickmanSlot[i];
            uiSlot.SetUp(slotData[i],OnSellectStickman,OnRemoveStickman);
        }
    }
    private void UpdateTextOwnedEffect(){
        var value = StickManManager.Ins.GetAttackOwnerEffect().ToString3();
        string s = $"Owned Effect";
        m_TextOwnedEffect.text = $"{s}: + <color=yellow>ATK +{value}%</color>";
    }
    public void OnSellectStickman(StickmanInfo stickman) {
        UIPanelStickmanInfo panel = UIManager.Ins.OpenUI<UIPanelStickmanInfo>();
        panel.Setup(stickman,OnUpgradeStickman,OnEquipStickman,OnRemoveStickman);
    }
    public void OnUpgradeStickman(StickmanInfo stickman) {
        StickManManager.Ins.UpgradeStickman(stickman);
        m_UIStickmanDic[stickman].UpdateUI();
        UpdateUISlot();
        UpdateTextOwnedEffect();
    }
    public void OnEquipStickman(StickmanInfo stickman) {
        if(StickManManager.Ins.HasFreeSlot()) {

        } else {

        }
        StickManManager.Ins.EquipStickman(stickman);
        m_UIStickmanDic[stickman].UpdateUI();
        UpdateUISlot();
    }
    public void OnRemoveStickman(StickmanInfo stickman) {
        StickManManager.Ins.RemoveStickman(stickman);
        m_UIStickmanDic[stickman].UpdateUI();
        UpdateUISlot();
    }
    private void OnEnhanceAll(){
        StickManManager.Ins.EnhanceAllStickman();
        UpdateUIStickmanCollection();
    }
    //public void OnExecuteAddSkillToSlot(int skillID) {
    //    //LineUp freeSlot = ProfileManager.PlayerData.GetStickmanFreeSlot();
    //    //if(freeSlot != null) {
    //    //    AddStickmanToFreeSlot(skillID,freeSlot.sid);
    //    //} else {
    //    //    SetupChangeStickmanToSlot(skillID);
    //    //}
    //}
    //private void SetupChangeStickmanToSlot(int smID) {
    //   // m_PanelListStickman.SetActive(false);
    //   // m_PanelStickmanOnChange.SetActive(true);
    //   //// m_UIStickmanOnChange.Setup(smID);
    //   // m_UIStickmanOnChange.SetOnChange();
    //   // m_SkillIDChange = smID;
    //   // SetupLineUpStickmanSlot(true);
    //}
    //public void AddStickmanToFreeSlot(int smID,int slotID) {
    //    //ProfileManager.PlayerData.EquipStickman(slotID,smID);
    //    //GetUIStickman(smID).UpdateUIEquip();
    //    //SetupLineUpStickmanSlot();
    //    //IngameManager.Ins.m_HeroManager.AddStickman(smID,slotID);
    //}
    //public void OnChangeStickmanToSlot(int smID) {
    //    int removeSkill = smID;
    //    int currentSlot = ProfileManager.PlayerData.GetStickmanData(smID).GetSlotID();

    //    OnRemoveStickman(removeSkill);
    //    AddStickmanToFreeSlot(m_SkillIDChange,currentSlot);
    //    OnEndChangeStickman();
    //}
    //public void OnEndChangeStickman() {
    //    m_PanelListStickman.SetActive(true);
    //    m_PanelStickmanOnChange.SetActive(false);
    //    SetupLineUpStickmanSlot();
    //}
    //public UIStickman GetUIStickman(int id) {
    //    return m_UIStickmanDic[id];
    //}
}
