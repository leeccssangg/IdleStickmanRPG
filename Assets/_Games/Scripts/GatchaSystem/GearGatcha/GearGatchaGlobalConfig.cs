using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities;

[CreateAssetMenu(fileName = "GearGatchaGlobalConfig", menuName = "GlobalConfigs/GearGatchaGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class GearGatchaGlobalConfig : GlobalConfig<GearGatchaGlobalConfig>
{
    public int numBox1;
    public int numBox2;
    public List<GearGatchaLevelConfig> levelConfig = new();

    public void LoadTextAsset(TextAsset textAsset)
    {
        levelConfig.Clear();
        List<Dictionary<string, string>> list = CSVReader.ReadDataFromString(textAsset.text);
        for (int i = 0; i < list.Count; i++)
        {
            Dictionary<string, string> dic = list[i];
            GearGatchaLevelConfig config = new();
            config.Load(list[i]);
            levelConfig.Add(config);
        }
    }
}
