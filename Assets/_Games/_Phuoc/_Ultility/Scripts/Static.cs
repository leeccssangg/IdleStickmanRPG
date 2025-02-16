using System;
using UnityEngine;

public static class Static
{
    public static int gameLaunchTime = 0;
    public static DateTime beginningOfTime = new DateTime(1970, 1, 1);

    public static int CurrentUTCTimeInSecond {
        get {
            return (int)(DateTime.UtcNow - beginningOfTime).TotalSeconds;
        }
    }

    public static int CurrentTimeInSecond {
        get {
            return (int)(DateTime.Now - beginningOfTime).TotalSeconds;
        }
    }

    public static int CurrentRealTimeInSecond {
        get {

#if UNITY_EDITOR
            int num = gameLaunchTime;
            //num = 0;
            if (num == 0) {
                DateTime now = DateTime.Now;
                return (int)(now - beginningOfTime).TotalSeconds;
            }
            return num + (int)Time.realtimeSinceStartup;
#else
            int num = gameLaunchTime;
            if (num == 0) {
                DateTime now = DateTime.Now;
                return (int)(now - beginningOfTime).TotalSeconds;
            }
            return num + (int)Time.realtimeSinceStartup;
#endif
        }
    }

    public static long CurrentTimeInMilisec {
        get {
            //if (gameLaunchTime == 0)
            return (long)((DateTime.Now - beginningOfTime).TotalMilliseconds);

            //return gameLaunchTime + (int)(Time.realtimeSinceStartup * 1000);
        }
    }

    public static long CurrentRealTimeInMilisec {
        get {
            if (gameLaunchTime == 0)
                return (long)(DateTime.Now - beginningOfTime).TotalMilliseconds;

            return gameLaunchTime + (int)(Time.realtimeSinceStartup * 1000);
        }
    }

    public static DateTime CurrentDate {
        get {
            return beginningOfTime + TimeSpan.FromSeconds(CurrentTimeInSecond);
        }
    }

    public static DateTime CurrentRealDate {
        get {
            return beginningOfTime + TimeSpan.FromSeconds(CurrentRealTimeInSecond);
        }
    }

    public static DateTime Seconds2Date(Double d) {
        TimeSpan time = TimeSpan.FromSeconds(d);
        return beginningOfTime + time;
    }
    public static long Date2Second(DateTime d) {
        return (long)(d - beginningOfTime).TotalSeconds;
    }
    public static string GetTimeStringFromSecond(double time) {
        int totalSecond = (int)time;
        int min = totalSecond / 60;
        int sec = totalSecond % 60;
        int hour = min / 60;
        min = min % 60;
        return (hour >= 10 ? hour.ToString() : "0" + hour.ToString()) + ":" + (min >= 10 ? min.ToString() : "0" + min.ToString()) + ":" + (sec >= 10 ? sec.ToString() : "0" + sec.ToString());
    }
    public static T FromJsonString<T>(string json) {
        T playerData = LitJson.JsonMapper.ToObject<T>(json);
        return playerData;
    }
    public static string ToJsonString(object obj) {
        return LitJson.JsonMapper.ToJson(obj, true, false);
    }
    public static DateTime GetNextDate() {
        DateTime now = DateTime.Now;
        DateTime temp = now.AddDays(1);
        return new DateTime(temp.Year, temp.Month, temp.Day);
    }
}
 