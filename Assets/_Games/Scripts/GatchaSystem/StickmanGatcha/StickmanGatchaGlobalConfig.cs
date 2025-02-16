using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities;

[CreateAssetMenu(fileName = "StickmanGatchaGlobalConfig", menuName = "GlobalConfigs/StickmanGatchaGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class StickmanGatchaGlobalConfig : GlobalConfig<StickmanGatchaGlobalConfig>
{
    public int numBox1;
    public int numBox2;
    public List<StickmanGatchaLevelConfig> levelConfig = new();

    public void LoadTextAsset(TextAsset textAsset)
    {
        levelConfig.Clear();
        List<Dictionary<string, string>> list = CSVReader.ReadDataFromString(textAsset.text);
        for(int i = 0; i < list.Count; i++)
        {
            Dictionary<string, string> dic = list[i];
            StickmanGatchaLevelConfig config = new();
            config.Load(list[i]);
            levelConfig.Add(config);
        }
    }
}
