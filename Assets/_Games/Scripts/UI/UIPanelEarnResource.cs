using System.Collections;
using System.Collections.Generic;
using AssetKits.ParticleImage;
using UnityEngine;

public class UIPanelEarnResource : UICanvas
{
    [SerializeField] private ParticleImage m_CoinEffect;
    [SerializeField] private ParticleImage m_GemEffect;
    private Transform m_CoinTarget;
    
    public void PlayEarnResourceEffect(Vector3 from, ResourceData.ResourceType type)
    {
        switch (type)
        {
            case ResourceData.ResourceType.GOLD:
                PlayCoinEffect(from);
                break;
            case ResourceData.ResourceType.GEM:
                PlayGemEffect(from);
                break;
        }
    }
    public void PlayCoinEffect(Vector3 from)
    {
        if(m_CoinTarget == null){ m_CoinTarget = UIManager.Ins.GetUI<UIPanelIngame>().PanelGold.GoldTransform;}
        m_CoinEffect.transform.position = from;
        m_CoinEffect.attractorTarget = m_CoinTarget; 
        m_CoinEffect.Play();
    }
    public void PlayGemEffect(Vector3 from)
    {
        if(m_CoinTarget == null){ m_CoinTarget = UIManager.Ins.GetUI<UIPanelIngame>().PanelGem.GemTransform;}
        m_GemEffect.transform.position = from;
        m_GemEffect. attractorTarget = m_CoinTarget; 
        m_GemEffect.Play();
    }
}
