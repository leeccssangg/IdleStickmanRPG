using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Globalization;

public class AllQuestManager : Pextension.Singleton<AllQuestManager>
{
    public void Notify(MissionTarget id, string info)
    {
        DailyQuestManager.Ins.Notify(id, info);
        RockieQuestManager.Ins.Notify(id, info);
    }
}
