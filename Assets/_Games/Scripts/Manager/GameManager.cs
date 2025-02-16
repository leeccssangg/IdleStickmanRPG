using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : SingletonFreeAlive<GameManager>
{

    public List<string> m_DifficultyText;

    public string GetTextDifficulty(int diff) {
        return m_DifficultyText[diff];
    }
    
    private void OnApplicationPause(bool pause) {
        if(pause) {
            //ProfileManager.LocalData.SaveDataToLocal(false);
            ProfileManager.Ins.Save();
        } else {
            //ProfileManager.LocalData.LoadMine();
        }
    }
    private void OnApplicationQuit() {
        //ProfileManager.LocalData.SaveDataToLocal(false);
        ProfileManager.Ins.Save();
        //m_PushNotificationManager.ScheduleDailyPush();
    }
}
