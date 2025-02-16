using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HeroManager:MonoBehaviour {
    public static HeroManager Ins;
    public MainCharacter m_MainCharacter;

    [FormerlySerializedAs("m_StickmanManager")] [SerializeField]
    private StickManManager m_StickManManager;
    [SerializeField] protected float m_BonusAttackSpeed = 0;
    public MainCharacter MainCharacter { get => m_MainCharacter; set => m_MainCharacter = value; }
    public Transform PlayerTransform => m_MainCharacter.Transform;
    private void Awake(){
        if(Ins == null) Ins = this;
    }
    public void InitMainCharacter(){
        var playerTransform = IngameManager.Ins.PositionManager.PlayerTranform;
        MainCharacter.InitCharacter();
        MainCharacter.Transform.position = playerTransform.position;
        m_MainCharacter.FollowPosition = playerTransform;
        MainCharacter.RegisterInScene();
        MainCharacter.InitHealthbar();
        m_StickManManager.InitStickman();
    }
    public void StartBattle(){
        MainCharacter.Reset();
        m_StickManManager.ResetStickman();
    }
    public void UpdateStat(MainStatType statType) {
        StatData stat = StatManager.Ins.GetStatData(statType);
        MainCharacter.ExecuteUpdateStat(stat);
    }
    public void ResetMainCharacter() {
        MainCharacter.Reset();
        MainCharacter.StateMachine.ChangeState(CharacterIdleState.Instance);
    }
    public void StartFight() {
        MainCharacter.Fight();
    }
    public void MarkLose() {
        StickmanStopAttack();
    }
    public void HoldingBattle() {
        MainCharacter.StopAttack();
        StickmanStopAttack();
    }
    public void StickmanStopAttack(){
        m_StickManManager.WaitState();
    }
    
    public void ResetHero() {
        ResetMainCharacter();
    }
    public void MoveToNextRound() {
        MainCharacter.MoveToNextRound();
        m_StickManManager.MoveToNextRound();
    }
#region BONUS
    public void SetBonusAttackSpeed(float bonus) {
        m_BonusAttackSpeed = bonus;
    }
    public void SubtractBonusAttackSpeed(float bonus) {
        m_BonusAttackSpeed -= bonus;
    }
    public void AddBonusAttackSpeed(float bonus) {
        m_BonusAttackSpeed += bonus;
    }
    public float GetBonusAttackSpeed(){
        return m_BonusAttackSpeed;
    }
#endregion
    
}
