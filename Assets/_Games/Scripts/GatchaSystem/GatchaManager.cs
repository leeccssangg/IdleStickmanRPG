using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif


public enum GatchaType { Stickman, Skill, Gear , NONE = -1}

public class GatchaManager : SingletonFree<GatchaManager>
{
    [SerializeField] private GatchaData m_GatchaData;
    public StickmanGatchaManager m_StickmanGatchaManager = new();
    public SkillGatchaManager m_SkillGatchaManager = new();
    public GearGatchaManager m_GearGatchaManager = new();


    #region Save & Load
    public void LoadData(GatchaData data)
    {
        m_StickmanGatchaManager = new();
        m_SkillGatchaManager = new();
        m_GearGatchaManager = new();
        m_GatchaData = data;
        m_StickmanGatchaManager.LoadData(m_GatchaData.StickmanGatchaD);
        m_SkillGatchaManager.LoadData(m_GatchaData.SkilGatchaD); 
        m_GearGatchaManager.LoadData(m_GatchaData.GearGatchaD);
    }
    private void SaveData()
    {
        m_GatchaData = new()
        {
            StickmanGatchaD = m_StickmanGatchaManager.SaveData(),
            SkilGatchaD = m_SkillGatchaManager.SaveData(),
            GearGatchaD = m_GearGatchaManager.SaveData(),
        };
        ProfileManager.Ins.SaveGatchaData(m_GatchaData);
    }
    #endregion
    #region Manager functions
    #region Gatcha functions
    public List<GatchaConfigInfo> GoGatcha(GatchaType gatchaType, int numOpen, GatchaButtonType gatchaButtonType)
    {
        List<GatchaConfigInfo> outList = new();
        switch (gatchaType)
        {
            case GatchaType.Stickman:
                outList = GoGatchaStickman(numOpen, gatchaButtonType);
                break;
            case GatchaType.Skill:
                outList = GoGatchaSkill(numOpen, gatchaButtonType);
                break;
            case GatchaType.Gear:
                outList = GoGatchaGear(numOpen, gatchaButtonType);
                break;
        }
        SaveData();
        return outList;
    }
    private List<GatchaConfigInfo> GoGatchaStickman(int numOpen,GatchaButtonType btnType)
    {
        List<StickmanConfig> list = m_StickmanGatchaManager.GoGatcha(numOpen, btnType);
        List<GatchaConfigInfo> outList = new();
        list.ForEach(x => outList.Add(new GatchaConfigInfo(x, 1)));
        return outList;
    }
    private List<GatchaConfigInfo> GoGatchaSkill(int numOpen, GatchaButtonType btnType)
    {
        List<SkillConfig> list = m_SkillGatchaManager.GoGatcha(numOpen, btnType);
        List<GatchaConfigInfo> outList = new();
        list.ForEach(x => outList.Add(new GatchaConfigInfo(x, 1)));
        return outList;
    }
    private List<GatchaConfigInfo> GoGatchaGear(int numOpen, GatchaButtonType btnType)
    {
        List<GearConfig> list = m_GearGatchaManager.GoGatcha(numOpen, btnType);
        List<GatchaConfigInfo> outList = new();
        list.ForEach(x => outList.Add(new GatchaConfigInfo(x, 1)));
        return outList;
    }
    #endregion
    #region Get data by Type
    public int GetGatchaLevelByType(GatchaType gatchaType)
    {
        int tmp = 0;
        switch(gatchaType)
        {
            case GatchaType.Stickman:
                tmp = m_StickmanGatchaManager.GetGatchaLevel();
                break;
            case GatchaType.Skill:
                tmp = m_SkillGatchaManager.GetGatchaLevel();
                break;
            case GatchaType.Gear:
                tmp = m_GearGatchaManager.GetGatchaLevel();
                break;
        }
        return tmp;
    }
    public int GetCurrentPointByType(GatchaType gatchaType)
    {
        int tmp = 0;
        switch (gatchaType)
        {
            case GatchaType.Stickman:
                tmp = m_StickmanGatchaManager.GetCurrentPoint();
                break;
            case GatchaType.Skill:
                tmp = m_SkillGatchaManager.GetCurrentPoint();
                break;
            case GatchaType.Gear:
                tmp = m_GearGatchaManager.GetCurrentPoint();
                break;
        }
        return tmp;
    }
    public float GetCurrentProcessByType(GatchaType gatchaType)
    {
        float tmp = 0;
        switch (gatchaType)
        {
            case GatchaType.Stickman:
                tmp = m_StickmanGatchaManager.GetProcessUpgraded();
                break;
            case GatchaType.Skill:
                tmp = m_SkillGatchaManager.GetProcessUpgraded();
                break;
            case GatchaType.Gear:
                tmp = m_GearGatchaManager.GetProcessUpgraded();
                break;
        }
        return tmp;
    }
    public int GetNumOpenBox1ByType(GatchaType gatchaType)
    {
        int tmp = 0;
        switch (gatchaType)
        {
            case GatchaType.Stickman:
                tmp = m_StickmanGatchaManager.GetNumOpenBox1();
                break;
            case GatchaType.Skill:
                tmp = m_SkillGatchaManager.GetNumOpenBox1();
                break;
            case GatchaType.Gear:
                tmp = m_GearGatchaManager.GetNumOpenBox1();
                break;
        }
        return tmp;
    }
    public int GetNumOpenBox2ByType(GatchaType gatchaType)
    {
        int tmp = 0;
        switch (gatchaType)
        {
            case GatchaType.Stickman:
                tmp = m_StickmanGatchaManager.GetNumOpenBox2();
                break;
            case GatchaType.Skill:
                tmp = m_SkillGatchaManager.GetNumOpenBox2();
                break;
            case GatchaType.Gear:
                tmp = m_GearGatchaManager.GetNumOpenBox2();
                break;
        }
        return tmp;
    }
    public int GetAdsLeftByType(GatchaType gatchaType)
    {
        int tmp = 0;
        switch (gatchaType)
        {
            case GatchaType.Stickman:
                tmp = m_StickmanGatchaManager.GetAdsLeft();
                break;
            case GatchaType.Skill:
                tmp = m_SkillGatchaManager.GetAdsLeft();
                break;
            case GatchaType.Gear:
                tmp = m_GearGatchaManager.GetAdsLeft();
                break;
        }
        return tmp;
    }
    public int GetMaxAdsByType(GatchaType gatchaType)
    {
        int tmp = 0;
        switch (gatchaType)
        {
            case GatchaType.Stickman:
                tmp = m_StickmanGatchaManager.GetMaxAds();
                break;
            case GatchaType.Skill:
                tmp = m_SkillGatchaManager.GetMaxAds();
                break;
            case GatchaType.Gear:
                tmp = m_GearGatchaManager.GetMaxAds();
                break;
        }
        return tmp;
    }
    public string GetTimeToNextFreeByType(GatchaType gatchaType)
    {
        string tmp = "";
        switch (gatchaType)
        {
            case GatchaType.Stickman:
                tmp = m_StickmanGatchaManager.GetTimeToNextFree();
                break;
            case GatchaType.Skill:
                tmp = m_SkillGatchaManager.GetTimeToNextFree();
                break;
            case GatchaType.Gear:
                tmp = m_GearGatchaManager.GetTimeToNextFree();
                break;
        }
        return tmp;
    }
    public bool IsFreeByType(GatchaType gatchaType)
    {
        bool tmp = false;
        switch (gatchaType)
        {
            case GatchaType.Stickman:
                tmp = m_StickmanGatchaManager.IsFree();
                break;
            case GatchaType.Skill:
                tmp = m_SkillGatchaManager.IsFree();
                break;
            case GatchaType.Gear:
                tmp = m_GearGatchaManager.IsFree();
                break;
        }
        return tmp;
    }
    public int GetCurrentUpgradeNeededByType(GatchaType gatchaType)
    {
        int tmp = 0;
        switch (gatchaType)
        {
            case GatchaType.Stickman:
                tmp = m_StickmanGatchaManager.GetCurrentUpgradeNeed();
                break;
            case GatchaType.Skill:
                tmp = m_SkillGatchaManager.GetCurrentUpgradeNeed();
                break;
            case GatchaType.Gear:
                tmp = m_GearGatchaManager.GetCurrentUpgradeNeed();
                break;
        }
        return tmp;
    }
    #endregion
    #endregion
    #region Editor
#if UNITY_EDITOR
    public TextAsset m_TextCSVStickMan;
    public TextAsset m_TextCSVSkill;
    public TextAsset m_TextCSVGear;
    [Button]
    public void ImportCSVData()
    {
        ImportCofigCSV();
        EditorUtility.SetDirty(gameObject);
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
    }
    private void ImportCofigCSV()
    {
        StickmanGatchaGlobalConfig m_StickmanGatchaConfig = StickmanGatchaGlobalConfig.Instance;
        SkillGatchaGlobalConfig m_SkillGatchaConfig = SkillGatchaGlobalConfig.Instance;
        GearGatchaGlobalConfig m_GearConfig = GearGatchaGlobalConfig.Instance;
        EditorUtility.SetDirty(m_StickmanGatchaConfig);
        EditorUtility.SetDirty(m_SkillGatchaConfig);
        EditorUtility.SetDirty(m_GearConfig);
        m_StickmanGatchaConfig.LoadTextAsset(m_TextCSVStickMan);
        m_SkillGatchaConfig.LoadTextAsset(m_TextCSVSkill);
        m_GearConfig.LoadTextAsset(m_TextCSVGear);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif
    #endregion
}
[System.Serializable]
public class GatchaData
{
    public StickmanGatchaData sgd;
    public SkillGatchaData skgd;
    public GearGatchaData ggd;

    public StickmanGatchaData StickmanGatchaD { get => sgd; set => sgd = value; }
    public SkillGatchaData SkilGatchaD { get=> skgd; set=> skgd = value;}
    public GearGatchaData GearGatchaD { get => ggd; set => ggd = value; }

    public GatchaData()
    {
        sgd = new();
        skgd = new();
        ggd = new();
    }
}
[System.Serializable]
public class GatchaConfigInfo
{
    public GatchaType gatchaType;
    public Rarity rarity;
    public int id;
    public int amount;
    public GearType gearType = GearType.NONE;

    public GatchaConfigInfo(StickmanConfig stickmanConfig,int amount)
    {
        gatchaType = GatchaType.Stickman;
        rarity = stickmanConfig.rarity;
        id = stickmanConfig.id;
        this.amount = amount;
    }
    public GatchaConfigInfo(SkillConfig skillConfig, int amount)
    {
        gatchaType = GatchaType.Skill;
        rarity = skillConfig.rarity;
        id = skillConfig.id;
        this.amount = amount;
    }
    public GatchaConfigInfo(GearConfig gearConfig,int amount)
    {
        gatchaType = GatchaType.Gear;
        rarity = gearConfig.rarity;
        id = gearConfig.id;
        this.amount = amount;
        gearType = gearConfig.gearType;
    }
}
