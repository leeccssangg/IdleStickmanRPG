using UnityEngine;

public class Quest<T> : Observer<T>
{
    public int id;
    public int qt;
    public int cl;
    public int tgm;
    public int icd;
    public int mt;

    public virtual void Init() { }
    public virtual void Load() { }
    public override void OnNotify(T id, string info)
    {
    }
    public virtual void OnCollect(int amount)
    {
        if (cl < tgm)
        {
            cl += amount;
            Mathf.Clamp(cl, 0, tgm);
        }
    }
    public virtual void OnClaim()
    {
        icd = 1;
    }
    public virtual string GetDescription()
    {
        return "";
    }
    public virtual float GetProgress()
    {
        float c = (float)cl / (float)tgm;
        if (c >= 1) c = 1;
        return c;
    }
    public virtual string GetProgressString()
    {
        string s = "" + cl + "/" + tgm;
        return s;
    }
    public bool IsClaimed()
    {
        return icd == 1;
    }
    public virtual bool IsCompleted()
    {
        return cl >= tgm;
    }
    public virtual int GetQuestType()
    {
        return this.qt;
    }
    public virtual string ToJsonString()
    {
        string json = JsonUtility.ToJson(this);
        return json;
    }
    public static Quest<T> FromJson(string s)
    {
        return JsonUtility.FromJson<Quest<T>>(s);
    }
}