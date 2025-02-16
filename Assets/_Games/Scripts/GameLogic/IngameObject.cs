using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum IngameType {
    PLAYER = 0,
    ALLIANCE,
    ENEMY,
    BULLET,
    ITEM,
    WEAPON,
    STICKMAN
}
public class IngameObject : MonoBehaviour, ITakenDamage {
    public static int m_NextID = 0;
    public IngameType m_IngameType;
    protected int m_ID;

    public BigNumber m_MaxHP;
    public BigNumber m_HP;
    public float m_IncreaseHP;

    public bool IsInSideCamer;
    public bool IsHideOnRadar = false;
    protected Transform m_Transform;
    public Transform Transform {
        get {
            if (m_Transform == null) {
                m_Transform = transform;
            }
            return m_Transform;
        }
    }
    public int ID {
        get => m_ID;
        set => m_ID = value;
    }
    public virtual void Init() {

    }
    public virtual void Reset() {
    }
    public virtual void RegisterInScene() {
        while (IngameEntityManager.Ins.GetEntityFromID(m_NextID) != null) {
            m_NextID++;
        }
        m_ID = m_NextID;
        IngameEntityManager.Ins.RegisterEntity(this);
    }
    public virtual void UnregisterInScene() {
        IngameEntityManager.Ins.UnRegisterEntity(this);
    }
    public virtual void BattleCry() { 
    }
    public int GetID() {
        return m_ID;
    }

    public int GetSubType() {
        return -1;
    }  
    public virtual void OnHit( BigNumber damage, Vector3 hitPos, DamageInfo damageInfo = null ) {
    }
    public virtual void OnDamage(BigNumber damage, DamageInfo damageInfo ) {
    }
    public virtual bool IsDead() {
        return true;
    }
    public virtual int ApplyBuff( Buff buff ) {
        return 0;
    }
    public virtual void RemoveBuff(Buff buff) {
    }
    #region HP
    public BigNumber GetMaxHP() {
        return m_MaxHP;
    }
    public void SetMaxHP(BigNumber maxHp ) {
        float currentHPper = GetCurrentHpPercentage();
        m_MaxHP = maxHp;
        SetPerHP(currentHPper);
    }
    public void SetPerHP(float per) {
        m_HP = m_MaxHP * (per / 100f);
    }
    public void AddHP(BigNumber amount ) {
        m_HP += amount;
        if(m_HP > m_MaxHP) {
            m_HP = m_MaxHP;
        }
    }
    public void SetHP(BigNumber hp ) {
        m_HP = hp;
        if(m_HP > m_MaxHP) {
            m_HP = m_MaxHP;
        }
        if(m_HP < 0) m_HP = 0;
    }
    public float GetCurrentHpPercentage() {
        if(m_MaxHP == 0) return 100;
        BigNumber b = m_HP * (100/m_MaxHP);
        return b.ToFloat();
    }
    public string GetCurrentHPPerString() {
        string s = m_HP.ToString3() + "/" + m_MaxHP.ToString3();
        return s;
    }
    #endregion
    public virtual void Deactive() { 
        UnregisterInScene();
        SimplePool.Despawn(this.gameObject);
    }
    public virtual bool IsDeActive() {
        bool isActive = gameObject.activeInHierarchy;
        return !isActive;
    }
    public virtual bool IsOutCamera(){
        return CameraManger.IsOutOfCamera(Transform.position);
    }
}

public interface ITakenDamage {
    int GetID();
    void OnHit( BigNumber damage, Vector3 hitPos, DamageInfo damageInfo = null );
    int ApplyBuff( Buff buff );
    int GetSubType();
    bool IsDead();
}
