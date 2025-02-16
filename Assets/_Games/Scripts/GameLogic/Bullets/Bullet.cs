using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum DamageType { NORMAL = 0, CRIT = 1, CHAOS = 2, POISON = 3, FIRE = 4, SKILL = 5, MISS = 6, REFLECT = 7, TAP = 8 }
[System.Serializable]
public class DamageInfo {
    public DamageInfo(){
        damageType = DamageType.NORMAL;
    }
    public int characterID;
    public Creature owner;
    public DamageType damageType;
    public Class classType;

}
[System.Serializable]
public class BulletInfo {
    public BigNumber damage;
    public int maxPiercingTarget;
    public float speed;
    public float maxSpeed;
    public float minSpeed;
    public float accel;
    public float piercing;
    public float grow;
    public float chaosValue;
    public float knockBack;
    //public TrueShoot trueShoot;
    public float criticalChance;
    public float criticalDamage;
    public bool isRotate;
    public bool isComeBack;

    public string endEffect = "";
    public string prefabName = "";
    //public string spriteName = "";
    public DamageInfo damageInfo;
    public IngameTeam targetTeam;
    public IngameTeam ownerTeam;
    public List<Buff> buffes = new List<Buff>();
}
public class TrueShoot {
    public float trueShootChance;
    public float trueShootDamage;
    public BigNumber maxDamage;
    public void Init( float chance, float damage, BigNumber maxDamage ) {
        this.trueShootChance = chance;
        this.trueShootDamage = damage;
        this.maxDamage = maxDamage;
    }
}
public class Bullet : IngameObject
{
    public BigNumber m_Damage;
    public BigNumber m_BaseDamage;
    public BigNumber m_MinDamage;
    public IngameTeam m_OwnerTeam;
    public IngameTeam m_TargetTeam;
    public Rigidbody2D m_Rigidbody;
    public Collider2D m_Collider;
    public SpriteRenderer m_Image;
    public IngameObject m_LockedTarget;
    public Animator m_Animator;
    public bool IsRotate = false;
    public bool IsComeBack = false;
    public bool IsEndEffectOnTarget = false;
    public float m_KnockBack;
    public int m_OwnerID;
    public string m_EndEffect;
    public DamageInfo m_DamageInfo;
    private float speed;
    protected float m_MaxSpeed;
    protected float m_MinSpeed;
    protected float m_Accel;
    protected float m_AccelTurn = 360;
    protected float m_RotateSpeed = 720;
    protected float m_LifeTime = 3;
    protected float m_Piercing;
    protected float m_Explosive;
    [SerializeField]protected float m_ExplosiveRange;
    protected float m_CurrentPiercing;
    protected float m_GrowUp;
    protected float m_CurrentGrowUp;
    protected TrueShoot m_TrueShoot;
    protected bool IsDealDamage = false;
    protected Vector3 m_Direction;
    protected List<Buff> m_Buffes = new List<Buff>();
    protected Transform m_Target;
    protected Vector2 m_TargetPos;
    protected bool IsCommingBack = false;
    protected int m_MaxPiercingTarget = 0;
    protected int m_CurrentPiercingTarget = 0;

    protected StateMachine<Bullet> m_StateMachine;
    public StateMachine<Bullet> StateMachine {
        get {
            return m_StateMachine;
        }
    }

    public float Speed { get => speed; set => speed = value; }

    protected virtual void Awake() {
        base.Init();
        InitStateMachine();
        m_Transform = transform;
    }
    private void Update() {
        m_StateMachine.Update();
    }
    protected void InitStateMachine() {
        m_StateMachine = new StateMachine<Bullet>(this);
        m_StateMachine.SetCurrentState(BulletWaitingState.Instance);
        m_StateMachine.SetGlobalState(BulletGlobalState.Instance);
    }
    public virtual void InitBullet( BigNumber damage, IngameTeam targetTeam, IngameTeam ownerTeam, string spriteName ) {
        m_Damage = damage;
        m_BaseDamage = damage;
        m_MinDamage = m_BaseDamage / 10f;
        m_TargetTeam = targetTeam;
        m_OwnerTeam = ownerTeam;
        m_Buffes.Clear();
        if(m_Collider) m_Collider.enabled = true;
        IsDealDamage = false;
        if (m_Image != null) {
           // m_Image.sprite = SpriteManager.Instance.GetBulletSprite(spriteName);
        }
        IsEndEffectOnTarget = false;
        IsRotate = false;
        IsComeBack = false;
        IsCommingBack = false;
        m_LockedTarget = null;
        m_CurrentPiercingTarget = 0;
    }
    public void InitSpeed( float speed, float maxSpeed, float minSpeed, float accel ) {
        Speed = speed;
        m_MaxSpeed = maxSpeed;
        m_MinSpeed = minSpeed;
        m_Accel = accel;
    }
    public void InitPiercingValue( float piercing ) {
        m_Piercing = piercing;
        m_CurrentPiercing = 0;
    }
    public int AddBuff( Buff buff ) {
        m_Buffes.Add(buff);
        return 0;
    }
    public void InitMaxPiercingTarget( int amount ) {
        m_MaxPiercingTarget = amount;
    }
    public void InitGrowUpValue( float growUp ) {
        m_GrowUp = growUp;
        m_CurrentGrowUp = 0;
    }
    public void InitExplosive(float value ) {
        m_Explosive = value;
    }
    protected Vector3 MoveToTarget( Vector3 target ) {
        float dt = Time.deltaTime;
        Vector3 v1 = Transform.position;
        Vector3 v2 = target - Transform.position;
        m_Direction = v2.normalized;
        Speed = Speed + m_Accel * dt;
        Speed = Mathf.Clamp(Speed, m_MinSpeed, m_MaxSpeed);
        float num1 = Speed * dt;
        if (num1 > v2.magnitude) {
            num1 = v2.magnitude;
        }
        Vector3 movement = v2.normalized * num1;
        return movement;
    }
    public void SetLockedTarget( IngameObject target ) {
        m_LockedTarget = target;
    }
    public virtual void FireToward( Vector3 direction ) {
        m_Direction = direction;
        float angle = Utilss.AngleBetweenVectors(Vector3.up, direction);
        Transform.localEulerAngles = new Vector3(0, 0, -angle);
        StateMachine.ChangeState(BulletFireForwardState.Instance);
    }
    public virtual void FireFollowTarget( Transform target ) {
        m_Target = target;
        StateMachine.ChangeState(BulletFollowTargetState.Instance);
    }
    public virtual void FireBoomerang( Vector3 direction ) {
        m_Direction = direction;
        float angle = Utilss.AngleBetweenVectors(Vector3.up, direction);
        m_Transform.localEulerAngles = new Vector3(0, 0, -angle);
        StateMachine.ChangeState(BulletFireBoomerangState.Instance);
    }
    public virtual void FireBoom(Vector3 dir) {
        m_Rigidbody.velocity = dir * Speed;
        m_LifeTime = 2;
    }
    public virtual void FireChase(Transform target ) {
        m_Target = target;
        StateMachine.ChangeState(BulletChaseTargetState.Instance);
    }
    public virtual void FireBoom() {
        StateMachine.ChangeState(BulletBoomState.Instance);
    }
    public virtual void FireMissile(Vector2 targetPos){
        m_TargetPos = targetPos;
        StateMachine.ChangeState(BulletMissileState.Instance);
    }
    public virtual void FireTween() {
        Transform.DOKill();
        StateMachine.ChangeState(BulletTweenState.Instance);
    }
    #region State
    //=========GlobalState
    public virtual void OnRunning() {
        m_LifeTime -= Time.deltaTime;
        if (m_LifeTime <= 0) {
            ForceExecute();
        }
        if (IsRotate) {
            Vector3 v = Transform.localEulerAngles;
            v.z += Time.deltaTime * m_RotateSpeed;
            Transform.localEulerAngles = v;
        }
        // if(IsOutCamera()) {
        //     Destroy();
        // }
    }
    //=============WaitState
    public virtual void OnWaitingStart() {

    }
    public virtual void OnWaitingExecute() {

    }
    public virtual void OnWatingExit() {

    }
    //==========Fire forward state
    public virtual void OnStartFireToward() {
    }
    public virtual void OnFiringToward() {
        Transform.position += MoveToTarget(Transform.position + m_Direction.normalized);
    }

    //========Fire Boomerang
    private float boomerangTime = 0;
    [SerializeField]private float backTime = 0.5f;
    public virtual void OnStartFireBoomerang() {
        boomerangTime = 0;
    }
    public virtual void OnExecuteFireBoomerang() {
        Transform.position += MoveToTarget(Transform.position + m_Direction.normalized);
        if (boomerangTime < backTime) {
            boomerangTime += Time.deltaTime;
            if (boomerangTime >= backTime) {
                m_Direction = -m_Direction;
                IsCommingBack = true;
            }
        }
    }

    //========Follow Target
    public virtual void OnFollowTargetStart() { }
    public virtual void OnFollowTargetExecute() {
        Transform.position += MoveToTarget(m_Target.position);
    }
    #endregion
    private void OnTriggerEnter2D( Collider2D collider ) {
        OnExecuteTriggerEnter2D(collider);
    }
    //=============Fire boom
    public virtual void OnBoomStart() { }
    public virtual void OnBoomExecute() { }
    public virtual void OnBoomExit() { }
    //==============Fire Missile
    public virtual void OnMissileStart() { }
    public virtual void OnMissileExecute() { }
    public virtual void OnMissileExit() { }
    //=============Fire Chase
    public virtual void OnChaseStart() { }
    public virtual void OnChaseExecute() { }
    public virtual void OnChaseExit() { }
    //=============Custom
    public virtual void OnTweenFireStart() { }
    public virtual void OnTweenFireExecute() { }
    public virtual void OnTweenFireExit() { }
    
    public virtual void OnExecuteTriggerEnter2D( Collider2D collider ) {
        if (IsDealDamage)
            return;
        //Debug.Log(gameObject.name + " Target " + m_TargetTeam + " co " + collider.gameObject.name);
        switch (m_TargetTeam) {
            case IngameTeam.Team2: {
                if (collider.gameObject.tag == "Team2") {
                    ITakenDamage it = collider.attachedRigidbody.gameObject.GetComponent<ITakenDamage>();
                    if (it != null && !it.IsDead()) {
                        if (m_LockedTarget != null) {
                            if (m_LockedTarget.ID != it.GetID()) {
                                return;
                            }
                        }
                        //ApplyBuff(it);
                        it.OnHit(m_Damage, Transform.position, m_DamageInfo);
                        Execute();
                        EffectImpact(collider.transform);
                    }
                }
            }
            break;
            case IngameTeam.Team1: {
                if (collider.gameObject.tag == "Character") {
                    ITakenDamage it = collider.attachedRigidbody.gameObject.GetComponent<ITakenDamage>();
                    if (it != null && !it.IsDead()) {
                        if (m_LockedTarget != null) {
                            if (m_LockedTarget.ID != it.GetID()) {
                                return;
                            }
                        }
                        //ApplyBuff(it);

                        it.OnHit(m_Damage, Transform.position, m_DamageInfo);
                       
                        Execute();
                        EffectImpact(collider.transform);
                    }
                }
            }
            break;
        }
    }
    protected virtual void Execute() {
        if (m_Explosive > 0) {
            m_Damage = m_BaseDamage * (m_Explosive / 100f);
            List<IngameObject> list = IngameEntityManager.Ins.GetAllEnemyInRange(m_OwnerTeam, Transform.position,m_ExplosiveRange);
            for (int i = 0; i < list.Count; i++) {
                IngameObject igo = list[i];
                igo.OnHit(m_Damage, Transform.position, m_DamageInfo);
                //ApplyBuff()
            }
            ForceExecute();
            return;
        }
        if (m_Piercing > 0) {
            //Debug.Log("1Target " + m_CurrentPiercingTarget + " ------- " + m_CurrentPiercing + "____Damage " + m_Damage.ToString3());
            if (m_CurrentPiercing >= 100) {
                m_Damage = m_MinDamage;
            } else {
                m_CurrentPiercing += m_Piercing;
                m_Damage = m_BaseDamage - m_BaseDamage * (m_CurrentPiercing / 100f);
            }
            //Debug.Log("2Target " + m_CurrentPiercingTarget + " ------- " + m_CurrentPiercing + "____Damage " + m_Damage.ToString3());
            m_CurrentPiercingTarget++;
            if (m_CurrentPiercingTarget >= m_MaxPiercingTarget) {
                ForceExecute();
            }
        } else if (m_GrowUp > 0) {
            m_Damage = m_BaseDamage - m_BaseDamage * (m_CurrentGrowUp / 100f);
            m_CurrentGrowUp += m_GrowUp;
            if (m_CurrentGrowUp >= 400)
                m_CurrentGrowUp = 300;
        } else {
            ForceExecute();
        }
    }
    public virtual void Explosive() {
        List<IngameObject> list = IngameEntityManager.Ins.GetAllEnemyInRange(m_OwnerTeam,Transform.position,m_ExplosiveRange);
        if(m_EndEffect != "")
            IngameManager.Ins.PutEffectImpact(m_EndEffect,Transform.position);
        for(int i = 0;i < list.Count;i++) {
            IngameObject igo = list[i];
            igo.OnHit(m_Damage,Transform.position,m_DamageInfo);
            //ApplyBuff()
        }
    }
    protected virtual void ForceExecute() {
        if(m_Collider) m_Collider.enabled = false;
        IsDealDamage = true;
        Destroy();
    }
    protected virtual void EffectImpact(Transform transform){
        Vector3 pos = Transform.position;
        if (IsEndEffectOnTarget) {
            pos = transform.position + new Vector3(0, 0.5f, 0);
        }
        if(m_EndEffect != "")  IngameManager.Ins.PutEffectImpact(m_EndEffect, pos);
    }
    public virtual void Destroy() {
        Reset();
        PrefabManager.Instance.DespawnPool(gameObject);
    }
    public override void Reset() {
        m_LifeTime = 2;
        m_Buffes.Clear();
    }
    public override bool IsOutCamera() {
        if(Camera.main.WorldToViewportPoint(transform.position).y >= 1 ||
                          Camera.main.WorldToViewportPoint(transform.position).y <= 0f ||
                           Camera.main.WorldToViewportPoint(transform.position).x >= 0.8f ||
                           Camera.main.WorldToViewportPoint(transform.position).x <= 0f)
            return true;
        return false;
    }
}
