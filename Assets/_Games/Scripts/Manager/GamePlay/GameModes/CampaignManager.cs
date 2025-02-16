using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public enum CampaignMode{
    NORMAL_FIGHT,
    BOSS_FIGHT,
}
public enum DifficultyLevel{
    Normal,
    Hard,
    Very_Hard,
    Hell_I,
    Hell_II,
    Hell_III,
    Cocytus_I,
    Cocytus_II,
    Cocytus_III,
    Heaven_I,
    Heaven_II,
    Heaven_III,
    Extreme,
    Merciless,
    Kinglike,
    Godlike,
    True_Slime,
}
public class CampaignManager : GameModeBase{

    [SerializeField]
    private  CampaignMode m_CurrentMode;
    
    public CampaignLevelConfig m_LevelConfig;
    private InGameLevel m_InGameLevel;
    private int m_CurrentLevel;
    private bool m_IsStarWave;
    private float m_SpawnRate = 0.6f;
    
    [SerializeField]private float m_TimeToNextWave = 1;
    [SerializeField] private float m_TimeToNextWaveCountDown;
    private void Update(){
        if(m_TimeToNextWaveCountDown > 0){
            m_TimeToNextWaveCountDown-= Time.deltaTime;
            if(m_TimeToNextWaveCountDown <= 0){
                NormalFight();
            }
        }
    }
    public override void Init(){
        m_LevelConfig = CampaignLevelConfig.Instance;
        m_InGameLevel = ProfileManager.PlayerData.LevelData;
    }
    public override void StartBattle(){
        // m_CurrentMode = CampaignMode.NORMAL_FIGHT;
        // StartNextWave();
        PrepareNextWave();
    }
    private void PrepareNextWave(){
        if(m_InGameLevel.w >= 5 && !m_InGameLevel.IsBossWaitting()){
            m_CurrentMode = CampaignMode.BOSS_FIGHT;
        } else{
            m_CurrentMode = CampaignMode.NORMAL_FIGHT;
        }
        StartNextWave();
    }
    private void StartNextWave(){
        StopAllCoroutines();
        m_IsStarWave = false;
        m_IsGenerateDone = false;
        InGameManager.Background.SetBackground();
        UIManager.Ins.GetUI<UIPanelIngame>().SetupUILevel();
        switch(m_CurrentMode){
            case CampaignMode.NORMAL_FIGHT:
                m_TimeToNextWaveCountDown = 0.5f;
                break;
            case CampaignMode.BOSS_FIGHT:
                BossFight();
                break;
            default:
                break;
        }
    }
    private void NormalFight(){
        if(m_IsStarWave) return;
        m_IsStarWave = true;
        InGameManager.SetNextRound();
        InGameManager.StartNewRound();
        GenerateNextNormalWave();
    }
    public void BossFight(){
        if(m_IsStarWave) return;
        m_IsStarWave = true;
        InGameManager.StopHeroAttack();
        InGameManager.StopSkillAttack();
        m_InGameLevel.SetWaittingBoss(false);
        StartCoroutine(CO_BossFight());
    }
    private IEnumerator CO_BossFight(){
        yield return Yielders.Get(1f);
        IngameManager.ShowDarkPanel(2);
        IngameManager.ShowBossPanel();
        yield return Yielders.Get(1f);
        InGameManager.ResetBattle();
        InGameManager.ClearEnemy();
        yield return Yielders.Get(1f);
        InGameManager.SetNextRound();
        InGameManager.StartNewRound();
        yield return Yielders.Get(1.2f);
        GenerateBossWave();
    }
    private IEnumerator CO_StartBossWave(){
        yield return Yielders.Get(0.5f);
        GenerateBossWave();
    }
    public void GenerateNextNormalWave(){
        StartCoroutine(CO_GeneratingEnemy());
    }
    private IEnumerator CO_GeneratingEnemy(){
        int count = 1;
        if(m_InGameLevel.d == 0){
            if(m_InGameLevel.l <= 1){
                int s = m_InGameLevel.s;
                if(s is 1 or 2){
                    count = 1;
                } else if(s is 3 or 4){
                    count = 2;
                } else if(s == 5){
                    count = 3;
                } else if(s is 6 or 7){
                    count = 4;
                } else if(s is 8 or 9){
                    count = 5;
                } else
                    count = 6;
            } else{
                count = 8;
            }
        } else count = 9;

        BigNumber enemyHp = FormulaManager.Ins.GetEnemyHP(m_InGameLevel.lv);
        BigNumber enemyDamage = FormulaManager.Ins.GetEnemyAttack(m_InGameLevel.lv);
        m_SpawnRate = 0;
        for(int i = 0; i < count; i++){
            yield return Yielders.Get(m_SpawnRate);
            string eName = GetEnemyName(count);
            Enemy enemy = PrefabManager.Instance.SpawnEnemy(eName, transform.position);
            var platform = enemy.Platform;
            Vector3 pos = InGameManager.PositionManager.GetEnemySpawnPos(platform);
            enemy.Transform.position = pos;
            enemy.InitEnemy();
            enemy.SetTeam(IngameTeam.Team2);
            enemy.InitHP(enemyHp / count);
            enemy.SetDamage(enemyDamage / count);
            enemy.BattleCry();
            enemy.Fight();
            enemy.transform.localScale = Vector3.one;
            InGameManager.Enemies.Add(enemy);
            // m_SpawnRate = Random.Range(0.2f, 0.6f);
            m_SpawnRate = Random.Range(0.1f, 0.3f);
        }
        m_IsGenerateDone = true;
    }
    public string GetEnemyName(int enemyCount){
        List<string> enemyConfigList = m_LevelConfig.GetWaveConfig(m_InGameLevel).enemyList;
        int count = enemyConfigList.Count;
        return enemyCount <= 2 ? enemyConfigList[0] : enemyConfigList[Random.Range(0, count)];
    }
    public void GenerateBossWave(){
        BigNumber enemyHp = FormulaManager.Ins.GetEnemyHP(m_InGameLevel.lv);
        BigNumber enemyDamage = FormulaManager.Ins.GetEnemyAttack(m_InGameLevel.lv);
        m_IsGenerateDone = false;
        Enemy enemy = PrefabManager.Instance.SpawnEnemy(GetEnemyName(1) + "_Boss", transform.position);
        var platform = enemy.Platform;
        Vector3 pos = InGameManager.PositionManager.GetEnemySpawnPos(platform);
        enemy.Transform.position = pos;
        InGameManager.Enemies.Add(enemy);
        enemy.BattleCry();
        enemy.SetTeam(IngameTeam.Team2);
        enemy.InitEnemy();
        enemy.Fight();
        enemy.InitHP(enemyHp);
        enemy.SetDamage(enemyDamage * 2);
        m_IsGenerateDone = true;
    }
    public override void MarkLose(){
        StopBattle();
        m_IsStarWave = false;
        switch(m_InGameLevel.wt){
            case WaveType.Normal:
                m_InGameLevel.Lose();
                break;
            case WaveType.Boss:
                m_InGameLevel.SetWaveType(WaveType.Normal);
                m_InGameLevel.SetWaittingBoss(true);
                break;
            default:
                break;
        }
    }
    public void Win(){
        if(!m_IsStarWave) return;
        m_IsStarWave = false;
        m_InGameLevel.PassWave();
        InGameManager.StopHeroAttack();
        ProfileManager.Ins.SaveLevelData(m_InGameLevel);
        StartCoroutine(CO_Win());
    }
    private IEnumerator CO_Win(){
        if(m_CurrentMode == CampaignMode.BOSS_FIGHT){
            UIManager.Ins.GetUI<UIPanelIngame>().ShowWinImage((true));
            yield return Yielders.Get(1.5f);
            IngameManager.ShowDarkPanel(2);
            yield return Yielders.Get(1.8f);
            InGameManager.ResetBattle();
            InGameManager.ClearEnemy();
            m_CurrentMode = CampaignMode.NORMAL_FIGHT; 
            PrepareNextWave();
            yield break;
        }
        PrepareNextWave();
    }
    public override void StopBattle(){
        m_TimeToNextWaveCountDown = 0;
        m_IsStarWave = false;
        StopAllCoroutines();
    }
    public void OnBossFight(){
        if(InGameManager.CurrentGameMode != GameMode.CAMPAIGN) return;
        m_IsStarWave = false;
        m_CurrentMode = CampaignMode.BOSS_FIGHT;
        InGameManager.StopEnemyToAttack();
        InGameManager.EndRound();
        m_InGameLevel.SetWaveType(WaveType.Boss);
        StartNextWave();
    }
    public bool IsGoodToWin(){
        return m_IsGenerateDone && IsAllEnemyDead();
    }
}
[System.Serializable]
public class InGameLevel{
    public int lv = 1;

    /// <summary>
    /// Difficulty
    /// </summary>
    public int d = 0;

    /// <summary>
    /// Level
    /// </summary>
    public int l = 1;

    /// <summary>
    /// Stage
    /// </summary>
    public int s = 1;

    /// <summary>
    /// Wave
    /// </summary>
    public int w = 0;

    /// <summary>
    /// Boss Waiting
    /// </summary>
    public int bw = 0;

    /// <summary>
    /// Wave Type
    /// </summary>
    public WaveType wt = WaveType.Normal;

    public void Init(int diff, int level, int stage, int wave){ }
    public void SetWaveType(WaveType waveType){
        wt = waveType;
    }
    public void PassLevelDifficulty(){
        d++;
        l = 1;
        s = 1;
        w = 1;
        bw = 0;
        wt = WaveType.Normal;
    }
    public void PassLevel(){
        if(l < 10){
            l++;
            s = 1;
            w = 1;
            bw = 0;
            wt = WaveType.Normal;
        } else{
            PassLevelDifficulty();
        }
    }
    public void PassSatge(){
        lv++;
        if(s < 10){
            s++;
            w = 1;
            bw = 0;
            wt = WaveType.Normal;
        } else{
            PassLevel();
        }
    }
    public void PassWave(){
        if(w < 5){
            w++;
            if(w == 5){
                wt = WaveType.Boss;
            }
        } else{
            if(wt == WaveType.Boss){
                PassSatge();
            }
        }
    }
    public void Lose(){
        if(!IsBossWaitting()) w = 0; 
        if(lv > 0) lv--;
        if(s > 1){
            s--;
        } else{
            if(l > 1){
                s = 10;
                l--;
            } else{
                if(d > 0){
                    l = 10;
                    d--;
                } else{
                    l = 1;
                    s = 1;
                }
            }
        }
    }
    public void SetWaittingBoss(bool isWaiting){
        bw = isWaiting ? 1 : 0;
    }
    public bool IsBossWaitting(){
        return bw == 1;
    }
    public bool IsLevel(int dif, int level, int stage){
        return d == dif && level == l && s == stage;
    }
}