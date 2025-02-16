using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class UIQuestInfo : MonoBehaviour
{
    [SerializeField] private Quest<MissionTarget> m_Quest;
    [SerializeField] private TextMeshProUGUI m_TxtQuestDescription;
    [SerializeField] private TextMeshProUGUI m_TxtQuestProcess;
    [SerializeField] private TextMeshProUGUI m_TextReward;
    [SerializeField] private Image m_ImgQuestProcess;
    [SerializeField] private Button m_BtnClaim;
    [SerializeField] private Image m_ImgBtnAds;

    private void Awake()
    {
        m_BtnClaim.onClick.AddListener(ClaimQuest);
    }
    public void InitData(Quest<MissionTarget> quest)
    {
        m_Quest = quest;
        if (m_Quest is DailyQuest)
            UpdateUIDailyQuest();
        else if (m_Quest is RockieQuest)
            UpdateUIRockieQuest();
        UpdateUIQuest();
        UpdateButtonStatus();
    }
    private void UpdateUIDailyQuest()
    {
        m_TxtQuestDescription.text = (m_Quest as DailyQuest).GetMissionTarget().ToString();
        m_TextReward.text = (m_Quest as DailyQuest).GetReward().amount.ToString();
        m_ImgBtnAds.gameObject.SetActive((QuestCollectType)m_Quest.GetQuestType() == QuestCollectType.ADS);
        //m_ImgBtnFree.gameObject.SetActive((QuestType)m_Quest.GetQuestType() == QuestType.FREE);
    }
    private void UpdateUIRockieQuest()
    {
        Debug.Log(m_Quest.id + "/" + m_Quest.mt + "/" + (m_Quest as RockieQuest).GetMissionTarget());
        m_TxtQuestDescription.text = (m_Quest as RockieQuest).GetMissionTarget().ToString();
        m_TextReward.text = (m_Quest as RockieQuest).GetReward().amount.ToString();
        m_ImgBtnAds.gameObject.SetActive((QuestCollectType)m_Quest.GetQuestType() == QuestCollectType.ADS);
        //m_ImgBtnFree.gameObject.SetActive((QuestType)m_Quest.GetQuestType() == QuestType.FREE);
    }
    private void UpdateUIQuest()
    {
        m_TxtQuestProcess.text = m_Quest.GetProgressString();
        m_ImgQuestProcess.fillAmount = m_Quest.GetProgress();
    }
    private void UpdateButtonStatus()
    {
        m_BtnClaim.interactable = m_Quest.IsCompleted();
    }
    private void ClaimQuest()
    {
        if((QuestCollectType)m_Quest.GetQuestType() == QuestCollectType.ADS)
        {

        }
        ProfileManager.PlayerData.AddGameResource((m_Quest as DailyQuest).GetReward());
    }
    [Button]
    public void DoQuest()
    {
        m_Quest.cl = m_Quest.tgm;
    }
}
