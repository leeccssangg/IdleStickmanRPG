using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIArtifact : MonoBehaviour
{
    [SerializeField] private Artifact m_Artifact;
    [SerializeField] private Image m_ImgSelect;
    [SerializeField] private Image m_ImgIcon;
    [SerializeField] private Image m_ImgLock;
    [SerializeField] private Image m_ImgUgradeAble;
    [SerializeField] private Button m_Btn;

    private void Awake()
    {
        m_Btn.onClick.AddListener(OnClickCallBack);
        EventManager.StartListening("UpdateArtifact", UpdateImgStatus);
    }
    public void Setup(Artifact artifact)
    {
        m_Artifact = artifact;
      
        UpdateImgStatus();
    }
    private void OnClickCallBack()
    {
        UIManager.Ins.OpenUI<UICPopupArtifactDetail>().Setup(m_Artifact);
    }
    public void UpdateImgStatus()
    {
        m_ImgSelect.gameObject.SetActive(ArtifactsManager.Ins.IsEquipedArtifact(m_Artifact));
        m_ImgLock.gameObject.SetActive(m_Artifact.GetLevel() < 1);
        m_ImgUgradeAble.gameObject.SetActive(m_Artifact.IsUpgradeAble());
    }
}
