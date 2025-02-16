using System;
using Sirenix.OdinInspector;

[Serializable]
public class MonopolyJsonData
{
    [LabelText("Position")] public int p;
    [LabelText("Board Value")] public string bv;
    [LabelText("Amount Dice")] public int ad;
    [LabelText("Max Dice")] public int md;
    [LabelText("Amount Magnet")] public int am;
    [LabelText("Max Magnet")] public int mm;
    [LabelText("Amount Dice 6")] public int ad6;
    [LabelText("Max Dice 6")] public int md6;
    [LabelText("Amount Reset Daily")] public int ard;
    [LabelText("Time Roll Start")] public string trs;
    [LabelText("Time Magnet Start")] public string tms;
    [LabelText("Time Dice6 Start")] public string td6s;
    [LabelText("Time Reset Daily")] public string trd;

    public string TimeResetDaily
    {
        get => trd;
        set => trd = value;
    }

    public int Position
    {
        get => p;
        set => p = value;
    }

    public string BoardValue
    {
        get => bv;
        set => bv = value;
    }

    public int AmountDice
    {
        get => ad;
        set => ad = value;
    }

    public int MaxDice
    {
        get => md;
        set => md = value;
    }

    public int AmountMagnet
    {
        get => am;
        set => am = value;
    }
    
    public int MaxMagnet
    {
        get => mm;
        set => mm = value;
    }
    
    public int AmountDice6
    {
        get => ad6;
        set => ad6 = value;
    }

    public int MaxDice6
    {
        get => md6;
        set => md6 = value;
    }
    
    public int AmountResetDaily
    {
        get => ard;
        set => ard = value;
    }
    
    public string TimeRollStart
    {
        get => trs;
        set => trs = value;
    }

    public string TimeMagnetStart
    {
        get => tms;
        set => tms = value;
    }

    public string TimeDice6Start
    {
        get => td6s;
        set => td6s = value;
    }
    

    public MonopolyJsonData()
    {
    }
    public MonopolyJsonData(int position, string boardValue, int amountDice, int maxDice, int amountMagnet, int maxMagnet, int amountDice6, int maxDice6, int amountResetDaily, string timeRollStart, string timeMagnetStart, string timeDice6Start, string timeResetDaily)
    {
        this.p = position;
        this.bv = boardValue;
        this.ad = amountDice;
        this.md = maxDice;
        this.am = amountMagnet;
        this.mm = maxMagnet;
        this.ad6 = amountDice6;
        this.md6 = maxDice6;
        this.ard = amountResetDaily;
        this.trs = timeRollStart;
        this.tms = timeMagnetStart;
        this.td6s = timeDice6Start;
        this.trd = timeResetDaily;
    }
}