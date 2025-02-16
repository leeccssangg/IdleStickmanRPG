#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;


public class ChangeScene : Editor {

    [MenuItem("Open Scene/FirstScene #1")]
    public static void OpenLoading()
    {
        OpenScene("FirstScene");
    }

    [MenuItem("Open Scene/Shop #2")]
    public static void OpenHome()
    {
        OpenScene("Coffee Shop");
    }
    
    [MenuItem("Open Scene/Game #3")]
    public static void OpenGame()
    {
        OpenScene("Main Scene");
    }
    [MenuItem("Open Scene/_Level 1 #4")]
    public static void OpenLevel_1() {
        OpenScene("Level_1");
    }
    [MenuItem("Open Scene/_Level 2 #5")]
    public static void OpenLevel_2() {
        OpenScene("Level_2");
    }

    private static void OpenScene (string sceneName) {
		if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo ()) {
			EditorSceneManager.OpenScene ("Assets/_Game/Scenes/" + sceneName + ".unity");
		}
	}
}
#endif