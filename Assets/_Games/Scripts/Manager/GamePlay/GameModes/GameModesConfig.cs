using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu(fileName = "GameModeConfig",menuName = "GlobalConfigs/GameModeConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class GameModesConfig : GlobalConfig<GameModesConfig>{
    public GameModeConfig[] gameModeConfigs;
    
    public GameModeConfig GetGameModeConfig(GameMode gameMode) {
        return gameModeConfigs.FirstOrDefault(x => x.gameMode == gameMode);
    }
#if UNITY_EDITOR
    [Button]
    public void Save(){
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif
}
[System.Serializable]
public class GameModeConfig {
    public GameMode gameMode;
    public float timePlay;
    public int wave;
}