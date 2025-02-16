using System.Collections;
using System.Collections.Generic;
using MyExtension;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[System.Serializable]
public class BigResourceData
{
    public string m_StringAmount;
    [SerializeField, BigNumberEditor]
    private BigNumber m_Amount;
    private readonly UnityEvent m_OnAddCallback = new UnityEvent();
    private readonly UnityEvent m_OnConsumeCallback = new UnityEvent();
    public void Add(BigNumber amount)
    {
        this.m_Amount += amount;
        this.m_OnAddCallback.Invoke();
    }
    public void Consume(BigNumber amount)
    {
        this.m_Amount -= amount;
        this.m_OnAddCallback.Invoke();
    }
    public bool IsEnough(BigNumber amount)
    {
        return amount < this.m_Amount;
    }
    public void AddAddCallback(UnityAction action)
    {
        m_OnAddCallback.AddListener(action);
    }
    public void AddConsumeCallback(UnityAction action)
    {
        m_OnConsumeCallback.AddListener(action);
    }
    public BigResourceData()
    {
        m_Amount = BigNumber.ZERO;
        m_StringAmount = m_Amount.ToString();
    }
    public BigResourceData(BigNumber am)
    {
        m_Amount = am;
        m_StringAmount = m_Amount.ToString();
    }
}
