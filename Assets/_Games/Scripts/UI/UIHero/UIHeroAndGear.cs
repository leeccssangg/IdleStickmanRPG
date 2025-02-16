using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHeroAndGear:UICanvas {
    public List<UIGearSlot> m_UIGearSlots;
    public UIPanelGear m_PanelGear;
    private void OnEnable() {
        Setup();
    }
    public override void Setup() {
        m_PanelGear.Setup();
    }
}
