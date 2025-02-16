using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UIDungeonInfo : MonoBehaviour
{
    [SerializeField] private Dungeon m_Dungeon;

    [SerializeField] private TextMeshProUGUI m_TxtDungeonName;
    [SerializeField] private TextMeshProUGUI m_TxtKeyCount;
    [SerializeField] private Button m_BtnOpenDungeonDetail;
    
    
    private UnityAction<Dungeon> m_EnterCallback;
    private void Awake()
    {
        m_BtnOpenDungeonDetail.onClick.AddListener(OpenDungeonDetail);
    }
    public void InitData(Dungeon dungeon, UnityAction<Dungeon> enterCallback)
    {
        m_Dungeon = dungeon;
        EventManager.StartListening("Update" + m_Dungeon.GetDungeonName() + KeyType.ADS.ToString(),UpdateKeyInfo);
        EventManager.StartListening("Update" + m_Dungeon.GetDungeonName() + KeyType.FREE.ToString(), UpdateKeyInfo);
        EventManager.StartListening("Update" + m_Dungeon.GetDungeonName() + KeyType.BONUS.ToString(), UpdateKeyInfo);
        m_TxtDungeonName.text = m_Dungeon.GetDungeonName();
        UpdateKeyInfo();
        m_EnterCallback = enterCallback;
    }
    public void SaveData()
    {
        m_Dungeon.SaveData();
    }
    private void OpenDungeonDetail()
    {
        UIManager.Ins.OpenUI<UICDungeonDetail>().InitData(m_Dungeon,OnEnterDungeon);
    }
    private void OnEnterDungeon()
    {
        m_EnterCallback?.Invoke(m_Dungeon);
    }
    private void UpdateKeyInfo()
    {
        m_TxtKeyCount.text = m_Dungeon.GetFreeBonusKeyCount().ToString() + "/" + m_Dungeon.GetNumKeyADay().ToString();
    }
}
