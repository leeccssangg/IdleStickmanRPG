using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyTimeCount {
    public string GetNextDayRemainTime() {
        DateTime now = DateTime.Now;
        DateTime nowTrimmed = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
        DateTime nextDayTrimmed = nowTrimmed.AddDays(1);
        TimeSpan ts = nextDayTrimmed - now;
        double totalSeconds = ts.TotalSeconds;
        return Utilss.GetTimeStringFromSecond(totalSeconds);
    }
}