using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CollectorData {
    /// <summary>
    /// ID
    /// </summary>
    public int i = 0;
    /// <summary>
    /// level
    /// </summary>
    public int lv = 1;
    /// <summary>
    /// Piece
    /// </summary>
    public int p = 0;
    /// <summary>
    /// Is Unlock
    /// </summary>
    public int ul = 0;
    /// <summary>
    /// SlotID
    /// </summary>
    public int si = 0;

    public void Init(int id) {
        i = id;
        lv = 1;
        p = 0;
        //ul = 0;
        //si = 0;
    }
    public void LevelUp(){
        lv++;
    }
    public void SetLevel(int level) {
        lv = level;
    }
    public int GetLevel() {
        return lv;
    }
    public int GetPiece() {
        return p;
    }
    public void AddPiece(int amount) {
        p += amount;
    }
    public void ConsumePiece(int amount) {
        if(p >= amount) {
            p -= amount;
        }
    }
    public int GetSlotID() {
        return this.si;
    }
    public void SetSlotID(int id) {
        si = id;
    }
    public bool IsEquiped() {
        return si != 0;
    }
    public bool isLocked() {
        return lv == 1 && p == 0;
    }
    public bool IsMaxLevel() {
        return lv == 100;
    }
    public bool IsGoodToUpgrade() {
        if(isLocked()) return false;
        if(IsMaxLevel()) return false;
        int piece = GetUpgradePiece();
        if(p > piece) return true;
        return false;
    }
    public int GetUpgradePiece() {
        return GameData.Ins.GetLeveupRequiredPiece(GetLevel() + 1);
    }
}
