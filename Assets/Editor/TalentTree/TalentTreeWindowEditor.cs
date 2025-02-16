using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

public enum TalentTreeType
{
    PLAYER_STATS,
    RESEARCH,
}

public class TalentTreeWindowEditor : EditorWindow
{
    private TalentTreeType m_TalentTreeType;
    private TalentTreeGraphView m_TalentTreeGraphView;
    [MenuItem("Tools/Talent Tree Window Editor")]
    public static void Open()
    {
        GetWindow<TalentTreeWindowEditor>();
    }
    private void OnEnable()
    {
        AddGraphView();
        AddToolBar();
        AddStyles();
    }
    public void AddGraphView()
    {
        m_TalentTreeGraphView = new TalentTreeGraphView(this, TalentTreeType.PLAYER_STATS);
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
        if (m_TalentTreeType == TalentTreeType.PLAYER_STATS) return;
        m_TalentTreeType = TalentTreeType.PLAYER_STATS;
        m_TalentTreeGraphView.ClearGraph();
        m_TalentTreeGraphView.ChangeTalentTreeType(TalentTreeType.PLAYER_STATS);

    }
    //public void LoadUtilityTalentTree()
    //{
    //    if (m_TalentTreeType == TalentTreeType.Utility) return;
    //    m_TalentTreeType = TalentTreeType.Utility;
    //    m_TalentTreeGraphView.ClearGraph();
    //    m_TalentTreeGraphView.ChangeTalentTreeType(TalentTreeType.Utility);
    //}
}
public class TalentTreeGraphView : GraphView
{
    private TalentTreeType m_TalentTreeType;
    private TalentTreeStaticData m_TalentTreeStaticData;
    private TalentTreeWindowEditor m_TalentTreeWindowEditor;

    public TalentTreeGraphView(TalentTreeWindowEditor talentTreeWindowEditor, TalentTreeType talentTreeType)
    {
        m_TalentTreeType = talentTreeType;
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
        string path = string.Format("Assets/Resources/TalentTreeGlobalConfig/{0}/{0}TalentTreeStaticData.asset", m_TalentTreeType);
        m_TalentTreeStaticData = AssetDatabase.LoadAssetAtPath<TalentTreeStaticData>(path);
        if (m_TalentTreeStaticData == null)
        {
            m_TalentTreeStaticData = ScriptableObject.CreateInstance<TalentTreeStaticData>();
            m_TalentTreeStaticData.Initialize();

            AssetDatabase.CreateAsset(m_TalentTreeStaticData, path);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = m_TalentTreeStaticData;
        }

        Dictionary<TalentNode, TalentNodeEditor> cacheNodes = new Dictionary<TalentNode, TalentNodeEditor>();
        for (int i = 0; i < m_TalentTreeStaticData.m_TalentNode.Count; i++)
        {
            TalentNodeEditor talentNodeEditor = new TalentNodeEditor();
            talentNodeEditor.Load(m_TalentTreeStaticData.m_TalentNode[i], m_TalentTreeStaticData);
            talentNodeEditor.Draw();

            AddElement(talentNodeEditor);

            cacheNodes.Add(m_TalentTreeStaticData.m_TalentNode[i], talentNodeEditor);
        }

        foreach (TalentNodeEditor talentNodeEditor in cacheNodes.Values)
        {
            for (int i = 0; i < talentNodeEditor.m_TalentNode.m_RequireNode.Count; i++)
            {
                Edge tempEdge = new Edge()
                {
                    input = talentNodeEditor.m_InputPort,
                    output = cacheNodes[talentNodeEditor.m_TalentNode.m_RequireNode[i]].m_OutputPort,

                };
                tempEdge?.input.Connect(tempEdge);
                tempEdge?.output.Connect(tempEdge);
                this.Add(tempEdge);
            }
        }

    }
    public TalentNodeEditor CreateTalentNode(Vector2 position)
    {
        TalentNodeEditor talentNodeEditor = new TalentNodeEditor();
        talentNodeEditor.Initialize(position, m_TalentTreeStaticData, m_TalentTreeType);
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

            List<TalentNodeEditor> removeNodes = new List<TalentNodeEditor>();
            List<Edge> removeEdges = new List<Edge>();
            foreach (GraphElement element in selection)
            {
                if (element is TalentNodeEditor node)
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
    public void ChangeTalentTreeType(TalentTreeType talentTreeType)
    {
        m_TalentTreeType = talentTreeType;
        LoadTalentNode();
    }

}
