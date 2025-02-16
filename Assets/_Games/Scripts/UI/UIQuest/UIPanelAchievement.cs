using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Pextension;

public class UIPanelAchievement : MonoBehaviour
{
    Pextension.MiniPool<UIQuestInfo> pool = new();
    [SerializeField] private UIQuestInfo m_UIQuestInfoPrefab;
    [SerializeField] private RectTransform m_Container;
    //[SerializeField] private List<DailyQuest> m_ListDailyQuests;
    [SerializeField] private List<UIQuestInfo> m_ListUIQuestInfos;

    public void AwakeInGame()
    {
        pool.OnInit(m_UIQuestInfoPrefab, 15, m_Container);
    }
    public void LoadData()
    {
        //m_ListDailyQuests = DailyQuestManager.Ins.GetListDailyQuests();
        //for (int i = 0; i < m_ListDailyQuests.Count; i++)
        //{
        //    UIQuestInfo uIQuestInfo = pool.Spawn(m_Container.position, Quaternion.identity);
        //    uIQuestInfo.InitData(m_ListDailyQuests[i]);
        //    m_ListUIQuestInfos.Add(uIQuestInfo);
        //}
    }
}
