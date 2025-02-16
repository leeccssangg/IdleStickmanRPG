using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class ResultGameEditorWindow : OdinEditorWindow {
    private object someObject;

    [TextArea(10, 30)]
    public string result;

    [MenuItem("My Game/Some Window")]
    private static void OpenWindow() {
        GetWindow<ResultGameEditorWindow>().Show();
    }
    private void InspectObjectInWindow() {
        OdinEditorWindow.InspectObject(someObject);
    }

    private void InspectObjectInDropDownWithAutoHeight() {
        var btnRect = GUIHelper.GetCurrentLayoutRect();
        OdinEditorWindow.InspectObjectInDropDown(someObject, btnRect, btnRect.width);
    }

    private void InspectObjectInDropDown() {
        var btnRect = GUIHelper.GetCurrentLayoutRect();
        OdinEditorWindow.InspectObjectInDropDown(someObject, btnRect, new Vector2(btnRect.width, 100));
    }

    private void InspectObjectInACenteredWindow() {
        var window = OdinEditorWindow.InspectObject(someObject);
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(270, 200);
    }

    private void OtherStuffYouCanDo() {
        var window = OdinEditorWindow.InspectObject(this.someObject);

        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(270, 200);
        window.titleContent = new GUIContent("Custom title", EditorIcons.RulerRect.Active);
        window.OnClose += () => Debug.Log("Window Closed");
        window.OnBeginGUI += () => GUILayout.Label("-----------");
        window.OnEndGUI += () => GUILayout.Label("-----------");
    }
}
