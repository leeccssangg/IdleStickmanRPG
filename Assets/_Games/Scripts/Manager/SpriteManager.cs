using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.Rendering;
using Spine.Unity;
using Unity.VisualScripting;
using System;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

public class SpriteManager:SingletonFreeAlive<SpriteManager> {
    public List<Sprite> m_SkillSprites = new List<Sprite>();
    private Dictionary<string,Sprite> m_SkillSpriteDic = new Dictionary<string,Sprite>();

    

    public List<HeroIconSprite> m_WarriorHeroIconSprites = new List<HeroIconSprite>();
    public List<HeroIconSprite> m_ArcherHeroIconSprites = new List<HeroIconSprite>();
    public List<HeroIconSprite> m_MageHeroIconSprites = new List<HeroIconSprite>();
    public List<HeroIconSprite> m_PriestHeroIconSprites = new List<HeroIconSprite>();
    
    public List<ClassSprite> m_ClasstypeSprites = new();
    public List<StickmanSkeleton> m_StickmanSkeleton = new();
    public List<StickmanIcon> m_StickmanSprite = new();
    public List<GearSprite> m_GearSprites = new();
    public List<Sprite> m_GearTypeSprites = new();
    
    public List<RaceSprite> m_StickmanRaceSprite = new();
    
    public List<ItemRaritySprite> m_GearRaritySprite;
    private Dictionary<Rarity,ItemRaritySprite> m_GearRaritySpriteDic = new Dictionary<Rarity,ItemRaritySprite>();
    public List<Sprite> m_GatchaRaritySprites = new List<Sprite>();
    [BoxGroup("Main Stat Sprite")]
    public List<MainStatSprite> m_MainStatSprites = new List<MainStatSprite>();
    protected override void Awake() {
        base.Awake();
        LoadGearRaritySprite();
    }
    private void LoadGearRaritySprite() {
        m_GearRaritySpriteDic.Clear();
        for(int i = 0;i < m_GearRaritySprite.Count;i++) {
            ItemRaritySprite itemRaritySprite = m_GearRaritySprite[i];
            Rarity rarity = itemRaritySprite.rarity;
            m_GearRaritySpriteDic.Add(rarity,itemRaritySprite);
        }
    }
    //GET RARITY SPRITE
    public ItemRaritySprite GetItemRaritySprite(Rarity rarity) {
        return m_GearRaritySpriteDic[rarity];
    }
    //GET STICKMAN RACE SPRITE
    public RaceSprite GetStickmanRaceSprite(Race race) {
        return m_StickmanRaceSprite.FirstOrDefault(x => x.race == race);
    }
    //GET STICKMAN CLASSTYPE SPRITE
    public ClassSprite GetClassSprite(Class classtype) {
        return m_ClasstypeSprites.FirstOrDefault(x =>x.classType == classtype);
    }
    //GET STICKMAN SKELETON
    public SkeletonDataAsset GetStickmanSkeleton(Class classtype, Race race) {
        StickmanSkeleton value = m_StickmanSkeleton.FirstOrDefault(x => x.classType == classtype && x.race == race);
        return value.asset;
    }

    //GET STICKMAN SPRITE
    public Sprite GetStickmanSprite(Class classtype,Race race) {
        return m_StickmanSprite.FirstOrDefault(x => x.race == race).GetSprite(classtype);
    }
    public void InitSprite() {
    }
    public Sprite GetSkillIconSprite(string name) {
        return m_SkillSprites[0];
    }
    public Sprite GetGachaRaritySprite(Rarity rarity) {
        return m_GatchaRaritySprites[(int)rarity - 1];
    }
    public Sprite GetHeroSprite(int cid,Class classType) {
        //Debug.Log("Hero " + cid + " Class " + classType.ToString());
        List<HeroIconSprite> list = null;
        switch(classType) {
            case Class.WARRIOR:
                list = m_WarriorHeroIconSprites;
                break;
            case Class.ARCHER:
                list = m_ArcherHeroIconSprites;
                break;
            case Class.MAGE:
                list = m_MageHeroIconSprites;
                break;
            case Class.PRIEST:
                list = m_PriestHeroIconSprites;
                break;
        }
        for(int i = 0;i < list.Count;i++) {
            HeroIconSprite hero = list[i];
            if(hero.id == cid && hero.classType == classType) {
                return hero.sprite;
            }
        }
        return null;
    }
    public Sprite GetSkillSprite(int skillID) {
         return null;
    }
    //GET GEAR SPRITE
    public Sprite GetGearSprite(GearType gearType,int id) {
        var gearSprite = m_GearSprites.FirstOrDefault(x => x.gearType == gearType);
        return gearSprite?.GearSperiteByID(id);
    }
    internal Sprite GetGearTypeSprite(GearType type){
        return m_GearTypeSprites[(int)type];
    }
    //GET RESOURCE ICON SPRITE
    internal Sprite GetResourceIconSprite(ResourceData.ResourceType type)
    {
        throw new NotImplementedException();
    }
    internal Sprite GetResourceBackgroundSprite(ResourceData.ResourceType type)
    {
        throw new NotImplementedException();
    }
    
    //Get Main Stat Sprite
    public Sprite GetMainStatSprite(MainStatType type){
        return m_MainStatSprites.FirstOrDefault(x => x.Stat == type)?.sprite;
    }
}
[System.Serializable]
public class HeroIconSprite {
    public int id;
    public Class classType;
    public Sprite sprite;
}

[System.Serializable]
public class RaceIconSprite {
    public Race race;
    public Sprite sprite;
    public Color color;
}

[System.Serializable]
public class ClassSprite {
    public Class classType;
    public Sprite sprite;
    public Color color;
}
[System.Serializable]
public class ItemRaritySprite {
    public Rarity rarity;
    [FormerlySerializedAs("backGround")] public Sprite background;
    public Sprite border;
}
[System.Serializable]
public class RaceSprite {
    public Race race;
    public Sprite raceBg;
    public Sprite raceIcon;
}
[System.Serializable]
public class StickmanSkeleton {
    public Race race;
    public Class classType;
    public SkeletonDataAsset asset;
}
[System.Serializable]
public class StickmanIcon {
    public Race race;
    public List<Sprite> iconSprites = new();
    public Sprite GetSprite(Class classType) {
        int index = 0;
        switch(classType) {
            case Class.NONE:
                break;
            case Class.WARRIOR:
                index = 0;
                break;
            case Class.ARCHER:
                index = 1;
                break;
            case Class.MAGE:
                index = 2;
                break;
            case Class.PRIEST:
                index = 3;
                break;
            default:
                break;
        }
        return iconSprites[index];
    }
}
[System.Serializable]
public class GearSprite {
    public GearType gearType;
    [PreviewField]
    public List<Sprite> sprites;

    public Sprite GearSperiteByID(int id){
        var max = sprites.Count;
        var index = id - 1;
        var sp = sprites[index % max];
        return sp ? sp : null;
    }
}
[System.Serializable]
public class MainStatSprite {
    public MainStatType Stat;
    public Sprite sprite;
}
