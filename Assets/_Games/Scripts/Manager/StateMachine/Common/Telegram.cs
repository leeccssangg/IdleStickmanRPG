using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Telegram : Object {
    public int Sender;
    public int Receiver;

    public MessageTypes Msg;
    public float DispatchTime;

    public Object ExtraObject;
    public Vector3 ExtraVector;

    public Telegram() {
        DispatchTime = -1;
        Sender = -1;
        Receiver = -1;
        Msg = MessageTypes.Msg_None;
    }
    public Telegram(float time, int receiver, MessageTypes msgType, Object extraInfo, Vector3 extraVector) {
        DispatchTime = time;
        Receiver = receiver;
        Msg = msgType;
        ExtraObject = extraInfo;
        ExtraVector = extraVector;
    }
    public Telegram(float time, int sender, int receiver, MessageTypes msgType, Object extraInfo, Vector3 extraVector) {
        DispatchTime = time;
        Sender = sender;
        Receiver = receiver;
        Msg = msgType;
        ExtraObject = extraInfo;
        ExtraVector = extraVector;
    }

    public float SmallestDelay = 0.25f;
    public bool equals(Object o) {
        if (o.GetType() != typeof(Telegram)) return false;
        Telegram t1 = this;
        Telegram t2 = (Telegram)o;
        return (Mathf.Abs(t1.DispatchTime - t2.DispatchTime) < SmallestDelay)
            && (t1.Sender == t2.Sender)
            && (t1.Receiver == t2.Receiver)
            && (t1.Msg == t2.Msg);
    }

    public int compareTo(Object o) {
        Telegram t1 = this;
        Telegram t2 = (Telegram)o;
        if (t1 == t2) {
            return 0;
        } else {
            return (t1.DispatchTime < t2.DispatchTime) ? -1 : 1;
        }
    }
}
public class TelegramComparer : IComparer<Telegram> {
    public int Compare(Telegram t1, Telegram t2) {
        if (t1 == null) {
            if (t2 == null) {
                return 0;
            } else {
                return -1;
            }
        } else {
            if (t2 == null) {
                return 1;
            } else {
                if (t1 == t2) {
                    return 0;
                } else {
                    return t1.compareTo(t2);
                }
            }
        }
    }
}
