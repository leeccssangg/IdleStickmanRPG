using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanProfileData 
{
    /// <summary>
    /// Stickman ID
    /// </summary>
    public int cid = 1;
    /// <summary>
    /// Stikman class
    /// </summary>
    public Class clt = Class.WARRIOR;
    /// <summary>
    /// Stickman rank
    /// </summary>
    public int rk = 0;
    /// <summary>
    /// Stickman level
    /// </summary>
    public int lv = 1;
    /// <summary>
    /// Stickman Piece
    /// </summary>
    public int p = 0;
    /// <summary>
    /// Is Unlock
    /// </summary>
    public int ul = 0;

    private int SlotID = 0;

    private StickmanDataConfig m_StickmanDataConfig = null;



    public int GetLevel() {
        return lv;
    }
    public int GetSlotID() {
        return SlotID;
    }
    public int GetRank() {
        return rk;
    }
    public int GetID() {
        return cid;
    }
    public StickmanDataConfig GetStickmanDataConfig() {
        if(m_StickmanDataConfig == null) {
            //m_StickmanDataConfig = GameData.Instance.Get
        }
        return m_StickmanDataConfig;
    }

}

