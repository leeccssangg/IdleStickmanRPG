using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class UIPanelEnhanceAll : UICanvas{
    Pextension.MiniPool<UIEnhance> m_UIItemEnhancePool = new(); 
    [SerializeField] private UIEnhance m_UIItemEnhancePrefab;
    [SerializeField] private Transform m_Container;
    
    
    [SerializeField] private UIButton m_BtnClose;
    
    private List<UIEnhance> m_ListUIITemEnhance = new List<UIEnhance>();
    private bool m_isShowDone = false;
    private void Awake(){
        m_BtnClose.onClick.AddListener(OnBtnCloseClick);
        m_UIItemEnhancePool.OnInit(m_UIItemEnhancePrefab, 10, m_Container);
    }
    
    public void Setup(List<ItemInfoEnhance> list){
        m_BtnClose.SetScaleZero();
        m_ListUIITemEnhance.Clear();
        m_UIItemEnhancePool.Collect();
        for(int i = 0; i < list.Count; i++){
            UIEnhance ui = m_UIItemEnhancePool.Spawn(m_Container.transform.position,Quaternion.identity);
            ui.transform.SetSiblingIndex(i);
            ui.Setup(list[i]);
            m_ListUIITemEnhance.Add(ui);
        }

        m_isShowDone = false;
        Show();
    }
    public void Show(){
        StartCoroutine(CO_Show());
    }
    private IEnumerator CO_Show(){
        float delay = 0.05f;
        for(int i = 0; i < m_ListUIITemEnhance.Count; i++){
            m_ListUIITemEnhance[i].Show();
            yield return Yielders.Get(delay);
        }
        m_isShowDone = true;
        m_BtnClose.DoScale(1,Ease.OutBack,0.5f);
    }
    public void OnBtnCloseClick()
    {
        if(!m_isShowDone) return;
        Close();
    }
}