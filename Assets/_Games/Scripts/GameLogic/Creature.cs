using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : IngameObject{
    public int m_CharacterID;
    public Transform m_HeadPoint;
    public Animator m_Animator;
    public Rigidbody2D m_Rigidbody;
    public Collider2D m_Collider;
    public Class m_Class;
    [SerializeField] private Platform m_Platform = Platform.GROUND;
    public IngameHealthbar m_HealthBar;
    public float m_OffSetYHealthBar = 0.25f;
    protected List<Buff> m_AddedBuffes = new List<Buff>();
    protected Movement m_Movement;
    protected BigNumber m_BonusHPPoint;
    protected BigNumber m_BonusAttackPoint;
    protected float m_BonusAttackSpeed;
    protected float m_ReduceAttackSpeed;
    protected float m_BonusInDamage;
    protected float m_ReduceInDamage;
    protected float m_BonusOutDamage;
    protected float m_ReduceOutDamage;
    protected float m_BonusSkillDamage;
    protected float m_BonusDoubleShotChance;
    protected float m_BonusCriticalChance;
    protected float m_BonusCriticalDamage;
    protected float m_BonusBlockChance;
    protected float m_BonusEvadeChance;
    protected float m_BonusBuffTime;
    protected float m_BonusHealRate;
    protected float m_BonusHPPercent;
    protected float m_BonusCooldownTime;
    protected float m_BonusStunChance;
    protected float m_ReflectPoint;
    protected float m_BonusTrueShootChance;
    protected float m_BonusLastWordDamage;
    private bool isHoldingAttack;
    public bool IsHoldingAttack{ get { return isHoldingAttack; } set { isHoldingAttack = value; } }
    public virtual Movement Movement{ get { return m_Movement; } }
    public Platform Platform{ get => m_Platform; set => m_Platform = value; }
    private void Awake(){
        Init();
    }
    public override void BattleCry(){
        base.BattleCry();
    }
    public override void Init(){
        base.Init();
        m_Transform = transform;
    }
    public virtual void InitHealthbar(){
        if(m_HealthBar == null){
            GameObject go = PrefabManager.Instance.SpawnPool("HealthBar");
            m_HealthBar = go.GetComponent<IngameHealthbar>();
        }
    }
    public virtual void OnRunning(){ }
    public override void OnHit(BigNumber damage, Vector3 hitPos, DamageInfo damageInfo){
        base.OnHit(damage, hitPos, damageInfo);
        if(IsDead()) return;

        //if(damage !=null && damageInfo.damageType == DamageType.NORMAL) {
        //}
        OnDamage(damage, damageInfo);
    }
    public override void OnDamage(BigNumber damage, DamageInfo damageInfo){
        base.OnDamage(damage, damageInfo);
        if(damage == 0 && damageInfo.damageType == DamageType.MISS){
            //IngameManager.Instance.PutMiss(Transform.position + new Vector3(0, 0.5f, 0), damageInfo.damageType);
            return;
        } else{
            IngameManager.Ins.PutTextDamage(damage, m_HeadPoint.position /*+ new Vector3(0, .7f, 0)*/,
                                            damageInfo.damageType);
        }

        m_HP -= damage;
        if(m_HP <= 0){
            m_HP = 0;
            Dead();
        }
    }
    public void DeactiveHPBar(){
        if(m_HealthBar != null){
            m_HealthBar.Despawn();
            m_HealthBar = null;
        }
    }
    public virtual void Dead(){
        DeactiveHPBar();
        RemoveAllBuff();
    }
    public override bool IsDead(){
        return m_HP <= 0 || IsDeActive();
    }
    //public void DeactiveHPBar() {
    //    if (m_HealthBar != null) {
    //        m_HealthBar.Despawn();
    //        m_HealthBar = null;
    //    }
    //}

#region BUFF
    public virtual void OnBuffRunning() {
        int num = 0;
        while (num < m_AddedBuffes.Count) {
            Buff buff = m_AddedBuffes[num];
            if (!buff.IsLinkedBuff()) {
                buff.Update();
                if (buff.IsOverTime()) {
                    RemoveBuff(buff);
                    continue;
                }
            } else {
                if (buff.IsOwnerDeactive()) {
                    RemoveBuff(buff);
                    continue;
                }
            }
            num++;
        }
    }
    public virtual void RemoveAllBuff() {
        while (m_AddedBuffes.Count > 0) {
            RemoveBuff(m_AddedBuffes[0]);
        }
    }
    public override void RemoveBuff(Buff buff) {
        if (m_AddedBuffes.Contains(buff)){
            buff.EndBuff();
            m_AddedBuffes.Remove(buff);
        }
    }
#endregion
#region Add Stat
    public void AddBonusHPPoint(BigNumber amount){
        m_BonusHPPoint += amount;
    }
    public void AddBonusHPPercent(float amount){
        m_BonusHPPercent += amount;
    }
    public void AddBonusAttackPoint(BigNumber amount){
        m_BonusAttackPoint += amount;
    }
    public void AddBonusAttackSpeed(float amount){
        m_BonusAttackSpeed += amount;
    }
    public void AddReduceAttackSpeed(float amount){
        m_ReduceAttackSpeed += amount;
    }
    public void AddBonusInDamage(float amount){
        m_BonusInDamage += amount;
    }
    public void AddBonusOutDamage(float amount){
        m_BonusOutDamage += amount;
    }
    public void AddReduceOutDamage(float amount){
        m_ReduceOutDamage += amount;
    }
    public void SetReduceInDamage(float amount){
        m_ReduceInDamage = amount;
    }
    public void AddReduceInDamage(float amount){
        m_ReduceInDamage += amount;
    }
    public void AddBonusCooldownTime(float amount){
        m_BonusCooldownTime += amount;
    }
    public void AddBonusStunChance(float amount){
        m_BonusStunChance += amount;
    }
    public void AddBonusReflectPoint(float amount){
        m_ReflectPoint += amount;
    }
    public void AddBonusTrueShootChance(float amount){
        m_BonusTrueShootChance += amount;
    }
    public void AddBonusLastWord(float amount){
        m_BonusLastWordDamage += amount;
    }
#endregion

#region Sub Stat
    public void SubtractBonusHPPoint(BigNumber amount){
        m_BonusHPPoint -= amount;
    }
    public void SubtractBonusHPPercent(float amount){
        m_BonusHPPercent -= amount;
    }
    public void SubtractBonusAttackPoint(BigNumber amount){
        m_BonusAttackPoint -= amount;
    }
    public void SubtractBonusAttackSpeed(float amount){
        m_BonusAttackSpeed -= amount;
    }
    public void SubtractReduceAttackSpeed(float amount){
        m_ReduceAttackSpeed -= amount;
    }
    public void SubtractBonusInDamage(float amount){
        m_BonusInDamage -= amount;
    }
    public void SubtractBonusOutDamage(float amount){
        m_BonusOutDamage -= amount;
    }
    public void SubtractReduceOutDamage(float amount){
        m_ReduceOutDamage -= amount;
    }
    public void SubtractReduceInDamage(float amount){
        m_ReduceInDamage -= amount;
    }
    public void SubtractBonusCoolDownTime(float amount){
        m_BonusCooldownTime -= amount;
    }
    public void SubtractBonusStunChance(float amount){
        m_BonusStunChance -= amount;
    }
    public void SubtractBonusReflectPoint(float amount){
        m_ReflectPoint -= amount;
    }
    public void SubtractBonusTrueShootChance(float amount){
        m_BonusTrueShootChance -= amount;
    }
    public void SubtractBonusLastWord(float amount){
        m_BonusLastWordDamage -= amount;
    }
#endregion

#region Get
    public virtual BigNumber GetBonusHPPoint(){
        return m_BonusHPPoint;
    }
    public virtual BigNumber GetBonusAttackPoint(){
        return m_BonusAttackPoint;
    }
    public virtual float GetBonusInDamage(){
        return m_BonusInDamage;
    }
    public virtual float GetBonusHPPercent(){
        return m_BonusHPPercent;
    }
    public virtual float GetBonusOutDamage(){
        return m_BonusOutDamage;
    }
    public virtual float GetReduceOutDamage(){
        return m_ReduceOutDamage;
    }
    public virtual float GetReduceInDamage(){
        return m_ReduceInDamage;
    }
    public virtual float GetReduceAttackSpeed(){
        return m_ReduceAttackSpeed;
    }
    public virtual float GetBonusAttackSpeed(){
        return m_BonusAttackSpeed;
    }
    public virtual float GetBonusSkillDamage(){
        return m_BonusSkillDamage;
    }
    public virtual float GetBonusCritChance(){
        return m_BonusCriticalChance;
    }
    public virtual float GetBonusCritDamage(){
        return m_BonusCriticalDamage;
    }
    public virtual float GetBonusBlockChance(){
        return m_BonusBlockChance;
    }
    public virtual float GetBonusEvadeChance(){
        return m_BonusEvadeChance;
    }
    public virtual float GetBonusBuffTime(){
        return m_BonusBuffTime;
    }
    public virtual float GetBonusHealRate(){
        return m_BonusHealRate;
    }
    public virtual float GetDoubleShotChance(){
        return m_BonusDoubleShotChance;
    }
    public virtual float GetBonusCooldownTime(){
        return m_BonusCooldownTime;
    }
    public virtual float GetBonusStunChance(){
        return m_BonusStunChance;
    }
    public virtual float GetBonusReflectPoint(){
        return m_ReflectPoint;
    }
    public virtual float GetBonusTrueShootChance(){
        return m_BonusTrueShootChance;
    }
    public virtual float GetBonusLastWordDamage(){
        return m_BonusLastWordDamage;
    }
#endregion
}