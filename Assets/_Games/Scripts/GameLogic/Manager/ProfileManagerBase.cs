using UnityEngine;

public class ProfileManagerBase<T> : Pextension.Singleton<T> where T : ProfileManagerBase<T>
{
    [SerializeField] private PlayerData m_PlayerData = new();
    public static PlayerData PlayerData => Ins.m_PlayerData;
    public DataSavingMachine m_DataSavingMachine = new();
    public void Awake() {
        Load();
    }
    protected virtual void Start()
    {
        m_DataSavingMachine.m_OnSaveEvent.AddListener(SaveData);
    }
    protected virtual void Update()
    {
        m_DataSavingMachine.Update(Time.deltaTime);
    }
    public virtual void Load()
    {
        string jsonData = DataGlobal.Ins.GetData<string>(DataKey.PlayerData, "");
        if (string.IsNullOrEmpty(jsonData))
        {
            m_PlayerData = new PlayerData();
            m_PlayerData.CreateNewData();
        }
        else
        {
            m_PlayerData = Static.FromJsonString<PlayerData>(jsonData);
            PlayerData.LoadData();
        }
    }
    public virtual void Save()
    {
        m_DataSavingMachine.Save();
    }
    public virtual void ForceSave()
    {
        m_DataSavingMachine.ForceSave();
    }
    protected virtual void SaveData()
    {
        string playerJsonData = m_PlayerData.ToJsonString();
        //Debug.Log(playerJsonData);
        DataGlobal.Ins.SetData<string>(DataKey.PlayerData, playerJsonData);
        //....Save Here
    }
}
