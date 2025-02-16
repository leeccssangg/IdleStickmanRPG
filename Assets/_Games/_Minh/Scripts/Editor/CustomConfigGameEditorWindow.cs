#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CustomConfigGameEditorWindow : OdinMenuEditorWindow {

    [MenuItem("Tools/Editor Windows/Game Config Editor Window")]
    private static void Open() {
        CustomConfigGameEditorWindow window = GetWindow<CustomConfigGameEditorWindow>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 500);
    }
    protected override OdinMenuTree BuildMenuTree() {
        OdinMenuTree tree = new OdinMenuTree(true);

        tree.DefaultMenuStyle.IconSize = 28.00f;
        tree.Config.DrawSearchToolbar = true;
        tree.Add("Global Config/Stat Config", StatCostConfig.Instance);
        tree.Add("Global Config/Stat Config/Player Stats", StatCostConfig.Instance.playerStat);
        tree.Add("Global Config/Stat Config/Player Stats Cost", StatCostConfig.Instance.playerStatCostConfig);
        tree.Add("Global Config/Stat Config/Owned Stats", StatCostConfig.Instance.ownedConfig);
        tree.Add("Global Config/Stat Config/Campaign Stats", StatCostConfig.Instance.campaignStatConfig);

        tree.Add("Global Config/Stat Config/Dungeon/BossRush", StatCostConfig.Instance.dungeonBossRush);
        tree.Add("Global Config/Stat Config/Dungeon/GoldRush", StatCostConfig.Instance.dungeonGoldRush);
        tree.Add("Global Config/Stat Config/Dungeon/DeadHunter", StatCostConfig.Instance.dungeonDeadHunter);
        tree.Add("Global Config/Stat Config/Dungeon/MassiveShadow", StatCostConfig.Instance.dungeonMassiveShadow);
        tree.Add("Global Config/Stat Config/Dungeon/Test", StatCostConfig.Instance.testStatConfig);

        if (Application.isPlaying) return tree;
        return tree;
    }
    protected override void OnBeginDrawEditors() {
        //base.OnBeginDrawEditors();
        var selected = this.MenuTree.Selection.FirstOrDefault();
        var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;
        SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Save Data"))) {
                EditorUtility.SetDirty(StatCostConfig.Instance);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }
} 
#endif