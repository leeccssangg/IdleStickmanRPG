using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class BossRushMode : GameModeBase{
    private int m_CurLevel;
    private int m_CurrentWave;
    private float m_TimeLeft;
    
    private float m_CountTime;
    public BossRushMode(){
        m_GameMode = GameMode.BOSS_RUSH;
    }
    public int CurrentWave{ get => m_CurrentWave; set => m_CurrentWave = value; }
    public float TimeLeft{ get => m_TimeLeft; set => m_TimeLeft = value; }
    public override void OnRunning(){
        base.OnRunning();
        if(!m_IsRunning) return;
        if(TimeLeft > 0){
            TimeLeft -= Time.deltaTime;
        } else{
            InGameManager.Win();
        }
        if(m_CountTime > 0){
            m_CountTime -= Time.deltaTime;
            if(m_CountTime <= 0){
                GenerateEnemyBoss();
            }
        }
    }
    public override void StartBattle(){
        m_IsRunning = true;
        TimeLeft = m_Config.timePlay;
        CurrentWave = 1;
        m_CurLevel = DungeonManager.Ins.GetDungeon(DungeonType.BOSS_RUSH).GetCurrentLevel();
        base.StartBattle();
        StartWave(2);

    }
    private void StartWave(float time){
        m_IsGenerateDone = false;
        InGameManager.SetNextRound();
        InGameManager.StartNewRound();
        m_CountTime = time;
    }
    private void GenerateEnemyBoss(){
        if(m_IsGenerateDone) return;
        string bossName = "Enemy_W1_Boss";
        BigNumber enemyHp = FormulaManager.Ins.GetEnemyHP(5);
        enemyHp = (CurrentWave) * 1000;
        BigNumber enemyDamage = FormulaManager.Ins.GetEnemyAttack(5);
        
        Enemy enemy = PrefabManager.Instance.SpawnEnemy(bossName);
        var platform = enemy.Platform;
        Vector3 pos = InGameManager.PositionManager.GetEnemySpawnPos(platform);
        enemy.Transform.position = pos;
        InGameManager.Enemies.Add(enemy);
        enemy.BattleCry();
        enemy.SetTeam(IngameTeam.Team2);
        enemy.InitEnemy();
        enemy.Fight();
        enemy.InitHP(7000);
        enemy.SetDamage(enemyDamage * 2);
        m_IsGenerateDone = true;
    }
    public void MarkKillEnemy(){
        if(m_IsRunning){
            if(IsGoodToNextWave()){
                EndWave();
                StartWave(0.1f);
            }
        }
    }
    private void EndWave(){
        CurrentWave++;
    }
    public void Win(){
        m_IsRunning = false;
        InGameManager.StopHeroAttack();
        UIManager.Ins.GetUI<UIPanelIngame>().ShowWinImage((true));
        DungeonManager.Ins.DungeonComplete(DungeonType.BOSS_RUSH);
    }
    public override void MarkLose(){
        m_IsRunning = false;
        UIManager.Ins.GetUI<UICDungeon>().SetInteractableButtonRunAway(false);
    }
    public bool IsGoodToNextWave(){
        if(m_IsRunning){
            return m_IsGenerateDone && IsAllEnemyDead();
        }
        return false;
    }
    public bool IsGoodToWin(){
        if(m_IsRunning){
            return CurrentWave == 10 || TimeLeft <= 0;
        }
        return false;
    }
    public override void OutBattle(){
        base.OutBattle();
        m_IsRunning = false;
    }
   
#region GET_SET
    public int GetCurLevel(){
        return m_CurLevel;
    }
#endregion
}