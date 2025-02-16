using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData:Datastring {
    public Rarity rarity;
    public virtual void Load() {
    }
    public virtual void FromJson(string json) {
    }

    public virtual string ToJson() {
        return "";
    }
}
public interface Datastring {
    public void FromJson(string json);
    public string ToJson();
}
