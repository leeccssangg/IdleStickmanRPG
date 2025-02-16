using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICGamePlay : UICanvas
{
    [SerializeField] private Button m_BtnDungeon;
    [SerializeField] private Button m_BtnDailyQuest;
    [SerializeField] private Button m_BtnDailyGift;
    [SerializeField] private Button m_BtnPlayerStatsTalentTree;
    [SerializeField] private Button m_BtnSpinWheel;
    [SerializeField] private Button m_BtnMineResearch;
    [SerializeField] private Button m_BtnRockieQuest;
    [SerializeField] private Button m_BtnArtifact;

    [Header("Cheat buttons")]
    [SerializeField] private Button m_BtnCheatRockieQuest;

    private void Awake()
    {
        m_BtnDungeon.onClick.AddListener(OpenDungeonUI);
        m_BtnDailyQuest.onClick.AddListener(OpenDailyQuestUI);
        m_BtnDailyGift.onClick.AddListener(OpenDailyGiftUI);
        m_BtnPlayerStatsTalentTree.onClick.AddListener(OpenPSTalentTree);
        m_BtnSpinWheel.onClick.AddListener(OpenSpinWheelUI);
        m_BtnMineResearch.onClick.AddListener(OpenMineResearchUI);
        m_BtnRockieQuest.onClick.AddListener(OpenRockieQuestUI);
        m_BtnArtifact.onClick.AddListener(OpenArtifactUI);

        m_BtnCheatRockieQuest.onClick.AddListener(CheatRockieQuest);
    }
    private void OpenDungeonUI()
    {
        UIManager.Ins.OpenUI<UICDungeon>(); //.LoadData();
        UIManager.Ins.CloseUI<UICGamePlay>();
    }
    private void OpenDailyQuestUI()
    {
        UIManager.Ins.OpenUI<UICMainQuest>().OpenTab();
        UIManager.Ins.CloseUI<UICGamePlay>();
    }
    private void OpenPSTalentTree()
    {
        UIManager.Ins.OpenUI<UIPlayerStatsTalentTree>();
        UIManager.Ins.CloseUI<UICGamePlay>();
    }
    private void OpenDailyGiftUI()
    {
        UIManager.Ins.OpenUI<UICDailyGift>().InitData();
        UIManager.Ins.CloseUI<UICGamePlay>();
    }
    private void OpenSpinWheelUI()
    {
        UIManager.Ins.OpenUI<UICSpinWheel>();
        UIManager.Ins.CloseUI<UICGamePlay>();
    }
    private void OpenMineResearchUI()
    {
        UIManager.Ins.OpenUI<UICMineResearch>().Setup();
        UIManager.Ins.CloseUI<UICGamePlay>();
    }
    private void OpenRockieQuestUI()
    {
        UIManager.Ins.OpenUI<UICRockieQuest>().OpenTab();
        UIManager.Ins.CloseUI<UICGamePlay>();
    }
    private void OpenArtifactUI()
    {
        UIManager.Ins.OpenUI<UIArtifacts>().Setup();
        UIManager.Ins.CloseUI<UICGamePlay>();
    }


    #region Cheat Btn
    private void CheatRockieQuest()
    {
        RockieQuestManager.Ins.StartRockieQuest();
    }
    #endregion
}
