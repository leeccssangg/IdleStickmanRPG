using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

[GUIColor("@MyExtension.EditorExtension.GetColor(\"Resource\", (int)$value)")]
public enum GearType{
    NONE = -1,
    WEAPON,
    ARMOR,
    HELMET,
    GLOVES,
    SHOES,
}

public class GearManager : SingletonFree<GearManager>,IHpOwnerEffect,IAttackOwnerEffect,IAttackEquippedEffect,IHpEquippedEffect{
     
    [SerializeField] private WeaponGearManager m_WeaponGearManager;
    [SerializeField] private ArmorGearManager m_ArmorGearManager;
    
    [SerializeField] private GearsDatas m_GearsData;
    [SerializeField] private List<GearInfoList> m_GearInfoList;
    
    
    [SerializeField] private GearGlobalConfig m_GearGlobalConfig;
    [SerializeField] private TextAsset m_TextCSV;
    [SerializeField] private TextAsset m_WeaponConfigCSV;
    [SerializeField] private TextAsset m_ArmorConfigCSV;
    [SerializeField] private TextAsset m_HelmetConfigCSV;
    [SerializeField] private TextAsset m_GlovesConfigCSV;
    private GearGlobalConfig GearGlobalConfig{ get => m_GearGlobalConfig; set => m_GearGlobalConfig = value; }

    public WeaponGearManager WeaponGearManager => m_WeaponGearManager;
    public ArmorGearManager ArmorGearManager => m_ArmorGearManager;

    #region SAVE LOAD
    public void LoadData(GearsDatas data){
        WeaponGearManager.LoadData(data.GetDataTypeByType(GearType.WEAPON),GearGlobalConfig.WeaponConfigs);
        m_ArmorGearManager.LoadData(data.GetDataTypeByType(GearType.ARMOR),GearGlobalConfig.ArmorConfigs);
        m_GearsData = data;
    }
    public void SaveData(){
        ProfileManager.Ins.SaveGearDatas(m_GearsData);
    }
#endregion

#region EXECUTE_DATA
[Button]
public void UnlockAWeaponGear(){
    m_GearsData.GetDataTypeByType(GearType.WEAPON).Unlock();
}
public void AddGearPiece(List<GearConfig> list){
    for(int i = 0; i < list.Count; i++){
        GearConfig config = list[i];
        switch (config.gearType)
        {
            case GearType.NONE:
                break;
            case GearType.WEAPON:
                WeaponGearManager.AddPiece(config.id,1);
                break;
            case GearType.ARMOR:
                ArmorGearManager.AddPiece(config.id,1);
                break;
            case GearType.HELMET:
                break;
            case GearType.GLOVES:
                break;
            case GearType.SHOES:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
public void EnhanceAllGear(GearType type){
    List<ItemInfoEnhance> enhanceList = new();
    enhanceList = type switch{
        GearType.WEAPON => WeaponGearManager.EnhanceAllGear(),
        GearType.ARMOR => ArmorGearManager.EnhanceAllGear(),
        _ => enhanceList
    };
    if(enhanceList.Count <= 0) return;
    UIManager.Ins.OpenUI<UIPanelEnhanceAll>().Setup(enhanceList);
}
#endregion
#region STAT
    public BigNumber GetAttackOwnerEffect(){
        BigNumber totalAttack = 0;
        totalAttack += WeaponGearManager.GetAttackOwnedEffect();
        return totalAttack;
    }
    public BigNumber GetHpOwnerEffect(){
        BigNumber totalHp = 0;
        totalHp += ArmorGearManager.GetHpOwnedEffect();
        return totalHp;
    }
    public BigNumber GetAttackEquippedEffect()
    {
        BigNumber totalAttack = 0;
        totalAttack += WeaponGearManager.GetAttackEquippedEffect();
        return totalAttack;
    }
    public BigNumber GetHpEquippedEffect()
    {
        BigNumber totalAttack = BigNumber.ZERO;
        totalAttack += ArmorGearManager.GetHpEquippedEffect();
        return totalAttack;
    }
#endregion
#region GET
    public GearsData GetGearsDataByType(GearType type){
        return m_GearsData.GetDataTypeByType(type);
    }
    public ItemGearInfo GetGearInfoByTypeAndID(GearType type, int id){
        return type switch
        {
            GearType.WEAPON => WeaponGearManager.GetWeaponInfoByID(id),
            GearType.ARMOR => ArmorGearManager.GetArmorInfoByID(id),
            _ => null
        };
    }
    public List<GearInfo> GetGearList(GearType type){
        List<GearInfo> list = new();
        GearInfoList gearInfoList = m_GearInfoList.FirstOrDefault(x => x.gearType == type);
        if(gearInfoList != null){
            list = gearInfoList.gearInfoList;
        }
        else{
            gearInfoList = new GearInfoList
            {
                gearType = type,
                gearInfoList = list
            };
            m_GearInfoList.Add(gearInfoList);
        }
        // switch(type){
        //     case GearType.None:
        //         break;
        //     case GearType.Weapon:
        //         list = m_Weapons;
        //         break;
        //     case GearType.Helmet:
        //         list = m_Helmet;
        //         break;
        //     case GearType.Armor:
        //         list = m_Armor;
        //         break;
        //     case GearType.Gloves:
        //         list = m_Gloves;
        //         break;
        //     case GearType.Shoes:
        //         list = m_Shoes;
        //         break;
        //     case GearType.Necklace:
        //         list = m_Necklaces;
        //         break;
        //     default:
        //         break;
        // }

        return list;
    }
    public bool IsUnlocked(GearType type)
    {
        return type switch
        {
            GearType.WEAPON => m_WeaponGearManager.IsUnLocked(),
            GearType.ARMOR => m_ArmorGearManager.IsUnLocked(),
            _ => false
        };
    }
#endregion
#region Gatcha
    public GearConfig GetRandomGearByRarity(Rarity rarity){
        List<GearsConfig> list = new List<GearsConfig>(m_GearGlobalConfig.allGearConfig);
        List<GearConfig> listRarity = new();

        for (int i = 0; i < m_GearsData.Gds.Count; i++)
        {
            GearsData data = m_GearsData.Gds[i];
            if(data.IsUnlocked()){
                switch (data.GearType)
                {
                    case GearType.NONE:
                        break;
                    case GearType.WEAPON:
                        listRarity.AddRange(WeaponGearManager.GetWeaponConfigByRarity(rarity));
                        break;
                    case GearType.ARMOR:
                        listRarity.AddRange(ArmorGearManager.GetWeaponConfigByRarity(rarity));
                        break;
                    case GearType.HELMET:
                        break;
                    case GearType.GLOVES:
                        break;
                    case GearType.SHOES:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        // List<GearsConfig> list = new List<GearsConfig>(m_GearGlobalConfig.allGearConfig);
        // for(int i = 0; i < list.Count; i++){
        //     GearsConfig config = list[i];
        //     if(GetGearsDataByType(config.gearType).IsUnlocked()){
        //         var listGearRarity = config.gearConfigList.Where(x => x.rarity == rarity).ToList();
        //         listRarity.AddRange(listGearRarity);
        //     }
        // }

        var num = Random.Range(0, listRarity.Count);
        return listRarity[num];
    }
#endregion

#region EDITOR
#if UNITY_EDITOR
    [Button]
    public void ImportCsvData(){
        //ImportStatCofigCSV();
        ImportGearConfigCsv();
        EditorUtility.SetDirty(gameObject);
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
    }
    private void ImportStatCofigCSV(){
        GearGlobalConfig = GearGlobalConfig.Instance;
        EditorUtility.SetDirty(GearGlobalConfig);
        GearGlobalConfig.Load(m_TextCSV);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    private void ImportGearConfigCsv(){
        GearGlobalConfig = GearGlobalConfig.Instance;
        EditorUtility.SetDirty(GearGlobalConfig);
        GearGlobalConfig.LoadWeaponConfig(m_WeaponConfigCSV);
        GearGlobalConfig.LoadArmorConfig(m_ArmorConfigCSV);
        //GearGlobalConfig.LoadHelmetConfig(m_HelmetConfigCSV);
       // GearGlobalConfig.LoadGlovesConfig(m_GlovesConfigCSV);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif
#endregion
}

[System.Serializable]
public class GearInfoList
{
    public GearType gearType;
    public List<GearInfo> gearInfoList = new();
    
    public List<GearInfo> GetGearInfoList()
    {
        return gearInfoList ??= new List<GearInfo>();
    }
}