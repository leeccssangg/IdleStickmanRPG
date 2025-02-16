using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGatchaTab : MonoBehaviour
{
    [SerializeField] private GatchaType m_GatchaType;
    [SerializeField] private int m_NumOpenBox1;
    [SerializeField] private int m_NumOpenBox2;

    [SerializeField] private Image m_ImgIcon;
    [SerializeField] private Image m_ImgProcess;

    [SerializeField] private TextMeshProUGUI m_TxtLevel;
    [SerializeField] private TextMeshProUGUI m_TxtPoint;

    [SerializeField] private GatchaButton m_BtnOpenBox1;
    [SerializeField] private GatchaButton m_BtnOpenBox2;
    [SerializeField] private GatchaButton m_BtnAds;

    public void Setup()
    {
        m_NumOpenBox1 = GatchaManager.Ins.GetNumOpenBox1ByType(m_GatchaType);
        m_NumOpenBox2 = GatchaManager.Ins.GetNumOpenBox2ByType(m_GatchaType);
        SetupUI();
        SetupButton();
    }
    private void SetupUI()
    {
        m_ImgProcess.fillAmount = GatchaManager.Ins.GetCurrentProcessByType(m_GatchaType);
        m_TxtLevel.text = GatchaManager.Ins.GetGatchaLevelByType(m_GatchaType).ToString();
        m_TxtPoint.text = GatchaManager.Ins.GetCurrentPointByType(m_GatchaType).ToString() 
                            + "/" + GatchaManager.Ins.GetCurrentUpgradeNeededByType(m_GatchaType).ToString(); 
    }
    private void SetupButton()
    {
        m_BtnOpenBox1.Setup(m_GatchaType, GatchaButtonType.Gem, m_NumOpenBox1, OnOpenBoxCompleted);
        m_BtnOpenBox2.Setup(m_GatchaType, GatchaButtonType.Gem, m_NumOpenBox2, OnOpenBoxCompleted);
        m_BtnAds.Setup(m_GatchaType, GatchaButtonType.Ads, m_NumOpenBox2, OnOpenBoxCompleted);
    }
    private void OnOpenBoxCompleted(List<GatchaConfigInfo> listGatcha)
    {
        if (listGatcha != null)
        {
            UICGatchaReturn uICItemReturn = UIManager.Ins.OpenUI<UICGatchaReturn>();
            uICItemReturn.CollectAllItem();
            uICItemReturn.Setup(listGatcha);
        }
        SetupUI();
    }
}
