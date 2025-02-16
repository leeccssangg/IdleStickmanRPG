using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PlayState{
    NORMAL_FIGHT = 0,
    BOSS_FIGHT = 1
}
public class IngameManager : SingletonFree<IngameManager>{
    [SerializeField] private CampaignManager m_CampaignCampaign;
    [SerializeField] private HeroManager m_HeroManager;
    [SerializeField] private BackgroundManager m_BackgroundManager;
    [SerializeField] private PositionManager m_PositionManager;
    [SerializeField] private CameraManger m_CameraManager;
    [SerializeField] private BossRushMode m_BossRushMode;
    private PlayState m_CurrentPlayState;

#region PROPERTY
    public GameMode CurrentGameMode{ get; set; }
    public PositionManager PositionManager{ get => m_PositionManager; set => m_PositionManager = value; }
    public CampaignManager Campaign{ get => m_CampaignCampaign; set => m_CampaignCampaign = value; }
    public BackgroundManager Background{ get => m_BackgroundManager; set => m_BackgroundManager = value; }
    public MainCharacter MainCharacter => Hero.MainCharacter;
    public HeroManager Hero{ get => m_HeroManager; set => m_HeroManager = value; }
    private SkillManager SkillManager => PropertiesManager.Ins.SkillManager;
    public List<Character> Enemies{ get; private set; }
    public bool IsEndMatch{ get; private set; } = true;
    public GameModeBase CurrentGamePlayMode{ get; private set; }
#endregion
    private void Start()
    {
        InitGame();
    }
    private void InitProperties()
    {
        PropertiesManager.Ins.Init();
    }
    private void InitUI()
    {
        UIManager.Ins.OpenUI<UIPanelPlayerStat>();
        UIManager.Ins.OpenUI<UIPanelIngame>();
        UIManager.Ins.OpenUI<UISkillPanel>();
        UIManager.Ins.OpenUI<UIPanelMainButton>();
    }
    private void InitGame(){
        InitProperties();
        InitUI();
        InitGameMode();
        ResetAllPosition();
        Enemies = new List<Character>();
        Background.Init();
        SkillManager.LoadLineupSkill();
        Hero.InitMainCharacter();
        StartCampaign();
    }
    private void InitGameMode(){
        Campaign.Init();
        m_BossRushMode.Init();
    }
    private void Update(){
        SkillManager.OnSkillRunning();
        if(CurrentGamePlayMode){
            CurrentGamePlayMode.OnRunning();
        }
    }

#region StartBattle
    private void StartGame(){
        InitGame();
        
    }
    private void StartCampaign(){
        StartBattle(GameMode.CAMPAIGN);
    }
    public void StartBossRush(){
        StartBattle(GameMode.BOSS_RUSH);
    }
    private void StartBattle(GameMode gameMode){
        StopAllCoroutines();
        IsEndMatch = true;
        if(CurrentGamePlayMode) CurrentGamePlayMode.StopBattle();
        CurrentGameMode = gameMode;
        if(gameMode != GameMode.CAMPAIGN){
            UIManager.Ins.SetActiveMaintab(false);
        }
        StartCoroutine(CO_StartBattle());
    }
    private IEnumerator CO_StartBattle(){
        float changeTime = CurrentGameMode == GameMode.CAMPAIGN ? 1f : 0.5f;
        ShowDarkPanel(1f);
        yield return Yielders.Get(changeTime * 0.7f);
        ResetBattle();
        ClearEnemy();
        yield return Yielders.Get(changeTime* 0.3f);
        StartNewRound();
        switch(CurrentGameMode){
            case GameMode.NONE:
            case GameMode.CAMPAIGN:
                CurrentGamePlayMode = m_CampaignCampaign;
                break;
            case GameMode.BOSS_RUSH:
                CurrentGamePlayMode = m_BossRushMode;
                break;
            case GameMode.GOLD_RUSH:
            case GameMode.MONSTER_NEST:
            case GameMode.CHALLENGE_KING:
            default:
                break;
        }
        CurrentGamePlayMode.StartBattle();
        UIManager.Ins.GetUI<UIPanelIngame>().SetupStartBattle(CurrentGameMode);
    }
#endregion

#region EXECUTE
    public void StartNewRound(){
        IsEndMatch = false;
    }
    public void EndRound(){
        IsEndMatch = true;
    }
    public void ResetBattle(){
        Hero.StartBattle();
        SkillManager.SetReadyToAttack();
        SkillManager.ContinueToAttack();
    }
    public void SetNextRound(){
        //m_PositionManager.SetNextWave();
        m_BackgroundManager.SetPlatformPosition();
        HeroMovetoNextRound();
    }
    public void HeroMovetoNextRound(){
        Hero.MoveToNextRound();
        //StartFight();
    }
    public void ResetAllPosition(){
        m_PositionManager.ResetAllPosition();
    }
    public void StopBattle(){
        StopHeroAttack();
        StopEnemyToAttack();
        StopSkillAttack();
    }
    public void StopHeroAttack(){
        Hero.HoldingBattle();
    }
    public void StopSkillAttack(){
        SkillManager.StopToAttacking();
    }
    public void HoldingBattleWhenPlayerDie(){
        m_HeroManager.StickmanStopAttack();
        StopSkillAttack();
        StopEnemyToAttack();
    }
    public void StopEnemyToAttack(){
        for(int i = 0; i < Enemies.Count; i++){
            var enemy = (Enemy)Enemies[i];
            enemy.WaitState();
        }
    }
    public void MarkKillCharacter(IngameTeam ingameTeam){
        if(IsGoodToWin()){
            Win();
            return;
        }
        switch(CurrentGameMode){
            case GameMode.NONE:
                break;
            case GameMode.CAMPAIGN:
                break;
            case GameMode.BOSS_RUSH:
                m_BossRushMode.MarkKillEnemy();
                break;
            case GameMode.GOLD_RUSH:
                break;
            case GameMode.MONSTER_NEST:
                break;
            case GameMode.CHALLENGE_KING:
                break;
            default:
                break;
        }
    }
    [Button]
    public void OutCurrentBattle(){
        if(CurrentGamePlayMode == null) return;
        CurrentGamePlayMode.OutBattle();
        StopBattle();
        StartBattle(GameMode.CAMPAIGN);
        CurrentGamePlayMode = null;
    }
    public void ClearEnemy(){
        if(Enemies.Count <= 0) return;
        Debug.Log(Enemies.Count);
        for(int i = Enemies.Count - 1; i >= 0; i--){
            var enemy = Enemies[i];
            enemy.Reset();
            enemy.Deactive();
            Enemies.Remove(enemy);
        }
        ClearBullet();
    }
#endregion

#region WIN_LOSE
    public void Win(){
        if(IsEndMatch) return;
        if(CurrentGameMode == GameMode.CAMPAIGN){
            Campaign.Win();
        } else{
            switch(CurrentGameMode){
                case GameMode.BOSS_RUSH:
                    m_BossRushMode.Win();
                    break;
                default:
                    break;
            }

            StartCampaign();
        }

        EndRound();
    }
    private bool IsGoodToWin(){
        switch(CurrentGameMode){
            case GameMode.NONE:
                break;
            case GameMode.CAMPAIGN:
                return Campaign.IsGoodToWin();
            case GameMode.BOSS_RUSH:
                return m_BossRushMode.IsGoodToWin();
            case GameMode.GOLD_RUSH:
            case GameMode.MONSTER_NEST:
            case GameMode.CHALLENGE_KING:
            default:
                break;
        }

        return false;
    }
    public void PlayerDead(){
        if(IsEndMatch) return;
        IsEndMatch = true;
        Lose();
    }
    public void Lose(){
        if(IsEndMatch) return;
        CurrentGamePlayMode.MarkLose();
        HoldingBattleWhenPlayerDie();
        OnLose();
        EndRound();
    }
    private void OnLose(){
        StartCoroutine(CO_Lose());
    }
    private IEnumerator CO_Lose(){
        yield return Yielders.Get(2f);
        switch(CurrentGameMode){
            case GameMode.NONE:
                break;
            case GameMode.CAMPAIGN:
                break;
            case GameMode.BOSS_RUSH:
                UIManager.Ins.GetUI<UICDungeon>().ClosePanelDungeonInProgress();
                break;
            case GameMode.GOLD_RUSH:

            case GameMode.MONSTER_NEST:
            case GameMode.CHALLENGE_KING:
            default:
                break;
        }
        StartBattle(GameMode.CAMPAIGN);
    }
#endregion

#region PANEL
    public static void ShowDarkPanel(float time){
        UIManager.Ins.OpenUI<UIPanelIngame>().DarkPanelEffect(time);
    }
    public static void ShowBossPanel(){
        UIManager.Ins.OpenUI<UIPanelIngame>().ShowBossPanel();
    }
#endregion

    public void DropGoldFromEnemy(BigNumber amount, Vector3 pos){
        if(IsEndMatch) return;
        int count = Random.Range(1, 4);
        BigNumber each = amount / count;
        for(int i = 0; i < count; i++){
            GameObject go = PrefabManager.Instance.SpawnPool("GoldDrop", pos);
            ItemGoldDrop gold = go.GetComponent<ItemGoldDrop>();
            gold.Setup(each);
        }
    }
    public void UpdateStat(MainStatType stat){
        Hero.UpdateStat(stat);
    }
    public void ClearBullet(){
        List<IngameObject> bullets = IngameEntityManager.Ins.GetAllBullet();
        if(bullets.Count <= 0) return;
        for(int i = 0; i < bullets.Count; i++){
            bullets[i].Deactive();
        }
    }

#region PutEffect
    public void PutEffectImpact(string effectName, Vector3 hitPoint){
        PutEffect(hitPoint, effectName);
    }
    public Effect PutEffect(Vector3 hitPoint, string prefabName){
        if(prefabName == "None") return null;
        hitPoint.z = 0;
        GameObject go = PrefabManager.Instance.SpawnPool(prefabName, hitPoint);
        go.transform.position = hitPoint;
        return go.GetComponent<Effect>();
    }
    public void PutTextDamage(BigNumber damage, Vector3 hitPoint, DamageType dt){
        string prefabName = "Effect_Damaged";
        GameObject go = PrefabManager.Instance.SpawnPool(prefabName);
        go.transform.localPosition = hitPoint +
                                     new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f),
                                                 UnityEngine.Random.Range(0.3f, 0.6f));
        UIDamageEffect ui = go.GetComponent<UIDamageEffect>();
        ui.SetDamage(damage);
        ui.SetDamageType(dt);
    }
#endregion

    public Buff GetNewBuff(Buff buff, bool isBoss = false){
        float affectTime = buff.m_AffectedTime;
        Buff newBuff = null;
        switch(buff.BuffType){
            case BuffType.POISON:{
                PoisonBuff poisonBuff = (PoisonBuff)buff;
                newBuff = new PoisonBuff(){ damageOverTime = poisonBuff.damageOverTime };
            }
                break;
        }

        newBuff.Init(buff.BuffType, buff.Value, affectTime, buff.IsPernamentBuff, buff.IsLinkedBuff());
        newBuff.LastWordDamage = buff.LastWordDamage;
        newBuff.SetOwnerID(buff.GetOwnerID());
        newBuff.SetCreator(buff.GetCreator());
        newBuff.Direction = buff.Direction;
        return newBuff;
    }
}
public enum WaveType{
    Normal = 0,
    Boss = 1
}