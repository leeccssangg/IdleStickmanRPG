public class DataGlobal :Pextension.Singleton<DataGlobal>
{
    private IDataAccess m_DataAccess;
    public IDataAccess DataAccess { get => m_DataAccess ??= new ObscuredPrefsDataAccess(); }

    public T GetData<T>(string dataKey, T defaultValue = default)
    {
        return DataAccess.GetData<T>(dataKey, defaultValue);
    }
    public void SetData<T>(string dataKey, T value)
    {
        DataAccess.SetData<T>(dataKey, value);
    }
    public T GetData<T>(DataKey dataKey, T defaultValue = default)
    {
        return DataAccess.GetData<T>(dataKey.ToString(), defaultValue);
    }
    public void SetData<T>(DataKey dataKey, T value)
    {
        DataAccess.SetData<T>(dataKey.ToString(), value);
    }
}
