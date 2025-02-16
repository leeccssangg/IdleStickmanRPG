using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities;

[CreateAssetMenu(fileName = "SkillGatchaGlobalConfig", menuName = "GlobalConfigs/SkillGatchaGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class SkillGatchaGlobalConfig : GlobalConfig<SkillGatchaGlobalConfig>
{
    public int numBox1;
    public int numBox2;
    public List<SkillGatchaLevelConfig> levelConfig = new();

    public void LoadTextAsset(TextAsset textAsset)
    {
        levelConfig.Clear();
        List<Dictionary<string, string>> list = CSVReader.ReadDataFromString(textAsset.text);
        for (int i = 0; i < list.Count; i++)
        {
            Dictionary<string, string> dic = list[i];
            SkillGatchaLevelConfig config = new();
            config.Load(list[i]);
            levelConfig.Add(config);
        }
    }
}
