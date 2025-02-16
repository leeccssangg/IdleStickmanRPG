using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BottomTabController : MonoBehaviour
{
    public RectTransform m_Target;
    public List<BottonTabItem> m_ItemTab;
    // private void Start() {
    //     for(int i = 0;i < m_ItemTab.Count;i++) {
    //         m_ItemTab[i].SetupScale(DeselectOther);
    //         m_ItemTab[i].SetupUIC(OpenUICId);
    //     }
    //
    //     float x = m_Target.rect.width / 5;
    //     float defaultX = 174;
    //
    //     if(x < defaultX) {
    //         RectTransform a = GetComponent<RectTransform>();
    //         a.anchorMin = m_Target.anchorMin;
    //         a.anchorMax = m_Target.anchorMax;
    //         a.anchoredPosition = m_Target.anchoredPosition;
    //         a.sizeDelta = m_Target.sizeDelta;
    //     }
    // }
    private void DeselectOther(Vector2 scale) {
        for(int i = 0;i < m_ItemTab.Count;i++) {
            m_ItemTab[i].Deselect(scale);
            //if (m_ItemTab[i].m_OnSelected)
            //    m_ItemTab[i].m_OnSelected = false;
        }
    }
    private void OpenUICId(int i, bool isOpen)
    {
        switch (i)
        {
            case 0:
                if(isOpen)
                    UIManager.Ins.OpenUI<UICHero>().Setup();
                else
                    UIManager.Ins.CloseUI<UICHero>();
                break;
            case 1:
                if(isOpen)
                     UIManager.Ins.OpenUI<UIPanelStickmanCollect>().UpdateUIStickmanCollection();
                else
                    UIManager.Ins.CloseUI<UIPanelStickmanCollect>();
                break;
            case 2:
                if(isOpen)
                    UIManager.Ins.OpenUI<UICDungeon>().Setup();
                else
                    UIManager.Ins.CloseUI<UICDungeon>();
                break;
            case 3:
                if (isOpen)
                    UIManager.Ins.OpenUI<UICMineResearch>();
                else
                    UIManager.Ins.CloseUI<UICMineResearch>();
                break;
            case 4:
                if(isOpen)
                    UIManager.Ins.OpenUI<UICShop>().OpenTab();
                else
                    UIManager.Ins.CloseUI<UICShop>();
                break;
        }
    }
}
