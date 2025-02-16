using Pextension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShopTab { NONE = -1, SUMMON, PACKAGE, PERIODIC, SPECIAL, PREMIUM}
public class UICShop : UICanvas
{
    [Header("UI Panels")]
    [SerializeField] private UIPanelGatcha m_UIPanelGatcha;

    [SerializeField] private TabGroupButton m_TabButton;
    private ShopTab m_CurrentTab = ShopTab.NONE;

    private void Awake()
    {
        SetupButon();
    }
    private void SetupButon()
    {
        m_TabButton.Setup<ShopTab>(OnOpenTab);
    }
    public void OpenTab(ShopTab tab = ShopTab.NONE)
    {
        if (tab == ShopTab.NONE) tab = m_CurrentTab;
        if (m_CurrentTab == ShopTab.NONE) tab = ShopTab.SUMMON;
        m_TabButton.OnClickButton(tab);
    }
    private void OnOpenTab(ShopTab tab)
    {
        if (m_CurrentTab == tab) return;
        Debug.Log("OnOpenTab: " + tab);
        CloseAllPanel();
        m_CurrentTab = tab;
        switch (tab)
        {
            case ShopTab.SUMMON:
                OpenPanelGatcha();
                break;
            case ShopTab.PACKAGE:
                break;
            case ShopTab.PERIODIC:
                break;
            case ShopTab.SPECIAL:
                break;
            case ShopTab.PREMIUM:
                break;
            case ShopTab.NONE:
            default:
                break;
        }
    }
    private void OpenPanelGatcha()
    {
        m_UIPanelGatcha.gameObject.SetActive(true);
        m_UIPanelGatcha.Setup();
    }
    private void CloseAllPanel()
    {
        m_UIPanelGatcha.gameObject.SetActive(false);
    }
}
