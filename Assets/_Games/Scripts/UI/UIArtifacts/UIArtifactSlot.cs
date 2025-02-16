using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIArtifactSlot : MonoBehaviour
{
    Pextension.MiniPool<UIArtifact> pool = new();
    [SerializeField] private UIArtifact m_Prefab;
    [SerializeField] private RectTransform m_Container;
    [SerializeField] private List<UIArtifact> m_ListUi;
    [SerializeField] private int m_SlotID;
    [SerializeField] private List<Artifact> m_ListArtifacts;
    [SerializeField] private Image m_ImgLock;

    private void Awake()
    { 
        EventManager.StartListening("UpdateArtifact", UpdateImgStatus);
    }
    public void StartInGame()
    {
        pool.OnInit(m_Prefab, 4, m_Container);
    }
    public void Setup(int slotId)
    {
        pool.Release();
        m_ListUi = new();
        m_SlotID = slotId;
        m_ListArtifacts = ArtifactsManager.Ins.GetListArtifactWithSlotId(m_SlotID);
        for(int i = 0; i < m_ListArtifacts.Count; i++)
        {
            UIArtifact ui = pool.Spawn(m_Container.position, Quaternion.identity);
            ui.Setup(m_ListArtifacts[i]);
            m_ListUi.Add(ui);
        }
        UpdateImgStatus();
    }
    private void UpdateImgStatus()
    {
        m_ImgLock.gameObject.SetActive(!ArtifactsManager.Ins.IsUnlockThreeOfLastSlotArtifact(m_SlotID));
    }
}
