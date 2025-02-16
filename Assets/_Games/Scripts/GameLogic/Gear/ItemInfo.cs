using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInfo
{
    public int m_Id;
    public string m_ItemName;
    public int Id
    {
        get => m_Id;
        set => m_Id = value;
    }

    public string ItemName
    {
        get => m_ItemName;
        set => m_ItemName = value;
    }
}

[System.Serializable]
public class ItemGearInfo : ItemInfo
{
    [SerializeField] private GearType m_Type;
    [SerializeField] private Rarity m_Rarity;
    protected GearData m_Data;
    public int Piece { get => m_Data.p; private set => m_Data.p = value; }
    public GearType Type { get => m_Type; set => m_Type = value; }
    public int Level { get => m_Data.l; set => m_Data.l = value; }

    public Rarity Rarity
    {
        get => m_Rarity;
        set => m_Rarity = value;
    }
    protected Sprite m_IconType;
    protected Sprite m_Icon;
    
    public bool IsUnlocked(){
        return Piece > 0 || Level > 1;
    }
    public bool IsLock() {
        return Piece == 0 && Level == 1;
    }
    public int GetRequirePieceAmount() {
        return GameData.Ins.GetLeveupRequiredPiece(Level + 1);
    }
    public bool UpgradeAble() {
        return Level < 100 && m_Data.p >= GetRequirePieceAmount();
    }
    public void UpgradeMaxLevel(){
        while(UpgradeAble()){
            Upgrade();
        }
    }
    public void Upgrade() {
        Piece -= GetRequirePieceAmount();
        Level++;
    }
    public float GetLevelProgress() {
        return (float)Piece/GetRequirePieceAmount();
    }
    public string GetLevelProgressString() {
        return $"{Piece}/{GetRequirePieceAmount()}";
    }
    
    public Sprite GetIcon()
    {
        if(m_Icon == null) m_Icon = SpriteManager.Ins.GetGearSprite(Type,Id);
        return m_Icon;
    }
    public Sprite GetIconType()
    {
        if(m_IconType == null) m_IconType = SpriteManager.Ins.GetGearTypeSprite(Type);
        return m_IconType;
    }
}
