using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

public class MineResearchNodeEditor : Node
{
    //private static string m_SavePath = "Assets/_Game/Scripts/TalentTree/{0}/Node/";

    public MineResearchStaticData m_TalentTreeStaticData;
    public MineResearchNode m_TalentNode;

    private Label m_LabelName;
    private Image m_IconImage;
    private EnumField m_EnumField;
    private TextField m_TextNodeNameField;
    private IntegerField m_LevelField;
    private IntegerField m_IndexField;

    private ObjectField m_IconField;

    private List<IntegerField> m_ValueFields;
    private List<TextField> m_DescriptionFields;

    public TalentTreePort m_InputPort { get; private set; }
    public TalentTreePort m_OutputPort { get; private set; }

    public void Initialize(Vector2 position, MineResearchStaticData talentTreeStaticData)
    {
        m_TalentTreeStaticData = talentTreeStaticData;
        m_TalentNode = ScriptableObject.CreateInstance<MineResearchNode>();

        string path = string.Format("Assets/Resources/MineResearchTalentTree/MineResearchNode_[{0}].asset", m_TalentNode.GetInstanceID());
        AssetDatabase.CreateAsset(m_TalentNode, path);
        AssetDatabase.SaveAssets();
        Selection.activeObject = m_TalentNode;


        EditorUtility.SetDirty(m_TalentNode);
        EditorUtility.SetDirty(m_TalentTreeStaticData);
        m_TalentNode.m_Position = position;
        m_TalentTreeStaticData.m_TalentNode.Add(m_TalentNode);

        SetPosition(new Rect(m_TalentNode.m_Position, Vector2.zero));

        mainContainer.AddClasses("ds-node__main-container");
        extensionContainer.AddClasses("ds-node__extension-container");
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedCallback);
        m_TalentNode.m_OnValueChange = OnScripableObjectValueChange;
    }
    public void Load(MineResearchNode talentNode, MineResearchStaticData talentTreeStaticData)
    {
        m_TalentTreeStaticData = talentTreeStaticData;
        m_TalentNode = talentNode;

        SetPosition(new Rect(m_TalentNode.m_Position, Vector2.zero));

        mainContainer.AddClasses("ds-node__main-container");
        extensionContainer.AddClasses("ds-node__extension-container");
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedCallback);

        m_TalentNode.m_OnValueChange = OnScripableObjectValueChange;
    }
    public void Draw()
    {
        /* TITLE CONTAINER */
        m_LabelName = ElementUtility.CreateLabel(string.Format("Node_Level{0}_Index{1}", m_TalentNode.m_NodeLine, m_TalentNode.m_NodeIndex));
        m_LabelName.AddClasses("ds-node__lable");
        titleContainer.Insert(0, m_LabelName);

        /* INPUT CONTAINER */
        m_InputPort = this.CreatePort("Input", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        m_InputPort.OnConnect = OnInputPortConect;
        m_InputPort.OnDisconnect = OnInputPortDisconect;
        inputContainer.Add(m_InputPort);

        /* OUTPUT CONTAINER */
        m_OutputPort = this.CreatePort("Output", Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
        outputContainer.Add(m_OutputPort);

        /* EXTENSION CONTAINER */
        VisualElement customDataContainer = new VisualElement();

        m_IconImage = ElementUtility.CreateImage(m_TalentNode.m_IconImage);
        m_IconImage.AddClasses("ds-node__image");
        mainContainer.Insert(1, m_IconImage);

        m_EnumField = ElementUtility.CreateEnumField(m_TalentNode.GetNodeType(), "Attribute", OnTypeChange);
        mainContainer.Insert(2, m_EnumField);

        m_TextNodeNameField = ElementUtility.CreateTextField(m_TalentNode.m_NodeName, "Node Name", OnNodeNameChange);
        mainContainer.Insert(3, m_TextNodeNameField);

        m_LevelField = ElementUtility.CreateIntegerField(m_TalentNode.m_NodeLine, "Line", OnLevelChange);
        mainContainer.Insert(4, m_LevelField);

        m_IndexField = ElementUtility.CreateIntegerField(m_TalentNode.m_NodeIndex, "Index", OnIndexChange);
        mainContainer.Insert(5, m_IndexField);

        m_IconField = ElementUtility.CreateObjectField<Sprite>(m_TalentNode.m_IconImage, "Icon Image", OnIconImageChange);
        mainContainer.Insert(7, m_IconField);

       




        //Foldout foldout = ElementUtility.CreateFoldout("Talent Level Datas", true);
        //m_ValueFields = new List<IntegerField>();
        //m_DescriptionFields = new List<TextField>();
        //IntegerField integerField;
        //TextField textField;
        //Foldout subFoldout;
        //for (int i = 0; i < m_TalentNode.m_TalentLevelDatas.Count; i++)
        //{
        //    subFoldout = ElementUtility.CreateFoldout(string.Format("Level {0}", i +1 ), true);
        //    integerField = ElementUtility.CreateIntegerField(m_TalentNode.m_TalentLevelDatas[i].m_Value, "Value", OnValueChange);
        //    textField = ElementUtility.CreateTextArea(m_TalentNode.m_TalentLevelDatas[i].m_Description, "Description", OnDescriptionChange);

        //    m_ValueFields.Add(integerField);
        //    m_DescriptionFields.Add(textField);

        //    subFoldout.Add(integerField);
        //    subFoldout.Add(textField);

        //    foldout.Add(subFoldout);
        //}

        //customDataContainer.Add(foldout);
        //mainContainer.Insert(6, customDataContainer);
        RefreshExpandedState();
    }
    private void OnNodeNameChange(ChangeEvent<string> evt)
    {
        EditorUtility.SetDirty(m_TalentNode);
        m_TalentNode.m_NodeName = evt.newValue;
        m_LabelName.text = m_TalentNode.m_NodeName;
    }
    private void OnLevelChange(ChangeEvent<int> evt)
    {
        EditorUtility.SetDirty(m_TalentNode);
        m_TalentNode.m_NodeLine = evt.newValue;
    }
    private void OnIndexChange(ChangeEvent<int> evt)
    {
        EditorUtility.SetDirty(m_TalentNode);
        m_TalentNode.m_NodeIndex = evt.newValue;
    }
    private void OnTypeChange(ChangeEvent<Enum> evt)
    {
        EditorUtility.SetDirty(m_TalentNode);
        m_TalentNode.SetNodeType(evt.newValue);
    }
    private void OnValueChange(ChangeEvent<int> evt)
    {
        EditorUtility.SetDirty(m_TalentNode);
        //m_TalentNode.m_NodeValue = evt.newValue;
    }
    private void OnIconImageChange(ChangeEvent<UnityEngine.Object> evt)
    {
        EditorUtility.SetDirty(m_TalentNode);
        m_TalentNode.m_IconImage = evt.newValue as Sprite;
        m_IconImage.sprite = evt.newValue as Sprite;
    }
    private void OnDescriptionChange(ChangeEvent<string> evt)
    {
        EditorUtility.SetDirty(m_TalentNode);
        //m_TalentNode.m_NodeDescription = evt.newValue;
    }
    private void OnGeometryChangedCallback(GeometryChangedEvent evt)
    {
        EditorUtility.SetDirty(m_TalentNode);
        m_TalentNode.m_Position = GetPosition().position;
    }
    private void OnInputPortConect(Edge edge)
    {
        if (m_TalentNode.m_RequireNode.Contains(((MineResearchNodeEditor)edge.output.node).m_TalentNode)) return;
        m_TalentNode.m_RequireNode.Add(((MineResearchNodeEditor)edge.output.node).m_TalentNode);
    }
    private void OnInputPortDisconect(Edge edge)
    {
        if (!m_TalentNode.m_RequireNode.Contains(((MineResearchNodeEditor)edge.output.node).m_TalentNode)) return;
        m_TalentNode.m_RequireNode.Remove(((MineResearchNodeEditor)edge.output.node).m_TalentNode);
    }
    public void OnScripableObjectValueChange()
    {
        Debug.Log("ScriptableObject Change");
        m_LabelName.text = m_TalentNode.m_NodeName;
        m_IconImage.sprite = m_TalentNode.m_IconImage;
        m_TextNodeNameField.value = m_TalentNode.m_NodeName;
        m_LevelField.value = m_TalentNode.m_NodeLine;
        m_IndexField.value = m_TalentNode.m_NodeIndex;
        m_EnumField.value = m_TalentNode.GetNodeType();
        //m_NodePointRequireField.value = m_TalentNode.m_NodePointRequire;
        //m_TimeResearchField.value = m_TalentNode.m_ResearchTime;
        //m_ValueField.value = m_TalentNode.m_NodeValue;
        m_IconField.value = m_TalentNode.m_IconImage;
        //m_DescriptionField.value = m_TalentNode.m_NodeDescription;
    }
    public void OnDeleteNode()
    {
        Debug.Log("Delete Node");
        EditorUtility.SetDirty(m_TalentTreeStaticData);
        EditorUtility.SetDirty(m_TalentNode);

        m_TalentTreeStaticData.m_TalentNode.Remove(m_TalentNode);
        string pathToDelete = AssetDatabase.GetAssetPath(m_TalentNode);
        AssetDatabase.DeleteAsset(pathToDelete);

    }
    public override void OnSelected()
    {
        base.OnSelected();
        Selection.activeObject = m_TalentNode;
    }
    public void DisconnectAllPort(GraphView graphView)
    {
        DisconnectPort(graphView, inputContainer);
        DisconnectPort(graphView, outputContainer);
    }
    private void DisconnectPort(GraphView graphView, VisualElement container)
    {
        foreach (Port port in container.Children())
        {
            if (port.connected)
            {
                graphView.DeleteElements(port.connections);
            }
        }
    }
}
