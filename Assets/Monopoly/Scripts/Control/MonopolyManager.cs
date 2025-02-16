using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TW.Utility.Extension;
using UnityEngine;

public class MonopolyManager : SingletonFree<MonopolyManager>
{
    public static readonly int ADay = 86400;
    public static readonly int ASpecialBlock = 12;
    
    [field: SerializeField] public MonopolyJsonData MonopolyJsonData {get; private set;}
    [field: SerializeField, ReadOnly, SuffixLabel("seconds ", true)] public int TimeDiceRefill {get; private set;}
    [field: SerializeField, ReadOnly, SuffixLabel("seconds ", true)] public int TimeMagnetRefill {get; private set;}
    [field: SerializeField, ReadOnly, SuffixLabel("seconds ", true)] public int TimeDice6Refill {get; private set;}
    [field: SerializeField, ReadOnly] public int DefaultAmountResetDaily {get; private set;}
    private AWaiter NormalDiceRefillWaiter { get; set; }
    private AWaiter MagnetRefillWaiter { get; set; }
    private AWaiter Dice6RefillWaiter { get; set; }
    private AWaiter ResetDailyWaiter { get; set; }

    public void LoadData(MonopolyJsonData data)
    {
        MonopolyJsonData tempData = data;
        if (tempData == null)
        {
            tempData = new MonopolyJsonData
            {
                TimeResetDaily = null,
                Position = 0,
                BoardValue = null,
                AmountDice = 10,
                MaxDice = 10,
                AmountMagnet = 2,
                MaxMagnet = 2,
                AmountDice6 = 2,
                MaxDice6 = 2,
                AmountResetDaily = 2,
                TimeRollStart = null,
                TimeMagnetStart = null,
                TimeDice6Start = null
            };
        }
        OnInItData(tempData);
    }

    [Button]
    public void SaveData()
    {
        // DataManager.SetDirty();
        // DataManager.SaveData();
    }

    public void OnInItData(MonopolyJsonData monopoly)
    {
        MonopolyJsonData = monopoly;
        if (MonopolyJsonData.TimeRollStart.IsNullOrWhitespace()) MonopolyJsonData.TimeRollStart = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        if (MonopolyJsonData.TimeMagnetStart.IsNullOrWhitespace()) MonopolyJsonData.TimeMagnetStart = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        if (MonopolyJsonData.TimeDice6Start.IsNullOrWhitespace()) MonopolyJsonData.TimeDice6Start = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        if (MonopolyJsonData.TimeResetDaily.IsNullOrWhitespace()) MonopolyJsonData.TimeResetDaily = DateTime.Now.Date.ToString(CultureInfo.InvariantCulture);
        
        NormalDiceRefillWaiter = new AWaiter(null, OnNormalDiceRefillComplete);
        MagnetRefillWaiter = new AWaiter(null, OnMagnetRefillComplete);
        Dice6RefillWaiter = new AWaiter(null, OnDice6RefillComplete);
        ResetDailyWaiter = new AWaiter(null, OnResetDailyComplete);
        
        TimeDiceRefill = MonopolyGlobalConfig.Instance.TimeDiceRefill;
        TimeMagnetRefill = MonopolyGlobalConfig.Instance.TimeMagnetRefill;
        TimeDice6Refill = MonopolyGlobalConfig.Instance.TimeDice6Refill;
        DefaultAmountResetDaily = MonopolyGlobalConfig.Instance.AmountResetDaily;
        
        CalculateRefillResource();
    }
    public void SaveBoardData(int currentPosition, EBroadType broadType, List<UIMonopolyBlock> normalBoard, List<UIMonopolyBlock> specialBoard)
    {
        MonopolyJsonData.Position = currentPosition;
        StringBuilder broadValue = new StringBuilder();
        broadValue.Append($"{(int)broadType}~");
        broadValue.Append($"{(int)normalBoard[ASpecialBlock].BlockType}~");
        specialBoard.ForEach(block =>
        {
            broadValue.Append($"{(block.BlockType == EBlockType.Trap ? "1": "0")}");
        });
        MonopolyJsonData.BoardValue = broadValue.ToString();
    }
    private void CalculateRefillResource()
    {
        DateTime now = DateTime.Now;
        // calculate normal dice refill
        CalculateNormalDice(now);
        
        // calculate magnet refill
        CalculateMagnet(now);

        // calculate dice6 refill
        CalculateDice6(now);
        
        // calculate reset daily
        CalculateResetDaily(now);
    }

    private void CalculateNormalDice(DateTime now)
    {
        if (MonopolyJsonData.AmountDice >= MonopolyJsonData.MaxDice) return;
        DateTime timeRollStart = DateTime.Parse(MonopolyJsonData.TimeRollStart, CultureInfo.InvariantCulture);
        int diceRefillPass = (int)(now - timeRollStart).TotalSeconds;
        int diceRefillAmount = diceRefillPass / TimeDiceRefill;
        MonopolyJsonData.TimeRollStart = timeRollStart.AddSeconds(diceRefillAmount * TimeDiceRefill).ToString(CultureInfo.InvariantCulture);
        MonopolyJsonData.AmountDice += diceRefillAmount;
        if (MonopolyJsonData.AmountDice > MonopolyJsonData.MaxDice)
        {
            MonopolyJsonData.AmountDice = MonopolyJsonData.MaxDice;
        }
        if (MonopolyJsonData.AmountDice < MonopolyJsonData.MaxDice)
        {
            NormalDiceRefillWaiter.SetDuration(TimeDiceRefill - diceRefillPass % TimeDiceRefill).Play();
        }
    }

    private void CalculateMagnet(DateTime now)
    {
        if (MonopolyJsonData.AmountMagnet >= MonopolyJsonData.MaxMagnet) return;
        
        DateTime timeMagnetStart = DateTime.Parse(MonopolyJsonData.TimeMagnetStart, CultureInfo.InvariantCulture);
        int magnetRefillPass = (int)(now - timeMagnetStart).TotalSeconds;
        int magnetRefillAmount = magnetRefillPass / TimeMagnetRefill;
        MonopolyJsonData.TimeMagnetStart = timeMagnetStart.AddSeconds(magnetRefillAmount * TimeMagnetRefill).ToString(CultureInfo.InvariantCulture);
        if (MonopolyJsonData.AmountMagnet > MonopolyJsonData.MaxMagnet)
        {
            MonopolyJsonData.AmountMagnet = MonopolyJsonData.MaxMagnet;
        }

        if (MonopolyJsonData.AmountMagnet < MonopolyJsonData.MaxMagnet)
        {
            MagnetRefillWaiter.SetDuration(TimeMagnetRefill - magnetRefillPass % TimeMagnetRefill).Play();
        }
    }

    private void CalculateDice6(DateTime now)
    {
        if (MonopolyJsonData.AmountDice6 >= MonopolyJsonData.MaxDice6) return;
        
        DateTime timeDice6Start = DateTime.Parse(MonopolyJsonData.TimeDice6Start, CultureInfo.InvariantCulture);
        int dice6RefillPass = (int)(now - timeDice6Start).TotalSeconds;
        int dice6RefillAmount = dice6RefillPass / TimeDice6Refill;
        MonopolyJsonData.TimeDice6Start = timeDice6Start.AddSeconds(dice6RefillAmount * TimeDice6Refill).ToString(CultureInfo.InvariantCulture);
        if (MonopolyJsonData.AmountDice6 > MonopolyJsonData.MaxDice6)
        {
            MonopolyJsonData.AmountDice6 = MonopolyJsonData.MaxDice6;
        }

        if (MonopolyJsonData.AmountDice6 < MonopolyJsonData.MaxDice6)
        {
            Dice6RefillWaiter.SetDuration(TimeDice6Refill - dice6RefillPass % TimeDice6Refill).Play();
        }
        
    }

    private void CalculateResetDaily(DateTime now)
    {
        if (MonopolyJsonData.AmountResetDaily >= DefaultAmountResetDaily) return;
        
        DateTime timeResetDaily = DateTime.Parse(MonopolyJsonData.TimeResetDaily, CultureInfo.InvariantCulture);
        int resetDailyPass = (int)(now - timeResetDaily).TotalSeconds;
        int resetDailyAmount = resetDailyPass / ADay;
        MonopolyJsonData.TimeResetDaily = timeResetDaily.AddDays(resetDailyAmount).ToString(CultureInfo.InvariantCulture);
        if (resetDailyAmount > 1)
        {
            MonopolyJsonData.AmountResetDaily = DefaultAmountResetDaily;
        }
        ResetDailyWaiter.SetDuration(ADay - resetDailyPass % ADay).Play();
    }



    #region Refill Resource Functions

    private void OnNormalDiceRefillComplete()
    {
        MonopolyJsonData.AmountDice++;
        if (MonopolyJsonData.AmountDice < MonopolyJsonData.MaxDice)
        {
            MonopolyJsonData.TimeRollStart = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            NormalDiceRefillWaiter.SetDuration(TimeDiceRefill).Play();
        }
        if (UIManager.Ins.IsOpened<CvMonopoly>())
        {
            UIManager.Ins.GetUI<CvMonopoly>().UpdateButtonFrame();
        }
    }
    private void OnMagnetRefillComplete()
    {
        MonopolyJsonData.AmountMagnet++;
        if (MonopolyJsonData.AmountMagnet < MonopolyJsonData.MaxMagnet)
        {
            MonopolyJsonData.TimeMagnetStart = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            MagnetRefillWaiter.SetDuration(TimeMagnetRefill).Play();
        }
        if (UIManager.Ins.IsOpened<CvMonopoly>())
        {
            UIManager.Ins.GetUI<CvMonopoly>().UpdateButtonFrame();
        }
    }
    private void OnDice6RefillComplete()
    {
        MonopolyJsonData.AmountDice6++;
        if (MonopolyJsonData.AmountDice6 < MonopolyJsonData.MaxDice6)
        {
            MonopolyJsonData.TimeDice6Start = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            Dice6RefillWaiter.SetDuration(TimeDice6Refill).Play();
        }
        if (UIManager.Ins.IsOpened<CvMonopoly>())
        {
            UIManager.Ins.GetUI<CvMonopoly>().UpdateButtonFrame();
        }
    }
    private void OnResetDailyComplete()
    {
        MonopolyJsonData.AmountResetDaily = DefaultAmountResetDaily;
        MonopolyJsonData.TimeResetDaily = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        ResetDailyWaiter.SetDuration(86400).Play();
        if (UIManager.Ins.IsOpened<CvMonopoly>())
        {
            UIManager.Ins.GetUI<CvMonopoly>().UpdateButtonFrame();
        }
    }

    #endregion

    #region Update Resource Functions
    
    public void AddDailyDice()
    {
        MonopolyJsonData.AmountResetDaily--;
        AddDice(4);
    }
    
    public void ConsumeDice()
    {
        MonopolyJsonData.AmountDice--;
        if (MonopolyJsonData.AmountDice < MonopolyJsonData.MaxDice && NormalDiceRefillWaiter.IsComplete)
        {
            MonopolyJsonData.TimeRollStart = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            NormalDiceRefillWaiter.SetDuration(TimeDiceRefill).Play();
        }
    }
    public void ConsumeMagnet()
    {
        MonopolyJsonData.AmountMagnet--;
        if (MonopolyJsonData.AmountMagnet < MonopolyJsonData.MaxMagnet && MagnetRefillWaiter.IsComplete)
        {
            MonopolyJsonData.TimeMagnetStart = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            MagnetRefillWaiter.SetDuration(TimeMagnetRefill).Play();
        }
    }
    public void ConsumeDice6()
    {
        MonopolyJsonData.AmountDice6--;
        if (MonopolyJsonData.AmountDice6 < MonopolyJsonData.MaxDice6 && Dice6RefillWaiter.IsComplete)
        {
            MonopolyJsonData.TimeDice6Start = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            Dice6RefillWaiter.SetDuration(TimeDice6Refill).Play();
        }
    }
    
    public void AddDice(int amount)
    {
        MonopolyJsonData.AmountDice += amount;
        if (MonopolyJsonData.AmountDice > MonopolyJsonData.MaxDice && !NormalDiceRefillWaiter.IsComplete)
        {
            NormalDiceRefillWaiter.Kill();
        }
        if (UIManager.Ins.IsOpened<CvMonopoly>())
        {
            UIManager.Ins.GetUI<CvMonopoly>().UpdateButtonFrame();
        }
    }
    public void AddMagnet(int amount)
    {
        MonopolyJsonData.AmountMagnet += amount;
        if (MonopolyJsonData.AmountMagnet > MonopolyJsonData.MaxMagnet && !MagnetRefillWaiter.IsComplete)
        {
            MagnetRefillWaiter.Kill();
        }
        if (UIManager.Ins.IsOpened<CvMonopoly>())
        {
            UIManager.Ins.GetUI<CvMonopoly>().UpdateButtonFrame();
        }
    }
    public void AddDice6(int amount)
    {
        MonopolyJsonData.AmountDice6 += amount;
        if (MonopolyJsonData.AmountDice6 > MonopolyJsonData.MaxDice6 && !Dice6RefillWaiter.IsComplete)
        {
            Dice6RefillWaiter.Kill();
        }
        if (UIManager.Ins.IsOpened<CvMonopoly>())
        {
            UIManager.Ins.GetUI<CvMonopoly>().UpdateButtonFrame();
        }
    }

    #endregion
}
