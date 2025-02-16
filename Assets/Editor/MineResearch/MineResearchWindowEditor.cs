using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

public class MineResearchWindowEditor : EditorWindow
{ 
    private MineResearchGraphView m_TalentTreeGraphView;
    [MenuItem("Tools/Mine Research Window Editor")]
    public static void Open()
    {
        GetWindow<MineResearchWindowEditor>();
    }
    private void OnEnable()
    {
        AddGraphView();
        AddToolBar();
        AddStyles();
    }
    public void AddGraphView()
    {
        m_TalentTreeGraphView = new MineResearchGraphView(this);
        m_TalentTreeGraphView.StretchToParentSize();
        rootVisualElement.Add(m_TalentTreeGraphView);
    }
    public void AddStyles()
    {
        rootVisualElement.AddStyleSheets("TalentTree/Variables.uss");
    }
    public void AddToolBar()
    {
        Toolbar toolbar = new Toolbar();
        Button loadPlayerStatsTalentTreeButton = ElementUtility.CreateButton("Load PlayerStats Talent Tree", LoadPlayerStatsTalentTree);
        //Button loadUtilityTalentTreeButton = ElementUtility.CreateButton("Load Utility Talent Tree", LoadUtilityTalentTree);
        toolbar.Add(loadPlayerStatsTalentTreeButton);
        //toolbar.Add(loadUtilityTalentTreeButton);
        rootVisualElement.Add(toolbar);
    }
    public void LoadPlayerStatsTalentTree()
    {
        m_TalentTreeGraphView.ClearGraph();
        m_TalentTreeGraphView.ChangeTalentTreeType();

    }
    //public void LoadUtilityTalentTree()
    //{
    //    if (m_TalentTreeType == TalentTreeType.Utility) return;
    //    m_TalentTreeType = TalentTreeType.Utility;
    //    m_TalentTreeGraphView.ClearGraph();
    //    m_TalentTreeGraphView.ChangeTalentTreeType(TalentTreeType.Utility);
    //}
}
public class MineResearchGraphView : GraphView
{
    private TalentTreeType m_TalentTreeType;
    private MineResearchStaticData m_TalentTreeStaticData;
    private MineResearchWindowEditor m_TalentTreeWindowEditor;

    public MineResearchGraphView(MineResearchWindowEditor talentTreeWindowEditor)
    {
        m_TalentTreeWindowEditor = talentTreeWindowEditor;

        AddManipulators();
        AddGridBackground();

        AddStyles();

        LoadTalentNode();
        OnElementDeleted();
    }
    public void AddManipulators()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        this.AddManipulator(CreateTalentNodeContextualMenu());
    }
    public void AddGridBackground()
    {
        GridBackground gridBackground = new GridBackground();
        gridBackground.StretchToParentSize();
        Insert(0, gridBackground);
    }
    public void AddStyles()
    {
        this.AddStyleSheets("TalentTree/GraphViewStyle.uss", "TalentTree/NodeStyles.uss");
    }
    public IManipulator CreateTalentNodeContextualMenu()
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
            menuEvent => menuEvent.menu.AppendAction("Add Talent Node", actionEvent => AddElement(CreateTalentNode(GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
            );

        return contextualMenuManipulator;
    }
    public void LoadTalentNode()
    {
        string path = string.Format("Assets/Resources/MineResearchTalentTree/MineResearchStaticData.asset");
        m_TalentTreeStaticData = AssetDatabase.LoadAssetAtPath<MineResearchStaticData>(path);
        if (m_TalentTreeStaticData == null)
        {
            m_TalentTreeStaticData = ScriptableObject.CreateInstance<MineResearchStaticData>();
            m_TalentTreeStaticData.Initialize();

            AssetDatabase.CreateAsset(m_TalentTreeStaticData, path);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = m_TalentTreeStaticData;
        }

        Dictionary<MineResearchNode, MineResearchNodeEditor> cacheNodes = new();
        for (int i = 0; i < m_TalentTreeStaticData.m_TalentNode.Count; i++)
        {
            MineResearchNodeEditor talentNodeEditor = new();
            talentNodeEditor.Load(m_TalentTreeStaticData.m_TalentNode[i], m_TalentTreeStaticData);
            talentNodeEditor.Draw();

            AddElement(talentNodeEditor);

            cacheNodes.Add(m_TalentTreeStaticData.m_TalentNode[i], talentNodeEditor);
        }

        foreach (MineResearchNodeEditor talentNodeEditor in cacheNodes.Values)
        {
            for (int i = 0; i < talentNodeEditor.m_TalentNode.m_RequireNode.Count; i++)
            {
                Edge tempEdge = new Edge()
                {
                    input = talentNodeEditor.m_InputPort,
                    output = cacheNodes[talentNodeEditor.m_TalentNode.m_RequireNode[i] as MineResearchNode].m_OutputPort,

                };
                tempEdge?.input.Connect(tempEdge);
                tempEdge?.output.Connect(tempEdge);
                this.Add(tempEdge);
            }
        }

    }
    public MineResearchNodeEditor CreateTalentNode(Vector2 position)
    {
        MineResearchNodeEditor talentNodeEditor = new();
        talentNodeEditor.Initialize(position, m_TalentTreeStaticData);
        talentNodeEditor.Draw();
        return talentNodeEditor;
    }
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        ports.ForEach(port =>
        {
            if (startPort == port) return;
            if (startPort.node == port.node) return;
            if (startPort.direction == port.direction) return;
            compatiblePorts.Add(port);
        });
        return compatiblePorts;
    }
    public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
    {
        Vector2 worldMousePosition = mousePosition;
        if (isSearchWindow)
        {
            worldMousePosition -= m_TalentTreeWindowEditor.position.position;
        }
        Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
        return localMousePosition;
    }
    public void OnElementDeleted()
    {
        deleteSelection = (operationName, askUser) =>
        {
            System.Type edgeType = typeof(Edge);

            List<MineResearchNodeEditor> removeNodes = new List<MineResearchNodeEditor>();
            List<Edge> removeEdges = new List<Edge>();
            foreach (GraphElement element in selection)
            {
                if (element is MineResearchNodeEditor node)
                {
                    removeNodes.Add(node);
                    continue;
                }
                if (element.GetType() == edgeType)
                {
                    Edge edge = (Edge)element;
                    removeEdges.Add(edge);
                    continue;
                }
            }
            for (int i = 0; i < removeNodes.Count; i++)
            {
                removeNodes[i].DisconnectAllPort(this);
                removeNodes[i].OnDeleteNode();
                RemoveElement(removeNodes[i]);
            }

            DeleteElements(removeEdges);
        };
    }
    public void ClearGraph()
    {
        graphElements.ForEach(graphElement => RemoveElement(graphElement));
    }
    public void ChangeTalentTreeType()
    {
        LoadTalentNode();
    }

}
