using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType{
    NONE = 0, 
    ICE = 1,
    FIRE = 2,
    POISON = 3,
}

[System.Serializable]
public class Buff{
    private static int nextID;
    protected int id;
    protected int ownerID;
    protected bool isLinkedBuff = false;
    protected Character owner;
    protected Character creator;
    protected DamageInfo damageInfo;
    protected BigNumber lastWordDamage;
    protected bool IsDebuff = false;
    protected Effect effect = null;

    public int ID => id;
    private Vector3 direction;
    public Vector3 Direction {
        get => direction;
        set => direction = value;
    }
    [SerializeField]private BuffType buffType;
    public BuffType BuffType {
        get => buffType;
        set => buffType = value;
    }
    protected float value;
    public float Value {
        get => value;
        set => this.value = value;
    }
    public BigNumber LastWordDamage {
        get => lastWordDamage;
        set => lastWordDamage = value;
    }
    public bool IsPernamentBuff;
    public float m_AffectedTime;
    public virtual void Update() {
        if (m_AffectedTime > 0) {
            m_AffectedTime -= Time.deltaTime;
        }
    }
    public void Init(BuffType buffType, float value, bool isLinkedBuff) {
        Init(buffType, value, 0, true, isLinkedBuff);
    }
    public void Init(BuffType buffType, float value, float affectedTime, bool isLinkedBuff) {
        Init(buffType, value, affectedTime, false, isLinkedBuff);
    }
    public void Init(BuffType buffType, float value, float affectedTime, bool isPernament, bool isLinkedBuff) {
        this.id = nextID;
        this.buffType = buffType;
        this.value = value;
        this.m_AffectedTime = affectedTime;
        this.IsPernamentBuff = isPernament;
        this.isLinkedBuff = isLinkedBuff;
        this.lastWordDamage = 0;
        nextID++;
    }
    public void InitDamageInfo(Character owner) {
        damageInfo = new DamageInfo {
            owner = owner,
            damageType = DamageType.NORMAL,
            characterID = owner.m_CharacterID,
            classType = owner.m_Class
        };
    }
    public void ApplyToOwner(Character owner) {
        SetOwner(owner);
        InitDamageInfo(owner);
    }
    public void SetTime(float time) {
        m_AffectedTime = time;
    }
    public float GetTime() {
        return m_AffectedTime;
    }
    public void SetCreator(Character creator) {
        this.creator = creator;
    }
    public Character GetCreator() {
        return this.creator;
    }
    public void SetOwner(Character owner) {
        this.owner = owner;
    }
    public void SetOwnerID(int ownerID) {
        this.ownerID = ownerID;
    }
    public int GetOwnerID() {
        return ownerID;
    }
    public bool IsOverTime() {
        return m_AffectedTime <= 0;
    }
    public bool IsLinkedBuff() {
        return isLinkedBuff;
    }
    public bool IsOwnerDeactive() {
        return owner.IsDeactive();
    }
    public void ClearEffect() {
        if (effect == null) return;
        effect.Deactive();
        effect = null;
    }
    public virtual void StartBuff() { }
    public virtual void EndBuff() {
        if (LastWordDamage > 0) {
            DamageInfo damageInfo = new DamageInfo() {
                damageType = DamageType.SKILL,
                owner = owner
            };
            //owner.OnHit(LastWordDamage, owner.m_BodyPoint.position, damageInfo);
        }
    }
}
public class PoisonBuff : Buff {
    public PoisonBuff() {
        BuffType = BuffType.POISON;
        IsDebuff = true;
    }
    public BigNumber damageOverTime;
    private float currentCountTime;
    private float damagedRate = 1;
    public override void StartBuff() {
        base.StartBuff();
        owner.AddReduceOutDamage(Value);
        Vector3 offsetFxPos = new Vector3(0, .5f, 0);
        var eb = IngameManager.Ins.PutEffect(owner.m_HeadPoint.position + offsetFxPos,"Effect_Poison");
        eb.SetFollow(owner);
        eb.Setup(m_AffectedTime);
        effect = eb;
    }
    public override void Update() {
        base.Update();
        currentCountTime -= Time.deltaTime;
        if (currentCountTime <= 0) {
            currentCountTime = damagedRate;
            DealDamage();
        }
    }
    public override void EndBuff() {
        base.EndBuff();
        owner.SubtractReduceOutDamage(Value);
        ClearEffect();
    }
    private void DealDamage() {
        owner.OnHit(damageOverTime, owner.Transform.position, new DamageInfo { damageType = DamageType.POISON });
    }
}