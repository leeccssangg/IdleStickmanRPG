using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Rarity { COMMON = 1, UNCOMMON = 2, RARE = 3, EPIC = 4, LEGEND = 5, IMMORTAL = 6, ANCIENT = 7 , NONE = 0}
public class DeviceConfigData {
    public int id;
    public int rank;
    public int characterID;
    public List<StatConfig> statConfigs = new List<StatConfig>();
   // public EquipmentType equipmentType;
    //public void Init( int id, int characterID, EquipmentType equipmentType ) {
    //    this.id = id;
    //    this.characterID = characterID;
    //    this.equipmentType = equipmentType;
    //}
    public void AddStat( StatConfig statconfig ) {
        //statConfigs.Add(statconfig);
    }
}

public struct StatConfig {
    public float value;
    public float increaseValue;
    public float GetValue( int level ) {
        return value + increaseValue * level;
    }
    public float GetValue() {
        return value;
    }
}
// public enum StatType {
//     NONE = 0,
//     HP_PERCENT = 1,
//     ATK_PERCENT = 2,
//     CRIT = 3,
//     PIERCE = 4,
//     KNOCK_BACK = 5,
//     STUN = 6,
//     ICE = 7,
//     POISON = 8,
//     FIRE = 9,
//     ATK_SPEED = 10,
//     TIME_UP = 11,
//     REDUCE_DAMAGE = 12,
//     BLOCK = 13,
//     SKILL_DAMAGE = 14,
//     DISARM = 15,
//     SILENCE = 16,
//     LIGHTING = 17,
//     HEALTH = 18,
//     ARMOR_BREAK = 19,
//     CURSE = 20,
//     TIME_DOWN = 21,
//     ARMOR_UP = 22,
//     CHAOS = 23,
//     DOUBLE_SHOT = 24,
//     MISS = 25,
//     CRIT_DAMAGE = 26,
//     STUN_CHANCE = 27,
//     BUFF_TIME = 28,
//     GROW_UP = 29,
//     CONTROL_IMMUNE = 30,
//     REFLECT = 31,
//     DISARM_CHANCE = 32,
//     SILENT_CHANCE = 33,
//     TRUE_SHOT_CHANCE = 34,
//     TRUE_SHOT_PER = 35,
//     MULTIPLE = 36,
//     SLOW_CHANCE = 37,
//     LIGHT_CHANCE = 38,
//     POISON_CHANCE = 39,
//     REDUCE_DAMAGE_BUFF = 40,
//     ATK_BUFF = 41,
//     AS_BUFF = 42,
//     SHIELD = 43,
//     HP_POINT = 44,
//     ATK_POINT = 45,
//     LAST_WORD = 46
// }
[System.Serializable]
public class RaritySprite {
    public Rarity rarity;
    public Sprite borderSprite;
    public Sprite centerHeroSprite;
    public Sprite centerItemSprite;
    public Color rarityColor;
}

