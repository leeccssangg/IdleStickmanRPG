using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;
using System.Globalization;

public class GatchaButton : MonoBehaviour
{
    [SerializeField] private GatchaType m_GatchaType;
    [SerializeField] private GatchaButtonType m_ButtonType;
    [SerializeField] private float m_Cost;
    [SerializeField] private int m_NumOpen;


    [SerializeField] private UIButton m_Button;
    [SerializeField] private TextMeshProUGUI m_TextCountDown;
    [SerializeField] private TextMeshProUGUI m_TxtCost;
    [SerializeField] private TextMeshProUGUI m_TxtNumOpen;


    private UnityAction<List<GatchaConfigInfo>> actionCallBack;


    private void Awake()
    {
        m_Button.onClick.AddListener(GoGatcha);
    }
    public void Setup(GatchaType gatchaType, GatchaButtonType gatchaButtonType, int numOpen, UnityAction<List<GatchaConfigInfo>> actionCallBack)
    {
        m_GatchaType = gatchaType;
        m_ButtonType = gatchaButtonType;
        m_NumOpen = numOpen;
        this.actionCallBack = actionCallBack;
    }
    public void GoGatcha()
    {
        List<GatchaConfigInfo> listGatcha = GatchaManager.Ins.GoGatcha(m_GatchaType, m_NumOpen, m_ButtonType);
        actionCallBack?.Invoke(listGatcha);
    }
    public GatchaButtonType GetButtonType()
    {
        return m_ButtonType;
    }
    public void UpdateStatus()
    {
        if (isActiveAndEnabled && m_ButtonType == GatchaButtonType.Ads)
        {
            bool isFree = IsFree();
            m_Button.interactable = isFree;
            m_TextCountDown.gameObject.SetActive(!isFree);
            m_TxtCost.gameObject.SetActive(isFree);
            m_TxtNumOpen.gameObject.SetActive(isFree);
            if(!isFree)
                m_TextCountDown.text = GatchaManager.Ins.GetTimeToNextFreeByType(m_GatchaType);
        }
    }
    public bool IsFree()
    {
        return GatchaManager.Ins.IsFreeByType(m_GatchaType);
    }
    private void Update()
    {
        UpdateStatus();
    }
}
public enum GatchaButtonType
{
    Free,
    Ads,
    Gem,
    //Rare,
    //Epic,
}
[System.Serializable]
public class GatchaButtonData
{
    public int aF;
    public string ngt;
    //public bool isPressed;

    //public bool IsPressed { get => isPressed; set => isPressed = value; }
    public int AdsFree { get => aF; set => aF = value; }
    public string NextGatchaTime { get => ngt; set => ngt = value; }    

    public GatchaButtonData()
    {
        //isPressed = false;
        AdsFree = 5;
        NextGatchaTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
    }
    //public GatchaButtonData(bool iP)
    //{
    //    isPressed = iP;
    //}
    //public GatchaButtonData(GatchaButton btn)
    //{
    //    isPressed = btn.GetStatus();
    //}
}
