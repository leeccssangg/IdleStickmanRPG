using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSkillLineup:MonoBehaviour {
    public List<UISKillLineUpSlot> m_UISkillSlot = new List<UISKillLineUpSlot>();

    public void Setup() {

    }

    public void AddSkillToLineUp(int skillID,int slotID) {
        UISKillLineUpSlot UISlot = m_UISkillSlot[slotID - 1];
        
    }
    public int GetFreeSlot() {
        return 0;
    }
    public bool HasSlotFree() {
        return false;
    }
}
