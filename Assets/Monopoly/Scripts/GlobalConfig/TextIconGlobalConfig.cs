using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.Utilities;

[CreateAssetMenu(fileName = "IconTextGlobalConfig", menuName = "GlobalConfigs/IconTextGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class TextIconGlobalConfig : GlobalConfig<TextIconGlobalConfig>
{
    [field: SerializeField] public TextIconConfig[] TextIconConfigs {get; private set;}
    private Dictionary<string, TextIconConfig> TextIconConfigDictionary { get; set; } = new Dictionary<string, TextIconConfig>();

    public TextIconConfig GetTextIconConfig(string iconTextType)
    {
        TextIconConfigDictionary.TryAdd(iconTextType,
            TextIconConfigs.FirstOrDefault(x => x.IconTextType == iconTextType));
        if (TextIconConfigDictionary[iconTextType] == null)
        {
            Debug.Log("Missing IconTextConfig: " + iconTextType);
            return new TextIconConfig();
        }
        return TextIconConfigDictionary[iconTextType];
    }
}
[System.Serializable]
public class TextIconConfig
{
    [field: SerializeField] public string IconTextType {get; set;} = "";
    [field: SerializeField] public string Text {get; set;} = "";
}

public static class IconText
{
    public static string Get(string iconTextType)
    {
        return TextIconGlobalConfig.Instance.GetTextIconConfig(iconTextType).Text;
    }
}