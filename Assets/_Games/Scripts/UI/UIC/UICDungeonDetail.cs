using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UICDungeonDetail : UICanvas{
    [SerializeField] private Dungeon m_Dungeon;
    [SerializeField] private DungeonLevelConfig m_CurLevelConfig;
    [SerializeField] private int m_CurLevel;

    [SerializeField] private TextMeshProUGUI m_TxtDungeonName;
    [SerializeField] private TextMeshProUGUI m_TxtKeyCount;
    [SerializeField] private TextMeshProUGUI m_Level;
    [SerializeField] private TextMeshProUGUI m_Reward;
    [SerializeField] private Button m_BtnEnterDungeon;
    [SerializeField] private Button m_BtnClose;
    [SerializeField] private Button m_BtnFoward;
    [SerializeField] private Button m_BtnBackward;


    private UnityAction m_EnterCallback;
    private void Awake(){
        m_BtnEnterDungeon.onClick.AddListener(() => EnterDungeon(m_CurLevel));
        m_BtnBackward.onClick.AddListener(() => { Move(true); });
        m_BtnFoward.onClick.AddListener(() => { Move(false); });
        m_BtnClose.onClick.AddListener(OnClose);
    }
    public void InitData(Dungeon dungeon, UnityAction enterCallback){
        m_Dungeon = dungeon;
        m_CurLevel = m_Dungeon.GetCurrentLevel();
        EventManager.StartListening("Dungeon" + m_Dungeon.GetDungeonName() + "NextLevel", () => { UpdateUIConfig(m_CurLevel); });
        UpdateUIDungeon();
        UpdateUIConfig(m_CurLevel);
        m_EnterCallback = enterCallback;
    }
    private void UpdateUIDungeon()
    {
        m_TxtDungeonName.text = m_Dungeon.GetDungeonName();
        if(m_Dungeon.GetFreeBonusKeyCount() > 0)
            m_TxtKeyCount.text = "Key: " + m_Dungeon.GetFreeBonusKeyCount().ToString() + "/" + m_Dungeon.GetNumKeyADay().ToString();
        else
            m_TxtKeyCount.text = "Ads: " + m_Dungeon.GetAllKeyCount().ToString() + "/" + m_Dungeon.GetNumKeyADay().ToString();
    }
    private void UpdateUIConfig(int level)
    {
        m_CurLevel = level;
        m_CurLevelConfig = m_Dungeon.GetDungeonConfig().GetDungeonLevelConfigByLevel(m_CurLevel);
        m_Level.text = m_CurLevel.ToString();
        m_Reward.text = "Reward: " + m_CurLevelConfig.Gift.amount.ToString();
        UpdateButtonStatus();
    }
    private void UpdateButtonStatus()
    {
        m_BtnEnterDungeon.interactable = m_Dungeon.GetAllKeyCount() > 0;
        m_BtnFoward.interactable = m_CurLevel < m_Dungeon.GetCurrentLevel();
        m_BtnBackward.interactable = m_CurLevel > 1;
    }
    private void EnterDungeon(int id)
    {
        m_Dungeon.CheckEnterDungeon(id);
        m_EnterCallback?.Invoke();
        OnClose();
    }
    private void Move(bool isBackWard)
    {
        if (isBackWard)
            m_CurLevel--;
        else
            m_CurLevel++;
        UpdateUIConfig(m_CurLevel);
    }
    private void OnEnable()
    {
        
    }
    private void OnClose()
    {
        EventManager.StopListening("Dungeon" + m_Dungeon.GetDungeonName() + "NextLevel", () => { UpdateUIConfig(m_CurLevel); });
        UIManager.Ins.CloseUI<UICDungeonDetail>();
    }
}
