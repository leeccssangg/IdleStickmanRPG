using UnityEngine;
using System.Collections;
using System;

public delegate void RefillCallback();
public class TimeRefillUnit {
    /// <summary>
    /// Time Refill
    /// </summary>
    public int tr = 20 * 60; // 20 minutes
    /// <summary>
    /// Max Value
    /// </summary>
    public int mv = 1;
    /// <summary>
    /// Current Value
    /// </summary>
    public int cv = 10;
    /// <summary>
    /// Default Value
    /// </summary>
    public int dv = 10;
    /// <summary>
    /// Last Time Change
    /// </summary>
    public DateTime ltc = DateTime.Now;
    /// <summary>
    /// Time to next Add
    /// </summary>
    public int tna = 0;
    /// <summary>
    /// Key Value
    /// </summary>
    public string kv = "LifeTime";
    /// <summary>
    /// Key Time
    /// </summary>
    public string kt = "LastTimeAdd";
    /// <summary>
    /// Event Trigger Name
    /// </summary>
    public string etn = "UpdateLife";
    /// <summary>
    /// Fullfill trigger
    /// </summary>
    public string fft = "UpdateLife";

    private RefillCallback m_RefillCallback = null;
    public TimeRefillUnit() {

    }
    public TimeRefillUnit(int refillTime, int maxValue, int defaultValue, string key, string eventTriggerName, string fullfillEventTriggerName = "") {
        tr = refillTime;
        mv = maxValue;
        cv = defaultValue;
        dv = defaultValue;
        kv = key;
        kt = "LastTimeAdd" + key;
        etn = eventTriggerName;
        fft = fullfillEventTriggerName;
    }
    public void Setup(int refillTime, int maxValue, int defaultValue, string key, string eventTriggerName, string fullfillEventTriggerName = "") {
        tr = refillTime;
        mv = maxValue;
        dv = defaultValue;
        kv = key;
        kt = "LastTimeAdd" + key;
        etn = eventTriggerName;
        fft = fullfillEventTriggerName;
    }
    public void InitNew() {
        cv = dv;
    }
    public void Reload() {

    }
    public void Load() {
        cv = PlayerPrefs.GetInt(kv, dv);
        string now = DateTime.Now.ToString();
        ltc = DateTime.Parse(PlayerPrefs.GetString(kt, now));
        PreloadData();
    }
    void PreloadData() {
        DateTime dateTimeNow = DateTime.Now;
        TimeSpan span = dateTimeNow - ltc;
        double totalSecs = span.TotalSeconds;
        while (cv < mv && totalSecs >= tr) {
            cv++;
            totalSecs -= tr;
            ltc += TimeSpan.FromSeconds(tr);
            Save();
            if (m_RefillCallback != null) m_RefillCallback();
        }
        if (cv < mv) {
            int val = tr - (int)totalSecs;
            if (tna != val) {
                tna = val;
            }
        }
    }
    public void Save() {
        PlayerPrefs.SetInt(kv, cv);
        PlayerPrefs.SetString(kt, ltc.ToString());
    }
    public void Reset() {
        PlayerPrefs.SetInt(kv, dv);
        cv = PlayerPrefs.GetInt(kv, dv);
        string now = DateTime.Now.ToString();
        PlayerPrefs.SetString(kt, now);
        ltc = DateTime.Parse(PlayerPrefs.GetString(kt, now));
    }
    void UpdateLastTimeChanged() {
        ltc = DateTime.Now;
    }
    public bool Add(int amount) {
        cv += amount;
        if (cv >= mv) {
            EventManager.TriggerEvent(fft);
        }
        EventManager.TriggerEvent(etn);
        return true;
    }
    public void Consume(int amount = 1) {
        if (cv == mv) {
            UpdateLastTimeChanged();
        }
        cv-=amount;
        if (cv < 0) {
            cv = 0;
        }
        EventManager.TriggerEvent(etn);
        Save();
    }
    public void SetupRefillTime(int refillTime) {
        tr = refillTime;
        Save();
    }
    public void SetRefillCallback(RefillCallback callback) {
        m_RefillCallback = callback;
    }
    public int GetCurrentValue() {
        return cv;
    }
    public string GetTimeToNextAdd(int type, string _defValue) {
        if (type == 3) {
            string ret = _defValue;
            if (cv < mv) {
                int min = tna / 60;
                int sec = tna % 60;
                int hour = min / 60;
                min = min % 60;
                ret = (hour >= 10 ? hour.ToString() : "0" + hour.ToString()) + ":" + (min >= 10 ? min.ToString() : "0" + min.ToString()) + ":" + (sec >= 10 ? sec.ToString() : "0" + sec.ToString());
            }
            return ret;
        } else if (type == 2) {
            string ret = _defValue;
            if (cv < mv) {
                int min = tna / 60;
                int sec = tna % 60;
                ret = (min >= 10 ? min.ToString() : "0" + min.ToString()) + ":" + (sec >= 10 ? sec.ToString() : "0" + sec.ToString());
            }
            return ret;
        } else {
            return null;
        }
    }
    
    public void Update() {
        if (cv < mv) {
            DateTime now = DateTime.Now;
            TimeSpan span = now - ltc;
            double totalSecs = span.TotalSeconds;
            while (cv < mv && totalSecs >= tr) {
                cv++;
                totalSecs -= tr;
                ltc += TimeSpan.FromSeconds(tr);
                EventManager.TriggerEvent(etn);
                Save();
                if (m_RefillCallback != null) m_RefillCallback();
            }
            if (cv < mv) {
                int val = tr - (int)totalSecs;
                if (tna != val) {
                    tna = val;
                }
            } else {
                tna = 0;
            }
        }
    }
}
public class MiniTimeRefill {
    /// <summary>
    /// Time to Add
    /// </summary>
    public int ta = 20 * 60; // 20 minutes

    /// <summary>
    /// Current Value
    /// </summary>
    public int cv = 1;
    /// <summary>
    /// LastTime Change
    /// </summary>
    public DateTime ltc = DateTime.Now;
    /// <summary>
    /// Time To Next Add
    /// </summary>
    public int tta = 0;
    /// <summary>
    /// Key Time
    /// </summary>
    public string kt = "";
    /// <summary>
    /// Key Value
    /// </summary>
    public string kv = "";

    private string triggerName = "";
    private int maxValue = 1;
    private RefillCallback m_RefillCallback = null;
    public MiniTimeRefill() {

    }
    public MiniTimeRefill(int refillTime, int defaultValue, string key) {
        ta = refillTime;
        cv = defaultValue;
        kv = key;
        kt = "LTA" + key;
        maxValue = 1;
    }
    public void InitNew() {
        cv = 1;
    }
    public void Setup(int refillTime, string key) {
        ta = refillTime;
        kv = key;
        kt = "LTA" + key;
        maxValue = 1;
    }
    public void Load() {
        cv = PlayerPrefs.GetInt(kv, 1);
        PreloadData();
    }
    void PreloadData() {
        DateTime dateTimeNow = DateTime.Now;
        TimeSpan span = dateTimeNow - ltc;
        double totalSecs = span.TotalSeconds;
        while (cv < 1 && totalSecs >= ta) {
            cv++;
            totalSecs -= ta;
            ltc += TimeSpan.FromSeconds(ta);
            Save();
            if (m_RefillCallback != null)
                m_RefillCallback();
        }
        if (cv < maxValue) {
            int val = ta - (int)totalSecs;
            if (tta != val) {
                tta = val;
            }
        }
    }
    public void Save() {
        PlayerPrefs.SetInt(kv, cv);
        PlayerPrefs.SetString(kt, ltc.ToString());
    }
    public void Reset() {
        PlayerPrefs.SetInt(kv, 1);
        cv = PlayerPrefs.GetInt(kv, 1);
        string now = DateTime.Now.ToString();
        PlayerPrefs.SetString(kt, now);
        ltc = DateTime.Parse(PlayerPrefs.GetString(kt, now));
    }
    void UpdateLastTimeChanged() {
        ltc = DateTime.Now;
    }
    public bool Add(int amount, bool isCapMax = false) {
        cv += amount;
        if (cv >= maxValue) {
            if (isCapMax) {
                cv = maxValue;
            }
        }
        EventManager.TriggerEvent(triggerName);
        return true;
    }
    public void AddToMax() {
        if (cv < maxValue) {
            cv = maxValue;
        }
    }
    public void Consume(int amount = 1) {
        if (cv < amount)
            return;
        if (cv == maxValue) {
            UpdateLastTimeChanged();
        }
        cv -= amount;
        if (cv < 0) {
            cv = 0;
        }
        Save();
    }
    public void SetTriggerName(string name) {
        triggerName = name;
    }
    public void ResetTime() {
        UpdateLastTimeChanged();
        Save();
    }
    public void SetupRefillTime(int refillTime, bool isSave) {
        ta = refillTime;
        if (isSave) {
            Save();
        }
    }
    public void SetRefillCallback(RefillCallback callback) {
        m_RefillCallback = callback;
    }
    public int GetCurrentValue() {
        return cv;
    }
    public int GetMaxValue() {
        return maxValue;
    }
    public bool IsMaxValue() {
        return cv >= maxValue;
    }
    public string GetCurrentValueProgress() {
        return cv + "/" + maxValue;
    }
    public string GetTimeToNextAdd(int type, string _defValue) {
        switch (type) {
            case 2: {
                    string ret = _defValue;
                    if (cv < maxValue) {
                        int min = tta / 60;
                        int sec = tta % 60;
                        ret = (min >= 10 ? min.ToString() : "0" + min.ToString()) + "M:" + (sec >= 10 ? sec.ToString() : "0" + sec.ToString() + "S");
                    }
                    return ret;
                }
            case 3: {
                    string ret = _defValue;
                    if (cv < maxValue) {
                        int min = tta / 60;
                        int sec = tta % 60;
                        int hour = min / 60;
                        min = min % 60;
                        ret = (hour >= 10 ? hour.ToString() : "0" + hour.ToString()) + "H:" + (min >= 10 ? min.ToString() : "0" + min.ToString()) + "M:" + (sec >= 10 ? sec.ToString() : "0" + sec.ToString()) + "S";
                        ;
                    }
                    return ret;
                }
            case 4: {
                    string ret = _defValue;
                    if (cv <= maxValue) {
                        int min = tta / 60;
                        int sec = tta % 60;
                        int hour = min / 60;
                        int day = hour / 24;
                        min = min % 60;
                        hour = hour % 24;
                        ret = (day >= 10 ? day.ToString() : "0" + day.ToString()) + "D:" + (hour >= 10 ? hour.ToString() : "0" + hour.ToString()) + "H:" + (min >= 10 ? min.ToString() : "0" + min.ToString()) + "M:" + (sec >= 10 ? sec.ToString() : "0" + sec.ToString()) + "S";
                    }
                    return ret;
                }
            case 5: {
                    string ret = _defValue;
                    if (cv < maxValue) {
                        int min = tta / 60;
                        int sec = tta % 60;
                        int hour = min / 60;
                        min = min % 60;
                        ret = (hour >= 10 ? hour.ToString() : "0" + hour.ToString()) + ":" + (min >= 10 ? min.ToString() : "0" + min.ToString()) + ":" + (sec >= 10 ? sec.ToString() : "0" + sec.ToString());
                    }
                    return ret;
                }
            default: {
                    return "";
                }
        }

    }
    public double GetTimeToFullfill() {
        int current = GetCurrentValue();
        int max = GetMaxValue();
        if (current >= max) {
            return 0;
        }
        double num = tta;
        int num1 = GetCurrentValue() + 1;
        if (num1 < max) {
            double num2 = (max - num1) * ta;
            num += num2;
        }
        return num;
    }
    public void Update() {
        if (cv < maxValue) {
            DateTime now = DateTime.Now;
            TimeSpan span = now - ltc;
            double totalSecs = span.TotalSeconds;
            while (cv < maxValue && totalSecs >= ta) {
                cv++;
                EventManager.TriggerEvent(triggerName);
                totalSecs -= ta;
                ltc += TimeSpan.FromSeconds(ta);
                Save();
                if (m_RefillCallback != null)
                    m_RefillCallback();
            }
            if (cv < maxValue) {
                int val = ta - (int)totalSecs;
                if (tta != val) {
                    tta = val;
                }
            } else {
                tta = 0;
            }
        }
    }
}