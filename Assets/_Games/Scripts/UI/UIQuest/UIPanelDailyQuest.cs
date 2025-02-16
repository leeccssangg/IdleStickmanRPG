using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Pextension;

public class UIPanelDailyQuest : MonoBehaviour
{
    Pextension.MiniPool<UIQuestInfo> pool = new();
    [SerializeField] private UIQuestInfo m_UIQuestInfoPrefab;
    [SerializeField] private RectTransform m_Container;
    [SerializeField] private List<DailyQuest> m_ListDailyQuests;
    [SerializeField] private List<UIQuestInfo> m_ListUIQuestInfos;

    public void AwakeInGame()
    {
        pool.OnInit(m_UIQuestInfoPrefab, 15, m_Container);
        m_ListDailyQuests = DailyQuestManager.Ins.GetListDailyQuests();
    }
    public void Setup()
    {
        if(m_ListDailyQuests == null) return;
        if(m_ListUIQuestInfos == null || m_ListUIQuestInfos.Count == 0)
        {
            InitData();
        }
        else
        {
            UpdateProcess();
        }
    }
    private void InitData()
    {
        m_ListUIQuestInfos = new List<UIQuestInfo>();
        for (int i = 0; i < m_ListDailyQuests.Count; i++)
        {
            UIQuestInfo uIQuestInfo = pool.Spawn(m_Container.position, Quaternion.identity);
            uIQuestInfo.InitData(m_ListDailyQuests[i]);
            m_ListUIQuestInfos.Add(uIQuestInfo);
        }
    }
    private void UpdateProcess()
    {
        for (int i = 0; i < m_ListDailyQuests.Count; i++)
        {
            m_ListUIQuestInfos[i].InitData(m_ListDailyQuests[i]);
        }
    }
}
