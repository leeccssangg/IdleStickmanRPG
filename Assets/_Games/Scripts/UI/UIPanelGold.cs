using TMPro;
using UnityEngine;

public class UIPanelGold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TextGold;
    [SerializeField] private Transform m_GoldTransform;
    private BigNumber m_CurrentGold;
    private BigNumber m_TargetGold;
    private BigNumber m_GoldRate;
    public Transform GoldTransform{ get => m_GoldTransform; set => m_GoldTransform = value; }
    private void Awake(){
        EventManager.StartListening(ConstantString.EVENT_UPDATEGOLD, UpdateGold);
    }
    private void OnEnable(){
        m_CurrentGold = ProfileManager.PlayerData.GetResoureValue(ResourceData.ResourceType.GOLD);
        m_TextGold.text = m_CurrentGold.ToString3();
        m_TargetGold = m_CurrentGold;
    }
    private void Update(){
        if(m_CurrentGold != m_TargetGold){
            if(m_CurrentGold < m_TargetGold){
                m_CurrentGold += m_GoldRate * Time.deltaTime;
                if(m_CurrentGold > m_TargetGold) m_CurrentGold = m_TargetGold;
            }else{
                m_CurrentGold += m_GoldRate * Time.deltaTime;
                if(m_CurrentGold < m_TargetGold) m_CurrentGold = m_TargetGold;
            }
            m_TextGold.text = m_CurrentGold.ToString3();
        }
    }
    
    public void UpdateGold(){
        m_TargetGold = ProfileManager.PlayerData.GetResoureValue(ResourceData.ResourceType.GOLD);
        m_GoldRate = (m_TargetGold - m_CurrentGold);
    }
}
