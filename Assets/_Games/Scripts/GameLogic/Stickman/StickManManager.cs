using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif


public enum Class{
    NONE = 0,
    WARRIOR = 1,
    ARCHER = 2,
    MAGE = 3,
    PRIEST = 4
}
public enum Race{
    NONE = 0,
    HUMAN = 1,
    DEMON = 2,
    ELF = 3,
    ORC = 4
}
public enum Platform{
    NONE = 0,
    GROUND = 1,
    FLY = 2
}
public class StickManManager : SingletonFree<StickManManager>,IAttackOwnerEffect{
    [FormerlySerializedAs("m_Datas")] [SerializeField]
    private StickmansData m_Data;

    [SerializeField]
    private List<StickmanInfo> m_InfoList = new();

    private Dictionary<int, StickmanInfo> m_EquipDic = new();
    [ShowInInspector]
    private Dictionary<StickmanInfo, Stickman> m_StickManEquippedDic = new();

    // Prefab
    public List<Stickman> m_StickmansPrefab = new();

    [ShowInInspector]
    public Dictionary<string, Stickman> m_StickmanPrefabDic = new();
    

    [SerializeField]
    private StickmanGlobalConfig m_StickmanGlobalConfig;

    [SerializeField]
    private TextAsset m_TextCSV;

    public List<StickmanInfo> StickmanInfoList => m_InfoList;
    public StickmansData Data{ get => m_Data; private set => m_Data = value; }
    
    private readonly List<ItemInfoEnhance> m_ItemInfoEnhances = new();

#region INIT
    public void InitStickman(){
        LoadLineupStickman();
    }
    private void InitPrefab(){
        m_StickmanPrefabDic.Clear();
        foreach(var stickman in m_StickmansPrefab){
            string name = stickman.name;
            stickman.gameObject.SetActive(false);
            m_StickmanPrefabDic.Add(name, stickman);
        }
    }
    private void LoadLineupStickman(){
        List<StickmanSlotData> slotData = m_Data.SlotsData;
        for(int i = 0; i < slotData.Count; i++){
            StickmanSlotData slot = slotData[i];
            if(slot.smi == -1) continue;
            StickmanInfo stickmanInfo = GetStickmanInforByID(slot.smi);
            AddStickmanToBattle(stickmanInfo, slot);
        }
    }
#endregion

#region SaveLoadData
    public void LoadData(StickmansData data){
        Data = data;
        if(Data == null || Data.StickmanDataList.Count == 0){
            CreateNewData();
            SaveData();
        }
        else{
            LoadLocalData();
        }

        StickmanInfoList.Sort(SortingStickman);
        LoadEquipDic();
        InitPrefab();
        UIManager.Ins.GetUI<UIPanelStickmanCollect>().SetupPanel();
    }
    private void CreateNewData(){
        Data = new StickmansData();
        List<StickmanConfig> list = m_StickmanGlobalConfig.configList;
        for(int i = 0; i < list.Count; i++){
            StickmanConfig config = list[i];
            StickmanData data = new(config.id);
            Data.StickmanDataList.Add(data);
            StickmanInfo skf = new(data, config);
            StickmanInfoList.Add(skf);
        }

        for(int i = 1; i <= 6; i++){
            StickmanSlotData slotData = new(i);
            Data.SlotsData.Add(slotData);
        }

        SaveData();
    }
    private void LoadLocalData(){
        List<StickmanConfig> list = new List<StickmanConfig>(m_StickmanGlobalConfig.configList);
        for(int i = 0; i < list.Count; i++){
            StickmanConfig config = list[i];

            int stickmanID = config.id;
            StickmanData data = GetStickmanDataByID(stickmanID);
            if(data == null){
                data = new(config.id);
                Data.StickmanDataList.Add(data);
            }

            StickmanInfo skf = new(data, config);
            StickmanInfoList.Add(skf);
        }

        for(int i = 1; i <= 6; i++){
            StickmanSlotData slotData = GetStickmanSlotDataByID(i);
            if(slotData != null) continue;
            slotData = new(i);
            Data.SlotsData.Add(slotData);
        }
    }
    private void LoadEquipDic(){
        m_EquipDic.Clear();
        for(int i = 0; i < Data.SlotsData.Count; i++){
            StickmanSlotData slot = Data.SlotsData[i];
            StickmanInfo smi = GetStickmanInforByID(slot.smi);
            m_EquipDic.Add(slot.i, smi);
        }
    }
    private static int SortingStickman(StickmanInfo x, StickmanInfo y){
        Rarity xRarity = x.Rarity;
        Rarity yRarity = y.Rarity;
        if(xRarity > yRarity){
            return 1;
        }
        else if(xRarity < yRarity){
            return -1;
        }

        Class xClass = x.Class;
        Class yClass = y.Class;
        if(xClass > yClass){
            return 1;
        }
        else if(xClass < yClass){
            return -1;
        }

        return 0;
    }
    private void SaveData(){
        ProfileManager.Ins.SaveStickmanData(Data);
    }
#endregion

#region EXECUTEDATA
    public void UpgradeStickman(StickmanInfo stickman){
        if(stickman.UpgradeAble()){
            stickman.Upgrade();
        }

        if(m_StickManEquippedDic.ContainsKey(stickman)){
            m_StickManEquippedDic[stickman].UpdateWeaponStat();
        }
        DoUpdateStat();
    }
    public void EquipStickman(StickmanInfo stickman){
        StickmanSlotData slot = GetFreeSlot();
        if(slot == null) return;
        slot.smi = stickman.ID;
        //if(m_EquipDic.TryGetValue(slot.i,out StickmanInfo stickmaninfo)) {
        //    m_EquipDic[slot.i] = stickman;
        //}
        AddStickmanToBattle(stickman, slot);
        SaveData();
    }
    public void RemoveStickman(StickmanInfo stickman){
        if(!StickmanIsEquiped(stickman)) return;
        StickmanSlotData slot = GetSlotDataByStickmanID(stickman.ID);
        if(slot != null){
            slot.smi = -1;
            //if(m_EquipDic.TryGetValue(slot.i, out StickmanInfo stickmaninfo)) {
            //    m_EquipDic[slot.i] = null;
            //}
        }
        RemoveStickmanFromBattle(stickman);
        SaveData();
    }
    public void AddStickmanPiece(List<StickmanConfig> list, int amount){
        for(int i = 0; i < list.Count; i++){
            StickmanConfig config = list[i];
            StickmanInfo stickman = GetStickmanInforByID(config.id);
            AddStickmanPiece(stickman, amount);
        }
        
        SaveData();
    }
    public void AddStickmanPiece(StickmanInfo stickman, int amount){
        DoUpdateStat();
        stickman.AddPiece(amount);
    }
    public bool StickmanIsEquiped(StickmanInfo stickman){
        int equipID = stickman.ID;
        return m_Data.SlotsData.FirstOrDefault(x => x.smi == equipID) != null;
    }
    public bool HasFreeSlot(){
        List<StickmanSlotData> slots = m_Data.SlotsData;
        return slots.FirstOrDefault(x => x.u == 1 && x.smi != -1) != null;
    }
    public void EnhanceAllStickman(){
        m_ItemInfoEnhances.Clear();
        for(int i = 0; i < StickmanInfoList.Count; i++){
            StickmanInfo info = StickmanInfoList[i];
            if(info.UpgradeAble()){
                ItemInfoEnhance itemEnhance = new();
                itemEnhance.itemIcon = info.GetIcon();
                itemEnhance.rarity = info.Rarity;
                itemEnhance.oldLevel = info.Level;
                
                info.UpgradeMaxLevel();
                
                itemEnhance.newLevel = info.Level;
                m_ItemInfoEnhances.Add(itemEnhance);
            }
        }
        if(m_ItemInfoEnhances.Count <= 0) return;
        UIManager.Ins.OpenUI<UIPanelEnhanceAll>().Setup(m_ItemInfoEnhances);
        DoUpdateStat();
        SaveData();
    }

    private void DoUpdateStat()
    {
        PropertiesManager.Ins.UpdateAttack();
    }
#endregion

#region EXECUTE IN BATTLE
    private void AddStickmanToBattle(StickmanInfo stickmanInfo, StickmanSlotData slot){
        var followPos = IngameManager.Ins.PositionManager.GetStickmanGroundTransform(slot.i, stickmanInfo.Platfom);
        Vector3 pos = followPos.position;
        Stickman stickman = PrefabManager.Instance.SpawnStickmanPool(stickmanInfo.PrefabName, pos);
        stickman.FollowTransform = followPos;
        stickman.InitStickman(stickmanInfo);
        stickman.gameObject.SetActive(true);
        if(!IngameManager.Ins.IsEndMatch){
            stickman.Fight();
        }
        m_StickManEquippedDic.Add(stickmanInfo, stickman);
        EventManager.TriggerEvent(ConstantString.EVENT_EQUIPSTICKMAN);
    }
    private void RemoveStickmanFromBattle(StickmanInfo stickmanInfo){
        Stickman stickman = m_StickManEquippedDic[stickmanInfo];
        SimplePool.Despawn(stickman.gameObject);
        m_StickManEquippedDic.Remove(stickmanInfo);
    }
    public void MoveToNextRound(){
        foreach(var stickman in m_StickManEquippedDic.Keys){
            m_StickManEquippedDic[stickman].MoveToNextRound();
        }
    }
    public void WaitState(){
        foreach(var stickman in m_StickManEquippedDic.Keys){
            m_StickManEquippedDic[stickman].WaitState();
        }
    }
    public void Fight(){
        foreach(var stickman in m_StickManEquippedDic.Keys){
            m_StickManEquippedDic[stickman].Fight();
        }
    }
    public void ResetStickman(){
        foreach(var stickman in m_StickManEquippedDic.Keys){
            m_StickManEquippedDic[stickman].Reset();
        }
    }
#endregion

#region GET SET
    public Stickman GetStickmanByInfo(StickmanInfo info){
        return m_StickmanPrefabDic[info.PrefabName];
    }

    public Stickman GetStickmanByRace(Race race)
    {
        if(m_StickManEquippedDic.Count <= 0) return null;
        foreach (StickmanInfo stickman in m_StickManEquippedDic.Keys)
        {
            if (stickman.Race == race)
            {
                return m_StickManEquippedDic[stickman];
            }
        }

        return null;
    }
    private StickmanData GetStickmanDataByID(int id){
        return Data.StickmanDataList.FirstOrDefault(x => x.id == id);
    }
    public StickmanInfo GetStickmanInforByID(int id){
        return StickmanInfoList.FirstOrDefault(x => x.ID == id);
    }
    private StickmanSlotData GetStickmanSlotDataByID(int id){
        return Data.SlotsData.FirstOrDefault(x => x.i == id);
    }
    private StickmanSlotData GetSlotDataByStickmanID(int id)
    {
        return Data.SlotsData.FirstOrDefault(x => x.smi == id);
    }
    private StickmanSlotData GetFreeSlot()
    {
        List<StickmanSlotData> list = m_Data.SlotsData;
        return list.FirstOrDefault(x => x.u == 1 && x.smi == -1);
    }
    public int GetStickManRaceAmount(Race race)
    {
        return m_StickManEquippedDic.Count <= 0 ? 0 : m_StickManEquippedDic.Keys.Count(stickMan => stickMan.Race == race);
    }
    public int GetStickmanClassAmount(Class classType)
    {
        return m_StickManEquippedDic.Keys.Count(stickman => stickman.Class == classType);
    }
    #endregion
    
#region Gatcha
    public StickmanConfig GetRandomStickmanConfigWithRarity(Rarity rarity) 
    {
        List<StickmanConfig> list = new List<StickmanConfig>(m_StickmanGlobalConfig.configList);
        List<StickmanConfig> listRarity = list.FindAll(x => x.rarity == rarity);
        var num = Random.Range(0, listRarity.Count);
        return listRarity[num];
    }
#endregion

#region STAT
    public BigNumber GetAttackOwnerEffect(){
        BigNumber total = 0;
        foreach(var stickMan in StickmanInfoList){
            if(stickMan.IsLock()) continue;
            total += stickMan.GetOwnerEffect();
        }
        Debug.Log($"Total StickMan Attack Owner Effect: {total}");
        return total;
    }
#endregion
#region EDITOR
#if UNITY_EDITOR
    [Button]
    public void ImportCSVData(){
        ImportStatCofigCSV();
        EditorUtility.SetDirty(gameObject);
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
    }
    private void ImportStatCofigCSV(){
        m_StickmanGlobalConfig = StickmanGlobalConfig.Instance;
        EditorUtility.SetDirty(m_StickmanGlobalConfig);
        m_StickmanGlobalConfig.Load(m_TextCSV);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif
    #endregion

    
}
[System.Serializable]
public class StickmanInfo{
    [SerializeField]
    private StickmanData m_Data;

    [SerializeField]
    private StickmanConfig m_Config;

    [SerializeField] private BigNumber m_OwnerEffect;
    public StickmanInfo(StickmanData data, StickmanConfig config){
        m_Data = data;
        m_Config = config;
    }
    public int ID => m_Data.id;
    public string Name => m_Config.name;
    public Rarity Rarity => m_Config.rarity;
    public Class Class => m_Config.classType;
    public Race Race => m_Config.race;
    public Platform Platfom => m_Config.platform;
    public int Level{ get => m_Data.lv; set => m_Data.lv = value; }
    public int Piece{ get => m_Data.p; private set => m_Data.p = value; }
    public string PrefabName => m_Config.prefabName;
    
    public void UpgradeMaxLevel(){
        while(UpgradeAble()){
            Upgrade();
        }
    }
    public void Upgrade(){
        Piece -= GetRequirePieceAmount();
        Level++;
    }
    public bool IsLock(){
        return Piece == 0 && Level == 1;
    }
    public bool UpgradeAble(){
        return Level < 100 && Piece >= GetRequirePieceAmount();
    }
    public bool IsEquiped(){
        return StickManManager.Ins.StickmanIsEquiped(this) && !IsLock();
    }
    public bool EquipAble(){
        return !IsLock() && !IsEquiped();
    }
    public void AddPiece(int amount){
        Piece += amount;
    }
#region GET
    public Sprite GetIcon(){
        return SpriteManager.Ins.GetStickmanSprite(Class, Race);
    }
    //public Sprite GetRaceIcon()
    //{
    //    return SpriteManager.Ins.GetStickmanRaceSprite(Race);
    //}
    public float GetLevelProgress(){
        return (float)Piece / GetRequirePieceAmount();
    }
    public string GetLevelProgressString(){
        return $"{Piece}/{GetRequirePieceAmount()}";
    }
    public int GetRequirePieceAmount(){
        return GameData.Ins.GetLeveupRequiredPiece(Level + 1);
    }
    public BigNumber GetAttackStat(){
        BigNumber baseValue = m_Config.attack;
        BigNumber increaseValue = m_Config.increaseAttack;
        BigNumber value = baseValue + (increaseValue * (Level - 1));
        return value;
    }
    public float GetAttackSpeed(){
        return m_Config.attackSpeed;
    }
    #endregion

#region STAT
    public BigNumber GetOwnerEffect(){
        m_OwnerEffect = FormulaManager.Ins.GetStickmanOwnedEffect(Level, Rarity);
        return m_OwnerEffect;
    }
#endregion
}