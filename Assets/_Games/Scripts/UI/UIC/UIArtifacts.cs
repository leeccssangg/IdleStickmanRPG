using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIArtifacts : UICanvas
{
    Pextension.MiniPool<UIArtifactSlot> pool = new();
    [SerializeField] private UIArtifactSlot m_Prefab;
    [SerializeField] private RectTransform m_Container;
    [SerializeField] private ScrollRect m_Scroll;
    [SerializeField] private List<UIArtifactSlot> m_ListUi;
    [SerializeField] private TextMeshProUGUI m_TxtStone;
    [SerializeField] private TextMeshProUGUI m_TxtCost;
    [SerializeField] private UIButton m_BtnGatchaArtifact;

    public void AwakeInGame()
    {
        pool.OnInit(m_Prefab, 7, m_Container);
        m_BtnGatchaArtifact.onClick.AddListener(OnClickButtonGatcha);
        EventManager.StartListening("UpdateArtifactStone", UpdateTextStone);
        EventManager.StartListening("UpdateArtifactStone", UpdateButtonGatcha);
    }
    public override void Setup()
    {
        base.Setup();
        pool.Release();
        m_ListUi = new();
        for(int i = 1;i<= ArtifactsGlobalConfig.Instance.GetTotalSlotId(); i++)
        {
            UIArtifactSlot ui = pool.Spawn(m_Container.position,Quaternion.identity);
            ui.StartInGame();
            ui.Setup(i);
            m_ListUi.Add(ui);
        }
        UpdateTextStone();
        UpdateButtonGatcha();
    }
    private void UpdateTextStone()
    {
        m_TxtStone.text = "Stone: " + ProfileManager.PlayerData.PlayerResourceD.Resources.GetResourceValue(ResourceData.ResourceType.ARTIFACT_STONE).ToString();
    }
    private void UpdateButtonGatcha()
    {
        m_BtnGatchaArtifact.gameObject.SetActive(!ArtifactsManager.Ins.IsUnlockAllArtifact());
        m_BtnGatchaArtifact.interactable = ArtifactsManager.Ins.IsHaveEnoughResource();
        m_TxtCost.text = ArtifactsManager.Ins.GetCostUnlock().ToString3();
    }
    private void OnClickButtonGatcha()
    {
        ArtifactsManager.Ins.GatchaAritfact();
        UpdateButtonGatcha();
    }

}
