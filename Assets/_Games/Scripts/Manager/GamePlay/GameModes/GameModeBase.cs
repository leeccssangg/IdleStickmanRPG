using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public enum GameMode{
    NONE = 0,
    CAMPAIGN = 1,
    BOSS_RUSH = 2,
    GOLD_RUSH = 3,
    MONSTER_NEST = 4,
    CHALLENGE_KING = 5,
}
[System.Serializable]
public class GameModeBase : MonoBehaviour{ 
    [FormerlySerializedAs("gameMode")]
    [SerializeField]
    protected GameMode m_GameMode;
    [SerializeField]protected GameModeConfig m_Config;
    
    protected bool m_IsRunning;
    protected bool m_IsGenerateDone;
    
    protected IngameManager InGameManager => IngameManager.Ins;
    protected DungeonManager DungeonManager => DungeonManager.Ins;
    
    public virtual void Init(){
        //m_Config = GameModesConfig.Instance.GetGameModeConfig(m_GameMode);
        m_IsRunning = false;
    }
    public virtual void OnRunning(){ }
    public virtual void StartBattle(){
        m_IsRunning = true;
        m_IsGenerateDone = false;
    }
    public virtual void StopBattle(){
        OutBattle();
    }
    protected bool IsAllEnemyDead(){
        var enemies = new List<Character>(InGameManager.Enemies);
        return enemies.All(enemy => enemy.IsDead());
    }
    
    public virtual void OutBattle(){
        m_IsRunning = false;
        m_IsGenerateDone = true;
    }
    public virtual int GetMaxProcess(){
        return m_Config.wave;
    }
    public virtual void MarkLose(){ }
}