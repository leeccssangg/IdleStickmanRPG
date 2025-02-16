using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICDungeon : UICanvas
{
    Pextension.MiniPool<UIDungeonInfo> pool = new();
    [SerializeField] private UIDungeonInfo m_UIDungeonInfoPrefab;
    [SerializeField] private RectTransform m_Container;
    [SerializeField] private List<Dungeon> m_ListDungeons;
    [SerializeField] private List<UIDungeonInfo> m_ListUIDungeonInfo;
    
    [SerializeField] private UIButton m_ButtonRunAway;
    [SerializeField] private TextMeshProUGUI m_TextDungeon;
    [SerializeField] private TextMeshProUGUI m_TextChallengeInProgress;

    //[SerializeField] private Button m_BtnClose;
    [SerializeField] private GameObject m_PanelDungeonInProgress;
    [SerializeField] private GameObject m_PanelListDungeon;

    private void Awake()
    {
        m_ButtonRunAway.onClick.AddListener(OnRunAway);
        pool.OnInit(m_UIDungeonInfoPrefab,5, m_Container);
        
        //m_BtnClose.onClick.AddListener(OnClose);
        LoadData();
    }
    public void LoadData()
    {
        //ClearListDungeonUI();
        m_ListDungeons.Clear();
        m_ListDungeons = DungeonManager.Ins.GetAllDungeon();
        for(int i = 0;i< m_ListDungeons.Count; i++)
        {
            UIDungeonInfo uIDungeonInfo = pool.Spawn(m_Container.position, Quaternion.identity);
            uIDungeonInfo.InitData(m_ListDungeons[i],OnEnterDungeon);
            m_ListUIDungeonInfo.Add(uIDungeonInfo);
        }
    }
    public override void Setup(){
        base.Setup();
        m_PanelListDungeon.SetActive(true);
        m_PanelDungeonInProgress.SetActive(false);
    }
    private void OnEnterDungeon(Dungeon curDungeon){
        OpenPanelDungeonInProgress(curDungeon);
        DOVirtual.DelayedCall(2, () => SetInteractableButtonRunAway(true));
    }
    private void OnOpenDungeonDetail(Dungeon dungeon){
        
    }
    private void OpenPanelDungeonInProgress(Dungeon dungeon)
    {
        m_PanelDungeonInProgress.SetActive(true);
        m_PanelListDungeon.SetActive(false);
        SetInteractableButtonRunAway(false);
        
        string s = $"The ringeleadrs are coming! Defeat them to earn {dungeon.GetDungeonConfig().GetDungeonLevelConfigByLevel(dungeon.GetCurrentLevel()).Gift.amount} gold!";
        m_TextDungeon.text = s;
        string s1 = $"Challenge in progress";
        m_TextChallengeInProgress.text = s1;
    }
    public void SetInteractableButtonRunAway(bool isInteractable)
    {
        m_ButtonRunAway.interactable = isInteractable;
    }
    private void OnRunAway()
    {
        IngameManager.Ins.OutCurrentBattle();
        ClosePanelDungeonInProgress();
    }
    public void DungeonComplete(Dungeon dungeon){
        OnOpenDungeonDetail(dungeon);
        ClosePanelDungeonInProgress();
        UIManager.Ins.OpenUI<UIPopupDungeonReward>();
    }
    public void ClosePanelDungeonInProgress(){
        m_PanelListDungeon.SetActive(true);
        m_PanelDungeonInProgress.SetActive(false);
        UIManager.Ins.SetActiveMaintab(true);
    }
    private void ClearListDungeonUI()
    {
        foreach(UIDungeonInfo ui in m_ListUIDungeonInfo)
        {
            pool.Despawn(ui);
        }
    }
    private void OnEnable()
    {
        
    }
    private void OnClose()
    {
        UIManager.Ins.OpenUI<UICGamePlay>();
        UIManager.Ins.CloseUI<UICDungeon>();
    }
}
