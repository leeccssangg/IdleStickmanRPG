using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ClassifyGear
{
    public List<GearConfig> ItemCommons;
    public List<GearConfig> ItemUnCommons;
    public List<GearConfig> ItemRares;
    public List<GearConfig> ItemEpics;
    public List<GearConfig> ItemLegendarys;
    public List<GearConfig> ItemImortals;
    public List<GearConfig> ItemAncients;
    
    public ClassifyGear Init()
    {
        ItemCommons = new();
        ItemUnCommons = new();
        ItemRares = new();
        ItemEpics = new();
        ItemLegendarys = new();
        ItemImortals = new();
        ItemAncients = new();
        return this;
    }   
}

