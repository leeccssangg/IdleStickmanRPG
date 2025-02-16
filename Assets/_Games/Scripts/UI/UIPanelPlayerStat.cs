using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelPlayerStat:UICanvas {
    public GameObject m_UIStatInfoPrefab;
    public Transform m_UIStatInfoContainer;
    private MiniPool<UIStatInfo> m_UIStatInfoPool = new MiniPool<UIStatInfo>();
    private Dictionary<MainStatType,UIStatInfo> m_UIStatInfoDic = new Dictionary<MainStatType,UIStatInfo>();
    private void Start() {
        transform.SetAsLastSibling();
    }
    public override void Setup() {
        m_UIStatInfoDic = new Dictionary<MainStatType, UIStatInfo>();
        UpdateStat();
    }
    public void UpdateStat() {
        List<MainStatConfig> list = StatManager.Ins.StatGlobalConfig.mainStatsConfigs;
        m_UIStatInfoPool.Init(list.Count,m_UIStatInfoPrefab,m_UIStatInfoContainer);
       
        for(int i = 0;i < m_UIStatInfoPool.Count;i++) {
            UIStatInfo ui = m_UIStatInfoPool[i];
            if(i < list.Count) {
                MainStatConfig statData = list[i];
                ui.gameObject.SetActive(true);
                ui.SetUp(statData.statType);
                m_UIStatInfoDic.Add(statData.statType,ui);
            } else {
                ui.gameObject.SetActive(false);
            }
        }
    }
    public void UpdateUIStat(MainStatType statType) {
        m_UIStatInfoDic.TryGetValue(statType,out var ui);
        Debug.Log(m_UIStatInfoDic.Count);
        if(ui != null) {
            ui.UpdateUI();
        }
    }
}
