using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

public class SkillManager :  SingletonFree<SkillManager>,IAttackOwnerEffect{
    //-------------Data-------------
    [SerializeField] private SkillsData m_SkillsData;
    //-----------Skill Info-----------
    public List<SkillInfo> m_SkillInfoList = new();
    private readonly Dictionary<int, SkillInfo> m_EquippedDic = new();
    //-----------Skill Using List----------
    public List<Skill> m_SkillUsing = new();
    [SerializeField] private List<Skill> m_SkillByArtifact = new();
    //------------Prefab
    public List<Skill> m_SkillsPrefab = new();
    [ShowInInspector] public Dictionary<string, Skill> m_SkillPrefabDic = new();
    //-------------UI
    private UISkillPanel m_SkillPanel;
    //-------------Config
    [SerializeField] private SkillGlobalConfig m_SkillGlobalConfig;
    [FormerlySerializedAs("m_TextCSV")] [SerializeField] private TextAsset m_TextCsv;
    //-------------Property
    private SkillGlobalConfig SkillGlobalConfig{ get => m_SkillGlobalConfig; set => m_SkillGlobalConfig = value; }
    public SkillsData SkillsData{ get => m_SkillsData; private set => m_SkillsData = value; }
    public bool IsAuto{ get; set; }

#region INIT
    private void InitPrefab(){
        m_SkillPrefabDic.Clear();
        for(int i = 0; i < m_SkillsPrefab.Count; i++){
            Skill skill = m_SkillsPrefab[i];
            string skillName = m_SkillsPrefab[i].name;
            //skill.gameObject.SetActive(false);
            m_SkillPrefabDic.Add(skillName, skill);
        }
    }
    public void LoadLineupSkill(){
        List<SkillSlotData> slotDatas = m_SkillsData.SlotsData;
        for(int i = 0; i < slotDatas.Count; i++){
            SkillSlotData slot = slotDatas[i];
            if(slot.si != -1){
                SkillInfo skillInfo = GetSkillInfoByID(slot.si);
                AddSkillToLineUp(skillInfo, slot);
            }
        }
        SetReadyToAttack();
    }
#endregion

#region EXECUTE SKILL
    public void OnSkillRunning()
    {
        foreach (Skill skill in m_SkillUsing)
        {
            skill.Running();
        }
        foreach (Skill skill in m_SkillByArtifact)
        {
            skill.Running();
        }
    }
    public bool ChangeAutoAttack(){
        bool isAuto = IsAutoAttack();
        isAuto = !isAuto;
        m_SkillsData.at = isAuto ? 1 : 0;
        SetAutoSkill();
        return isAuto;
    }
    public virtual void StopToAttacking(){
        for(int i = 0; i < m_SkillUsing.Count; i++){
            m_SkillUsing[i].StopToAttacking();
        }
    }
    public void ContinueToAttack(){
        for(int i = 0; i < m_SkillUsing.Count; i++){
            m_SkillUsing[i].ContinueToAttack();
        }
    }
    public void TriggerAttackSkill(Skill skill){
        GetSkillUsing(skill.ID).StartAttack();
    }
    public void SetReadyToAttack(){
        for(int i = 0; i < m_SkillUsing.Count; i++){
            Skill skill = m_SkillUsing[i];
            skill.WaitingToAttack();
        }
    }

    private void AddSkillToLineUp(SkillInfo skillInfo, SkillSlotData slot){
        Skill skill = Instantiate(GetSkill(skillInfo),transform) ;
        skill.Init(skillInfo,SkillType.NORMAL ,IngameManager.Ins.Hero.MainCharacter);
        skill.SetAuto(IsAutoAttack());
        skill.StartRunning();
        m_SkillUsing.Add(skill);

        int slotID = slot.i;
        UIManager.Ins.GetUI<UISkillPanel>().AddSkillTolineUp(skill, slotID);
    }
    

    private void RemoveSkillFromLineup(SkillInfo skillInfo, SkillSlotData slot){
        Skill skill = GetSkillUsing(skillInfo.SkillID);
        if (!skill) return;
        Debug.Log("Remove Skill ");
        m_SkillUsing.Remove(skill);
        UIManager.Ins.GetUI<UISkillPanel>().RemoveSkill(slot.i, false);
        Destroy(skill.gameObject);
    }
    public bool RemoveSkill(int skillID){
        Skill skill = GetSkillUsing(skillID);
        if(skill != null){
            if(skill.IsInUse()){
                return false;
            } else{
                m_SkillUsing.Remove(skill);
                m_SkillPanel.RemoveSkill(skillID);
                skill.gameObject.SetActive(false);
            }
        }

        return true;
    }
    
    //Artifacts
    public Skill AddSkillArtifact(int id,float reloadTime){
        SkillInfo skillInfo = GetSkillInfoByID(id);
        SkillConfig config = skillInfo.SkillConfig.Clone();
        config.cooldownTime = reloadTime;

        var skillArtifact = new SkillInfo(skillInfo.SkillData,config);
        
        Skill skill = Instantiate(GetSkill(skillArtifact),transform) ;
        skill.Init(skillArtifact,SkillType.ARTIFACT,IngameManager.Ins.Hero.MainCharacter);
        skill.StartRunning();
        m_SkillByArtifact.Add(skill);

        return skill;
    }
    public void RemoveSkillArtifact(Skill skill){
        var s = GetSkillArtifact(skill.ID);
        if(!s) return;
        Destroy(s.gameObject);
        m_SkillByArtifact.Remove(s);
    }
#endregion

#region GET SET
    private void SetAutoSkill()
    {
        foreach (Skill skill in m_SkillUsing)
        {
            skill.SetAuto(IsAutoAttack());
        }
    }
    private Skill GetSkillUsing(int id){
        return m_SkillUsing.FirstOrDefault(sk => sk.ID == id);
    }
    private Skill GetSkillArtifact(int id){
        return m_SkillByArtifact.FirstOrDefault(sk => sk.ID == id);
    }
    private Skill GetSkill(SkillInfo skillInfo){
        Skill skill = m_SkillPrefabDic[skillInfo.PrefabName];
        return skill;
    }
    private SkillData GetSkillDataByID(int id){
        return SkillsData.SkillDataList.FirstOrDefault(x => x.sid == id);
    }

    private SkillSlotData GetSkillSlotDataByID(int id){
        return SkillsData.SlotsData.FirstOrDefault(x => x.i == id);
    }

    private SkillSlotData GetSkillSlotDataSkillID(int id){
        return SkillsData.SlotsData.FirstOrDefault(x => x.si == id);
    }
    public bool SkillIsEquipped(int id){
        return SkillsData.SlotsData.FirstOrDefault(x => x.si == id) != null;
    }
    public SkillInfo GetSkillInfoByID(int id){
        return m_SkillInfoList.FirstOrDefault(x => x.SkillID == id);
    }

    private SkillSlotData GetFreeSlot(){
        List<SkillSlotData> list = SkillsData.SlotsData;
        return list.FirstOrDefault(x => x.u == 1 && x.si == -1);
    }
    public bool IsAutoAttack(){
        return m_SkillsData.at == 1;
    }
#endregion

#region ExecuteData
    public void UpgradeSkill(SkillInfo skill){
        if(skill.UpgradeAble()){
            skill.Upgrade();
        }

        DoUpdateStat();
    }
    public void EquipSkill(SkillInfo skill){
        SkillSlotData slot = GetFreeSlot();
        if(slot == null) return;
        slot.si = skill.SkillID;
        if(m_EquippedDic.TryGetValue(slot.i, out SkillInfo skillinfo)){
            m_EquippedDic[slot.i] = skill;
        }

        AddSkillToLineUp(skill, slot);
        SaveData();
    }
    public void RemoveSkill(SkillInfo skill){
        if(!skill.IsEquipped()) return;
        SkillSlotData slot = GetSkillSlotDataSkillID(skill.SkillID);
        if(slot != null){
            slot.si = -1;
            if(m_EquippedDic.TryGetValue(slot.i, out SkillInfo skillinfo)){
                m_EquippedDic[slot.i] = null;
            }

            RemoveSkillFromLineup(skill, slot);
        }
    }
    public bool HasSlotFree(){
        List<SkillSlotData> list = SkillsData.SlotsData;
        return list.FirstOrDefault(x => x.u == 1 && x.si != -1) != null;
    }
    
    public void AddSkillPiece(List<SkillConfig> list){
        for(int i = 0; i < list.Count; i++){
            SkillConfig config = list[i];
            SkillInfo skill = GetSkillInfoByID(config.id);
            AddSkillPiece(skill, 1);
        }
        SaveData();
    }
    private void AddSkillPiece(SkillInfo skill, int amount){
        skill.AddPiece(amount);
        DoUpdateStat();
    }
    
    public void EnhanceAllSkill(){
        List<ItemInfoEnhance> enhanceList = new();
        for(int i = 0; i < m_SkillInfoList.Count; i++){
            SkillInfo skill = m_SkillInfoList[i];
            if(skill.UpgradeAble()){
                ItemInfoEnhance itemEnhance = new();
                itemEnhance.itemIcon = skill.GetIcon();
                itemEnhance.rarity = skill.Rarity;
                itemEnhance.oldLevel = skill.Level;
                
                skill.UpgradeMaxLevel();
                
                itemEnhance.newLevel = skill.Level;
                enhanceList.Add(itemEnhance);
            }
        }
        if(enhanceList.Count <= 0) return;
        UIManager.Ins.OpenUI<UIPanelEnhanceAll>().Setup(enhanceList);
        DoUpdateStat();
    }
#endregion

#region SaveLoadData
    public void LoadData(SkillsData data){
        SkillsData = data;
        if(SkillsData == null || SkillsData.SkillDataList.Count <= 0){
            CreateNewData();
        } else{
            LoadLocalData();
        }
        LoadEquipDic();
        InitPrefab();
    }
    public void CreateNewData(){
        SkillsData = new();
        List<SkillConfig> list = m_SkillGlobalConfig.configList;
        for(int i = 0; i < list.Count; i++){
            SkillConfig config = list[i];
            SkillData data = new(config.id);
            SkillsData.SkillDataList.Add(data);
            SkillInfo skf = new(data, config);
            m_SkillInfoList.Add(skf);
        }

        for(int i = 1; i <= 6; i++){
            SkillSlotData slotData = new(i);
            SkillsData.SlotsData.Add(slotData);
        }

        SaveData();
    }
    public void LoadLocalData(){
        List<SkillConfig> list = m_SkillGlobalConfig.configList;
        for(int i = 0; i < list.Count; i++){
            SkillConfig config = list[i];

            int skillID = config.id;
            SkillData data = GetSkillDataByID(skillID);
            if(data == null){
                data = new(config.id);
                SkillsData.SkillDataList.Add(data);
            }

            SkillInfo skf = new(data, config);
            m_SkillInfoList.Add(skf);
        }

        for(int i = 1; i <= 6; i++){
            int id = i;
            SkillSlotData slotData = GetSkillSlotDataByID(id);
            if(slotData == null){
                slotData = new(id);
                SkillsData.SlotsData.Add(slotData);
            }
        }
    }
    public void SaveData(){
        ProfileManager.Ins.SaveSkillData(SkillsData);
    }
    public void LoadEquipDic(){
        m_EquippedDic.Clear();
        for(int i = 0; i < SkillsData.SlotsData.Count; i++){
            SkillSlotData slot = SkillsData.SlotsData[i];
            SkillInfo ski = GetSkillInfoByID(slot.si);
            m_EquippedDic.Add(slot.i, ski);
        }
    }
#endregion

#region STAT
    public BigNumber GetSkillOwnedEffect(){
        BigNumber totalOwnedEffect = 0;
        for(int i = 0; i < m_SkillInfoList.Count; i++){
            SkillInfo skill = m_SkillInfoList[i];
            if(skill.IsUnlock()){
                totalOwnedEffect += skill.GetOwnerEffect();
            }
        }
        Debug.Log(totalOwnedEffect);
        return totalOwnedEffect;
    }
#endregion
#region Gatcha
    public SkillConfig GetRandomSkillConfigWithRarity(Rarity rarity){
        List<SkillConfig> list = m_SkillGlobalConfig.configList.Where(x => x.rarity == rarity).ToList();
        return list[Random.Range(0, list.Count)];
    }
#endregion
#region  STAT
public BigNumber GetAttackOwnerEffect()
{
    BigNumber total = 0;
    for (int i = 0; i < m_SkillInfoList.Count; i++)
    {
        SkillInfo skill = m_SkillInfoList[i];
        if (skill.IsUnlock())
        {
            total += skill.GetAttackOwnerEffect();
        }
    }
    Debug.Log($"Total Skill Attack OwnedEffect: {total}");
    return total;
}
private void DoUpdateStat()
{
    PropertiesManager.Ins.UpdateAttack();
}

#endregion
#region EDITOR
#if UNITY_EDITOR
    [Button]
    public void ImportCsvData(){
        ImportStatSkillCsv();
        EditorUtility.SetDirty(gameObject);
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
    }
    private void ImportStatSkillCsv(){
        SkillGlobalConfig = SkillGlobalConfig.Instance;
        EditorUtility.SetDirty(SkillGlobalConfig);
        SkillGlobalConfig.Load(m_TextCsv);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    [Button]
    public void CheatUnlockSkillSlot()
    {
        foreach (SkillSlotData slot in m_SkillsData.SlotsData.Where(slot => slot.u == 0))
        {
            slot.u = 1;
            UIManager.Ins.GetUI<UISkillPanel>().UpdateUIskillSlot(slot.i);
            return;
        }
    }
#endif
#endregion
    
}
[System.Serializable]
public class SkillInfo{
    [BoxGroup("Stat")]
    [SerializeField] private BigNumber m_OwnerEffect;
    
    [SerializeField] private SkillData m_SkillData;
    [SerializeField] private SkillConfig m_SkillConfig;
    public SkillData SkillData => m_SkillData;
    public SkillConfig SkillConfig => m_SkillConfig;
    public int SkillID => m_SkillConfig.id;
    public Rarity Rarity => m_SkillConfig.rarity;
    public int Level{ get => m_SkillData.lv; set => m_SkillData.lv = value; }
    public int Piece{ get => m_SkillData.p; private set => m_SkillData.p = value; }
    public string Name => m_SkillConfig.name;
    public string PrefabName => m_SkillConfig.prefabName;
    public float CoolDownTime{ get => m_SkillConfig.cooldownTime; set => m_SkillConfig.cooldownTime = value; }
    public float DmgRate => m_SkillConfig.dmgRate;
    public SkillInfo(){ }
    public SkillInfo(SkillData data, SkillConfig config){
        m_SkillData = data;
        m_SkillConfig = config;
    }
    public void Upgrade(){
        Piece -= GetRequirePieceAmount();
        Level++;
    }
    public void UpgradeMaxLevel(){
        while(UpgradeAble()){
            Upgrade();
        }
    }
    public bool IsLock(){
        return Piece == 0 && Level == 1;
    }
    public bool IsUnlock(){
        return !IsLock();
    }
    public float GetLevelProgress(){
        return (float)Piece / GetRequirePieceAmount();
    }
    public string GetLevelProgressString(){
        return $"{Piece}/{GetRequirePieceAmount()}";
    }
    public bool UpgradeAble(){
        return Level < 100 && Piece >= GetRequirePieceAmount();
    }
    public void AddPiece(int amount){
        Piece += amount;
    }
    public int GetRequirePieceAmount(){
        return GameData.Ins.GetLeveupRequiredPiece(Level + 1);
    }
    public bool IsEquipped(){
        return SkillManager.Ins.SkillIsEquipped(SkillID) && !IsLock();
    }
    public bool EquipAble(){
        return !IsLock() && !IsEquipped();
    }
    public Sprite GetIcon()
    {
        return SpriteManager.Ins.GetSkillIconSprite(PrefabName);
    }
    
#region STAT
    public BigNumber GetOwnerEffect(){
        m_OwnerEffect = FormulaManager.Ins.GetSkillOwnedEffect(Level, Rarity);
        return m_OwnerEffect;
    }
    public BigNumber GetAttackOwnerEffect(){
        return FormulaManager.Ins.GetSkillOwnedEffect(Level, Rarity);
    }

    public float GetDpsRate()
    {
        float dpsRate = m_SkillConfig.dpsRate;
        float dpsIncrease = m_SkillConfig.dpsIncrease;
        dpsRate += (Level-1) * dpsIncrease;
        return dpsRate;
    }
#endregion

    public SkillInfo Clone()
    {
        return (SkillInfo)this.MemberwiseClone();
    }
}