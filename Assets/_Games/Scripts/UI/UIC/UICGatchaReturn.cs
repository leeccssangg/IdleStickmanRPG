using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UICGatchaReturn : UICanvas
{
    Pextension.MiniPool<UIItemGatcha> m_UIItemGatchaPool = new();
    Pextension.MiniPool<UIItemResource> m_UIItemResourcePool = new();

    [SerializeField] private Transform m_Container;

    [SerializeField] private UIItemGatcha m_UIItemGatchaPrefab;
    [SerializeField] private UIItemResource m_UIItemResourcePrefab;

    [SerializeField] private List<UIItemGatcha> m_UIItemGatchaList;
    [SerializeField] private List<UIItemResource> m_UIItemResourceList;

    [SerializeField] private UIButton m_BtnClose;
    
    private bool m_isDone = false;
    private void Awake()
    {
        m_BtnClose.onClick.AddListener(OnBtnCloseClick);
        m_UIItemGatchaPool.OnInit(m_UIItemGatchaPrefab, 10, m_Container);
        m_UIItemResourcePool.OnInit(m_UIItemResourcePrefab, 10, m_Container);
    }
    public void Setup(List<GatchaConfigInfo> listGatcha){
        m_isDone = false;
        m_UIItemGatchaList.Clear();
        for(int i = 0;i< listGatcha.Count;i++)
        {
            UIItemGatcha uiItemGatcha = m_UIItemGatchaPool.Spawn(m_Container.transform.position,Quaternion.identity);
            uiItemGatcha.InitData(listGatcha[i]);
            uiItemGatcha.transform.SetAsLastSibling();
            m_UIItemGatchaList.Add(uiItemGatcha);
        }
        ShowGatcha();
    }
    private void ShowGatcha(){
        StartCoroutine(CO_ShowGatcha());
    }
    private IEnumerator CO_ShowGatcha()
    {   
        int count = m_UIItemGatchaList.Count;
        float delay = 0.02f;
        m_Container.DOShakePosition(count * 0.04f,25,50,50,false,true);
        for(int i = 0;i< m_UIItemGatchaList.Count;i++)
        {
            UIItemGatcha uiItemGatcha = m_UIItemGatchaList[i];
            uiItemGatcha.Show();
            yield return new WaitForSeconds(delay);
        }

        m_isDone = true;
    }
    public void Setup(List<ResourceData> listResourceData)
    {
        m_UIItemResourceList.Clear();
        for(int i = 0;i< listResourceData.Count;i++)
        {
            UIItemResource uiItemResource = m_UIItemResourcePool.Spawn(m_Container.transform.position,Quaternion.identity);
            uiItemResource.InitData(listResourceData[i]);
            m_UIItemResourceList.Add(uiItemResource);
        }
    }
    public void CollectAllItem()
    {
        m_UIItemGatchaPool.Collect();
        m_UIItemResourcePool.Collect();
    }
    public void OnBtnCloseClick()
    {
        if(!m_isDone) return;
        Close();
    }
}
