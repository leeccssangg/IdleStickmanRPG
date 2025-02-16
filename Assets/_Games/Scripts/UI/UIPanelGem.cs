using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIPanelGem : MonoBehaviour{
    [SerializeField] private TextMeshProUGUI m_TextGem;
    [SerializeField] private Transform m_GemTransform;
    private BigNumber m_CurrentGem;
    private BigNumber m_TargetGem;
    private BigNumber m_ChangeRate;
    public Transform GemTransform{ get => m_GemTransform; set => m_GemTransform = value; }
    private void Awake(){
        EventManager.StartListening(ConstantString.EVENT_UPDATGEM, UpdateGem);
    }
    private void OnEnable(){
        m_CurrentGem = ProfileManager.PlayerData.GetResoureValue(ResourceData.ResourceType.GEM);
        m_TextGem.text = m_CurrentGem.ToString3();
        m_TargetGem = m_CurrentGem;
    }
    private void Update(){
        if(m_CurrentGem != m_TargetGem){
            if(m_CurrentGem < m_TargetGem){
                m_CurrentGem += m_ChangeRate * Time.deltaTime;
                if(m_CurrentGem > m_TargetGem) m_CurrentGem = m_TargetGem;
            }else{
                m_CurrentGem += m_ChangeRate * Time.deltaTime;
                if(m_CurrentGem < m_TargetGem) m_CurrentGem = m_TargetGem;
            }
            m_TextGem.text = m_CurrentGem.ToString3();
        }
    }
    
    public void UpdateGem(){
        m_TargetGem = ProfileManager.PlayerData.GetResoureValue(ResourceData.ResourceType.GEM);
        m_ChangeRate = (m_TargetGem - m_CurrentGem);
    }    
}