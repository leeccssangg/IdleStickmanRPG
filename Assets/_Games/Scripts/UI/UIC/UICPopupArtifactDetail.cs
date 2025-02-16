using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UICPopupArtifactDetail : UICanvas
{
    [SerializeField] private Artifact m_Artifact;
    [SerializeField] private Button m_BtnEquip;
    [SerializeField] private Button m_BtnUnEquip;
    [SerializeField] private Button m_BtnClose;
    [SerializeField] private Button m_BtnUpgrade;
    [SerializeField] private Button m_BtnMaxlevel;
    [SerializeField] private TextMeshProUGUI m_TxtConstUpgrade;

    private void Awake()
    {
        m_BtnEquip.onClick.AddListener(EquipArtifact);
        m_BtnUnEquip.onClick.AddListener(UnEquipArtifact);
        m_BtnClose.onClick.AddListener(OnClickButtonClose);
        m_BtnUpgrade.onClick.AddListener(UpgradeArtifact);
        EventManager.StartListening("UpdateArtifact", UpdateButtonStatus);
    }
    public void Setup(Artifact artifact)
    {
        m_Artifact = artifact;
        UpdateText();
        UpdateButtonStatus();
    }
    private void UpdateText()
    {
        m_TxtConstUpgrade.text = new BigNumber(m_Artifact.GetCostUpgrade()).ToString3();
    }
    private void UpdateButtonStatus()
    {
        if (!isActiveAndEnabled)
            return;

        m_BtnEquip.gameObject.SetActive(m_Artifact.GetLevel() > 0 && !ArtifactsManager.Ins.IsEquipedArtifact(m_Artifact));
        m_BtnUnEquip.gameObject.SetActive(m_Artifact.GetLevel() > 0 && ArtifactsManager.Ins.IsEquipedArtifact(m_Artifact));

        m_BtnMaxlevel.gameObject.SetActive(m_Artifact.IsMaxLevel());

        m_BtnUpgrade.gameObject.SetActive(m_Artifact.GetLevel() > 0 && !m_Artifact.IsMaxLevel());
        m_BtnUpgrade.interactable = ArtifactsManager.Ins.IsUpgradeAbleArtifact(m_Artifact);
    }
    private void UnlockArtifact()
    {
        ArtifactsManager.Ins.UnLockArtifact(m_Artifact);
    }
    private void UpgradeArtifact()
    {
        ArtifactsManager.Ins.UpgradeArtifact(m_Artifact);
        if (!m_Artifact.IsMaxLevel())
            UpdateText();
    }
    private void EquipArtifact()
    {
        ArtifactsManager.Ins.EquipArtifact(m_Artifact);
    }
    private void UnEquipArtifact()
    {
        ArtifactsManager.Ins.UnEquipArtifact(m_Artifact);
    }
    private void OnClickButtonClose()
    {
        UIManager.Ins.CloseUI<UICPopupArtifactDetail>();
    }
}
