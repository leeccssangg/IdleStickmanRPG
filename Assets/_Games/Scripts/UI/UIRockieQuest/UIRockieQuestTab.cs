using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRockieQuestTab : MonoBehaviour
{
    Pextension.MiniPool<UIQuestInfo> pool = new();
    [SerializeField] private UIQuestInfo m_UIQuestInfoPrefab;
    [SerializeField] private RectTransform m_Container;
    [SerializeField] private List<UIQuestInfo> m_ListUIQuestInfos;
    [SerializeField] private List<RockieQuest> m_ListRockieQuest;

    public void AwakeInGame()
    {
        pool.OnInit(m_UIQuestInfoPrefab, 10, m_Container);
    }
    public void InitData(int day)
    {
        pool.Collect();
        m_ListRockieQuest.Clear();
        m_ListRockieQuest = RockieQuestManager.Ins.GetListDailyQuestsByDay(day);
        for(int i = 0; i < m_ListRockieQuest.Count; i++)
        {
            UIQuestInfo ui = pool.Spawn(m_Container.position, Quaternion.identity);
            ui.InitData(m_ListRockieQuest[i]);
            m_ListUIQuestInfos.Add(ui);
        }
    }
}
