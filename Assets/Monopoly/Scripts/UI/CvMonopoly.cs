using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using TW.Utility.Extension;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CvMonopoly : UICanvas
{
    [field: Title("Data")]
    [field: SerializeField, ReadOnly]
    public MonopolyManager MonopolyControl { get; private set; }

    [field: SerializeField, ReadOnly] public MonopolyJsonData MonopolyJsonData { get; private set; }

    [field: Title("Pool")]
    [field: SerializeField]
    public Transform UIMonopolyResourceClaimContainer { get; private set; }

    [field: SerializeField] public UIMonopolyResourceClaim UIMonopolyResourceClaimPrefab { get; private set; }
    private Pextension.MiniPool<UIMonopolyResourceClaim> UIResourceClaimPool { get; set; }

    [field: Title("Broad")]
    [field: SerializeField]
    public EBroadType BroadType { get; private set; }

    [field: SerializeField] public Transform StartBlock { get; private set; }
    [field: SerializeField] public Transform UpStageBlock { get; private set; }
    [field: SerializeField] public UIMonopolyBlock UIMonopolyBlockPrefab { get; private set; }
    [field: SerializeField] public Transform NormalBroad { get; private set; }
    [field: SerializeField] public Transform SpecialBroad { get; private set; }
    [field: SerializeField] public List<UIMonopolyBlock> UIMonopolyNormalBlocks { get; private set; }
    [field: SerializeField] public List<UIMonopolyBlock> UIMonopolySpecialBlocks { get; private set; }

    [field: Title("Character")]
    [field: SerializeField]
    public RectTransform Character { get; private set; }

    [field: SerializeField] public RectTransform CharacterShadow { get; private set; }
    [field: SerializeField] public int CurrentPosition { get; private set; }

    [field: Title("Dice")]
    [field: SerializeField]
    public Transform Dice { get; private set; }

    [field: SerializeField] public Image DiceImage { get; private set; }
    [field: SerializeField] public Sprite[] DiceSprites { get; private set; }

    [field: Title("Button Panel")]
    [field: SerializeField]
    public GameObject BlockButtonPanel { get; private set; }

    [field: SerializeField] public Button ButtonRoll { get; private set; }
    [field: SerializeField] public Button ButtonAddDice { get; private set; }
    [field: SerializeField] public Button ButtonMagnet { get; private set; }
    [field: SerializeField] public Button ButtonDice6 { get; private set; }
    [field: SerializeField] public GameObject AddMagnet { get; private set; }
    [field: SerializeField] public GameObject AddDice6 { get; private set; }
    [field: SerializeField] public GameObject MagnetCheckBox { get; private set; }
    [field: SerializeField] public GameObject MagnetCheckMark { get; private set; }
    [field: SerializeField] public GameObject Dice6CheckBox { get; private set; }
    [field: SerializeField] public GameObject Dice6CheckMark { get; private set; }
    [field: SerializeField] public TextMeshProUGUI TextDiceAmount { get; private set; }
    [field: SerializeField] public TextMeshProUGUI TextMagnetAmount { get; private set; }
    [field: SerializeField] public TextMeshProUGUI TextDice6Amount { get; private set; }
    [field: SerializeField] public TextMeshProUGUI TextDiceRefill { get; private set; }
    [field: SerializeField] public TextMeshProUGUI TextMagnetRefill { get; private set; }
    [field: SerializeField] public TextMeshProUGUI TextDice6Refill { get; private set; }
    [field: SerializeField] public TextMeshProUGUI TextResetDaily { get; private set; }

    private List<UIMonopolyBlock> UIMonopolyBlocks =>
        BroadType == EBroadType.Normal ? UIMonopolyNormalBlocks : UIMonopolySpecialBlocks;

    [FormerlySerializedAs("SLOW")]
    [Header("Jump Config")]
    [SerializeField] private float m_Slow = 0.6f;
    [SerializeField] private float m_Fast = 0.15f;
    [SerializeField] private float m_MoveUpPower = 5;
    [SerializeField] private float m_MoveDownPower = 15;
    [SerializeField] private float m_NormalPower = 10;

    private bool IsUsingMagnet { get; set; }
    private bool IsUsingDice6 { get; set; }
    private Coroutine SyncPerSecondCoroutine { get; set; }

    private Vector3 UpStagePosition => UpStageBlock.position;
    private Vector3 StartPosition => StartBlock.position;

    private void Awake()
    {
        // TODO: Get MonopolyControl 
        
        MonopolyControl = MonopolyManager.Ins;
        
        MonopolyJsonData = MonopolyControl.MonopolyJsonData;
        UIResourceClaimPool =
            new Pextension.MiniPool<UIMonopolyResourceClaim>(UIMonopolyResourceClaimPrefab, 6, UIMonopolyResourceClaimContainer);
        
        ButtonRoll.onClick.AddListener(OnClickButtonRoll);
        ButtonAddDice.onClick.AddListener(OnClickButtonAdDice);
        ButtonMagnet.onClick.AddListener(OnClickButtonMagnet);
        ButtonDice6.onClick.AddListener(OnClickButtonDice6);
    }

    public override void Setup()
    {
        base.Setup();

        // Load broad Data
        CurrentPosition = MonopolyJsonData.Position;
        LoadBoardData(MonopolyJsonData.BoardValue);

        Character.position = UIMonopolyBlocks[CurrentPosition].transform.position;
        CharacterShadow.position = Character.position;

        BlockButtonPanel.SetActive(false);
        IsUsingMagnet = false;
        IsUsingDice6 = false;

        UpdateButtonFrame();
    }
    private void OnEnable()
    {
        SyncPerSecondCoroutine = StartCoroutine(SyncPerFrame());
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        StopCoroutine(SyncPerSecondCoroutine);
    }

    private void LoadBoardData(string broadValue)
    {
        if (broadValue.IsNullOrWhitespace()) return;
        string[] splitData = broadValue.Split('~');
        BroadType = (EBroadType)int.Parse(splitData[0]);
        NormalBroad.gameObject.SetActive(BroadType == EBroadType.Normal);
        SpecialBroad.gameObject.SetActive(BroadType == EBroadType.Special);

        EBlockType highlightBlockType = (EBlockType)int.Parse(splitData[1]);
        BlockConfig highlightBlockConfig = MonopolyGlobalConfig.Instance.GetHighLightBlockConfig(highlightBlockType);
        Debug.Log(highlightBlockType.ToString());
        UIMonopolyNormalBlocks[MonopolyManager.ASpecialBlock].ChangeBlockConfig(highlightBlockConfig);

        splitData[2].ForEach((c, i) =>
        {
            if (c == '1')
            {
                UIMonopolySpecialBlocks[i].ChangeToTrap();
            }
        });
    }

    private void OnUpStage()
    {
        CharacterShadow.DOScale(0, 1).SetEase(Ease.OutCubic).SetDelay(0.3f);
        Character.DOMove(UpStagePosition + Vector3.up * 10, 1).SetEase(Ease.OutCubic).SetDelay(0.3f);

        UIManager.Ins.OpenUI<CvChangeView>().SetupOnOpen(UpStagePosition, StartPosition, 1, 1, () =>
        {
            ChangeBroadType(EBroadType.Special);
            CurrentPosition = 0;
            MonopolyControl.SaveBoardData(CurrentPosition, BroadType, UIMonopolyNormalBlocks, UIMonopolySpecialBlocks);
        }, () =>
        {
            CharacterShadow.position = StartPosition;
            CharacterShadow.DOScale(1f, 1).SetEase(Ease.InCubic);
            Character.DOMove(StartPosition, 1).SetEase(Ease.InCubic);
        });
    }

    private void OnDownStage()
    {
        Vector3 currentPosition = Character.position;
        CharacterShadow.DOScale(0, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.3f);
        Character.DOMove(currentPosition + Vector3.up * 0.5f, 0.2f).SetEase(Ease.OutSine).SetDelay(0.3f);
        Character.DOMove(currentPosition, 0.5f).SetEase(Ease.InSine).SetDelay(0.5f);
        Character.DOScale(0, 0.5f).SetEase(Ease.InSine).SetDelay(0.5f);
        UIManager.Ins.OpenUI<CvChangeView>().SetupOnOpen(currentPosition, UpStagePosition, 1, 1, () =>
        {
            ChangeBroadType(EBroadType.Normal);
            CurrentPosition = 18;
            MonopolyControl.SaveBoardData(CurrentPosition, BroadType, UIMonopolyNormalBlocks, UIMonopolySpecialBlocks);
        }, () =>
        {
            CharacterShadow.position = UpStagePosition;
            CharacterShadow.DOScale(1f, 1).SetEase(Ease.InCubic);
            Character.position = UpStagePosition + Vector3.up * 10;
            Character.localScale = Vector3.one;
            Character.DOMove(UpStagePosition, 1).SetEase(Ease.InCubic);
            Character.eulerAngles = Vector3.zero;
        });
    }

    private void ChangeBroadType(EBroadType broadType)
    {
        BroadType = broadType;

        NormalBroad.gameObject.SetActive(BroadType == EBroadType.Normal);
        SpecialBroad.gameObject.SetActive(BroadType == EBroadType.Special);
        UIMonopolyNormalBlocks.ForEach(block => block.ResetBlock());
        UIMonopolySpecialBlocks.ForEach(block => block.ResetBlock());
    }

    private void RollADice(int diceValue, bool isUsingMagnet = false, bool isUsingDice6 = false)
    {
        BlockButtonPanel.SetActive(true);
        int count = 0;
        Dice.DOScale(2f, 0.5f).SetEase(Ease.OutCubic).SetLoops(2, LoopType.Yoyo)
            .OnUpdate(() =>
            {
                count++;
                if (count > 2)
                {
                    DiceImage.sprite = DiceSprites[6..].GetRandomElement();
                    count = 0;
                }
            })
            .OnComplete(() =>
            {
                if (isUsingDice6) diceValue = 6;
                diceValue = diceValue < 0 ? UnityEngine.Random.Range(1, 7) : diceValue;
                DiceImage.sprite = DiceSprites[diceValue - 1];
                MoveToNextStep(diceValue, isUsingMagnet);
            });
    }

    private void MoveToNextStep(int stepRemaining, bool isUsingMagnet)
    {
        if (stepRemaining == 0)
        {
            BlockButtonPanel.SetActive(false);
            return;
        }

        ;

        CurrentPosition = (CurrentPosition + 1) % UIMonopolyBlocks.Count;
        Vector3 nextPosition = UIMonopolyBlocks[CurrentPosition].transform.position;
        Vector3 currentPosition = Character.position;
        if (!TryGetJumpConfig(stepRemaining, currentPosition, nextPosition, out float power, out float duration)) return;
        CharacterShadow.DOMove(nextPosition, duration).SetEase(Ease.Linear);
        CharacterShadow.DOScale(CharacterShadow.localScale * 0.5f, duration / 2).SetEase(Ease.Linear)
            .SetLoops(2, LoopType.Yoyo);
        Character.DOJump(nextPosition, power, 1, duration)
            
            .OnComplete(() =>
            {
                OnCharacterPass(stepRemaining, isUsingMagnet);
                MoveToNextStep(stepRemaining - 1, isUsingMagnet);
            });
    }

    private void OnCharacterLand()
    {
        Vector3 currentScale = Character.localScale;
        Vector3 currentPosition = Character.position;
        CharacterShadow.DOScaleX(currentScale.x * 1.1f, 0.1f).SetLoops(2, LoopType.Yoyo);
        CharacterShadow.DOScaleY(currentScale.y * 0.9f, 0.1f).SetLoops(2, LoopType.Yoyo);
        CharacterShadow.DOMoveY(currentPosition.y - 0.1f, 0.1f).SetLoops(2, LoopType.Yoyo);
        Character.DOScaleX(currentScale.x * 1.1f, 0.1f).SetLoops(2, LoopType.Yoyo);
        Character.DOScaleY(currentScale.y * 0.9f, 0.1f).SetLoops(2, LoopType.Yoyo);
        Character.DOMoveY(currentPosition.y - 0.1f, 0.1f).SetLoops(2, LoopType.Yoyo);
    }

    private void OnCharacterPass(int stepRemaining, bool isUsingMagnet)
    {
        UIMonopolyBlocks[CurrentPosition].OnCharacterLand();
        if (stepRemaining == 1)
        {
            switch (UIMonopolyBlocks[CurrentPosition].BlockType)
            {
                case EBlockType.UpStage:
                    OnUpStage();
                    UIMonopolyBlocks[CurrentPosition].StartUpStageAnim();
                    break;
                case EBlockType.Trap:
                    OnDownStage();
                    UIMonopolyBlocks[CurrentPosition].StartTrapAnim();
                    break;
            }

            OnCharacterLand();
        }


        if (stepRemaining != 1 && !isUsingMagnet &&
            UIMonopolyBlocks[CurrentPosition].BlockType != EBlockType.Start) return;
        ClaimResource(UIMonopolyBlocks[CurrentPosition]);
    }

    private void ClaimResource(UIMonopolyBlock uiMonopolyBlock)
    {
        if (uiMonopolyBlock.BlockType != EBlockType.Empty &&
            uiMonopolyBlock.BlockType != EBlockType.UpStage &&
            uiMonopolyBlock.BlockType != EBlockType.Trap)
        {
            UIMonopolyResourceClaim uiMonopolyResourceClaim = UIResourceClaimPool.Spawn();
            uiMonopolyResourceClaim.Setup(uiMonopolyBlock.transform, uiMonopolyBlock.BlockType,
                uiMonopolyBlock.BlockConfig.GetValueText(),
                () => UIResourceClaimPool.Despawn(uiMonopolyResourceClaim));
        }

        if (uiMonopolyBlock.BlockType == EBlockType.Start)
        {
            UIMonopolyNormalBlocks[MonopolyManager.ASpecialBlock].ChangeRandomHighLightBlock();
        }

        int amount = uiMonopolyBlock.BlockConfig.GetValue().ToInt();
        switch (uiMonopolyBlock.BlockType)
        {
            case EBlockType.Start:
                Debug.Log($"Claim Resource: Start - {amount}");
                break;
            case EBlockType.Gold:
                Debug.Log($"Claim Resource: Gold - {amount}");
                break;
            case EBlockType.Gem:
                Debug.Log($"Claim Resource: Gem - {amount}");
                break;
            case EBlockType.Diamond:
                Debug.Log($"Claim Resource: Diamond - {amount}");
                break;
            case EBlockType.Crystal:
                Debug.Log($"Claim Resource: Crystal - {amount}");
                break;
            case EBlockType.Key:
                Debug.Log($"Claim Resource: Key - {amount}");
                break;
            case EBlockType.Empty:
                Debug.Log("Claim Resource: Empty");
                break;
            case EBlockType.UpStage:
                Debug.Log("Claim Resource: UpStage");
                break;
            case EBlockType.Trap:
                Debug.Log("Claim Resource: Trap");
                break;
            case EBlockType.Dice6Only:
                MonopolyControl.AddDice6(amount);
                break;
            case EBlockType.Magnet:
                MonopolyControl.AddMagnet(amount);
                break;
            case EBlockType.Dice:
                MonopolyControl.AddDice(amount);
                break;
            default:
                break;
        }

        if (BroadType == EBroadType.Special)
        {
            uiMonopolyBlock.ChangeToTrap();
        }

        MonopolyControl.SaveBoardData(CurrentPosition, BroadType, UIMonopolyNormalBlocks, UIMonopolySpecialBlocks);
    }

    private bool TryGetJumpConfig(int stepRemaining, Vector3 currentPosition, Vector3 nextPosition, out float power,
        out float duration)
    {
        bool isFastStep = stepRemaining > 1;
        bool isMoveUp = nextPosition.y - currentPosition.y > 0.1f;
        bool isMoveDown = nextPosition.y - currentPosition.y < -0.1f;
        bool isMoveLeft = nextPosition.x - currentPosition.x < -0.1f;
        bool isMoveRight = nextPosition.x - currentPosition.x > 0.1f;

        power = m_NormalPower;
        if (isMoveUp) power = m_MoveUpPower;
        if (isMoveDown) power = m_MoveDownPower;
        if (isMoveLeft)
        {
            Character.localScale = Vector3.one;
            CharacterShadow.localScale = Vector3.one;
        }

        if (isMoveRight)
        {
            Character.localScale = Vector3.one + 2 * Vector3.left;
            CharacterShadow.localScale = Vector3.one + 2 * Vector3.left;
        }

        power *= isFastStep ? 1f : 5f;
        duration = isFastStep ? m_Fast : m_Slow;

        return true;
    }

    public void UpdateButtonFrame()
    {
        bool isNormalDiceEnough = MonopolyJsonData.AmountDice > 0;
        bool isMagnetEnough = MonopolyJsonData.AmountMagnet > 0;
        bool isDice6Enough = MonopolyJsonData.AmountDice6 > 0;
        TextDiceAmount.SetText("{0}/{1}", MonopolyJsonData.AmountDice, MonopolyJsonData.MaxDice);
        ButtonRoll.gameObject.SetActive(isNormalDiceEnough);
        ButtonAddDice.gameObject.SetActive(!isNormalDiceEnough);
        TextMagnetAmount.SetText(
            $"{(MonopolyJsonData.AmountMagnet == 0 ? "<color=#F05946>" : "")}{MonopolyJsonData.AmountMagnet.ToString()}");
        TextDice6Amount.SetText(
            $"{(MonopolyJsonData.AmountDice6 == 0 ? "<color=#F05946>" : "")}{MonopolyJsonData.AmountDice6.ToString()}");
        TextResetDaily.SetText("({0}/{1})", MonopolyJsonData.AmountResetDaily, MonopolyControl.DefaultAmountResetDaily);
        MagnetCheckBox.SetActive(isMagnetEnough);
        MagnetCheckMark.SetActive(IsUsingMagnet);
        Dice6CheckBox.SetActive(isDice6Enough);
        Dice6CheckMark.SetActive(IsUsingDice6);
        AddMagnet.SetActive(!isMagnetEnough);
        AddDice6.SetActive(!isDice6Enough);
        UpdateTimeRefill();
    }

    private void OnClickButtonRoll()
    {
        MonopolyControl.ConsumeDice();
        RollADice(-1, IsUsingMagnet, IsUsingDice6);
        if (IsUsingMagnet)
        {
            MonopolyControl.ConsumeMagnet();
            IsUsingMagnet = false;
        }

        if (IsUsingDice6)
        {
            MonopolyControl.ConsumeDice6();
            IsUsingDice6 = false;
        }

        UpdateButtonFrame();
    }

    private void OnClickButtonAdDice()
    {
        if (MonopolyJsonData.AmountResetDaily > 0)
        {
            //TODO: Show reward ads
            {
                MonopolyControl.AddDailyDice();
                UpdateButtonFrame();
            }
        }
    }

    private void OnClickButtonMagnet()
    {
        if (MonopolyJsonData.AmountMagnet > 0)
        {
            IsUsingMagnet = !IsUsingMagnet;
            MagnetCheckMark.SetActive(IsUsingMagnet);
        }
        else
        {
            //TODO: Show reward ads
            {
                MonopolyControl.AddMagnet(2);
                UpdateButtonFrame();
            }
        }
    }

    private void OnClickButtonDice6()
    {
        if (MonopolyJsonData.AmountDice6 > 0)
        {
            IsUsingDice6 = !IsUsingDice6;
            Dice6CheckMark.SetActive(IsUsingDice6);
        }
        else
        {
            // TODO: Show reward ads
            {
                MonopolyControl.AddDice6(2);
                UpdateButtonFrame();
            }
        }
    }

    private void UpdateTimeRefill()
    {
        int amountDice = MonopolyJsonData.AmountDice;
        int maxDice = MonopolyJsonData.MaxDice;
        int amountMagnet = MonopolyJsonData.AmountMagnet;
        int amountDice6 = MonopolyJsonData.AmountDice6;

        DateTime now = DateTime.Now;
        if (amountDice < maxDice)
        {
            DateTime.TryParse(MonopolyJsonData.TimeRollStart, out DateTime timeDiceRefill);
            TimeSpan timeDiceRefillRemaining =
                timeDiceRefill + TimeSpan.FromSeconds(MonopolyControl.TimeDiceRefill + 1) - now;
            TextDiceRefill.SetText(timeDiceRefillRemaining.ToStringFull());
        }
        else
        {
            TextDiceRefill.SetText("");
        }

        if (amountMagnet == 0)
        {
            DateTime.TryParse(MonopolyJsonData.TimeMagnetStart, out DateTime timeMagnetStart);
            TimeSpan timeMagnetRefillRemaining =
                timeMagnetStart + TimeSpan.FromSeconds(MonopolyControl.TimeMagnetRefill + 1) - now;
            TextMagnetRefill.SetText(timeMagnetRefillRemaining.ToStringFull());
        }
        else
        {
            TextMagnetRefill.SetText("");
        }

        if (amountDice6 == 0)
        {
            DateTime.TryParse(MonopolyJsonData.TimeDice6Start, out DateTime timeDice6Start);
            TimeSpan timeDice6RefillRemaining =
                timeDice6Start + TimeSpan.FromSeconds(MonopolyControl.TimeDice6Refill + 1) - now;
            TextDice6Refill.SetText(timeDice6RefillRemaining.ToStringFull());
        }
        else
        {
            TextDice6Refill.SetText("");
        }
    }

    [SuppressMessage("ReSharper", "IteratorNeverReturns")]
    private IEnumerator SyncPerFrame()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        while (true)
        {
            UpdateTimeRefill();
            yield return waitForEndOfFrame;
        }
    }

    #region Editor Functions

#if UNITY_EDITOR
    [field: Title("Editor")]
    [field: SerializeField]
    public Transform[] NormalGrid { get; private set; }

    [field: SerializeField] public Transform[] SpecialGrid { get; private set; }

    // private int[] NormalPositionData { get; } = { 24, 23, 22, 21, 20, 15, 10, 5, 0, 1, 2, 3, 4, 9, 14, 19 };
    private int[] NormalPositionData { get; } =
        { 48, 47, 46, 45, 44, 43, 42, 35, 28, 21, 14, 7, 0, 1, 2, 3, 4, 5, 6, 13, 20, 27, 34, 41 };

    private int[] SpecialPositionData { get; } =
        { 35, 34, 33, 32, 31, 30, 24, 18, 12, 6, 0, 1, 2, 3, 4, 5, 11, 17, 23, 29 };

    [Button]
    private void InitStage()
    {
        InitNormalStage();
        InitSpecialStage();
    }

    private void InitNormalStage()
    {
        UIMonopolyNormalBlocks.ForEach(block => DestroyImmediate(block.gameObject));
        UIMonopolyNormalBlocks.Clear();

        for (int i = 0; i < MonopolyGlobalConfig.Instance.NormalBlockConfigs.Length; i++)
        {
            UIMonopolyBlock block =
                PrefabUtility.InstantiatePrefab(UIMonopolyBlockPrefab, NormalBroad) as UIMonopolyBlock;
            EditorUtility.SetDirty(block);
            block.transform.localPosition = NormalGrid[NormalPositionData[i]].localPosition;
            block.name = $"Block {i}";
            block.Init(MonopolyGlobalConfig.Instance.NormalBlockConfigs[i]);
            UIMonopolyNormalBlocks.Add(block);
        }

        // set sibling index by y position
        List<UIMonopolyBlock> sortUIMonopolyBlocks =
            UIMonopolyNormalBlocks.OrderByDescending(block => block.transform.localPosition.y).ToList();
        for (int i = 0; i < sortUIMonopolyBlocks.Count; i++)
        {
            sortUIMonopolyBlocks[i].transform.SetSiblingIndex(i);
        }
    }

    private void InitSpecialStage()
    {
        UIMonopolySpecialBlocks.ForEach(block => DestroyImmediate(block.gameObject));
        UIMonopolySpecialBlocks.Clear();

        for (int i = 0; i < MonopolyGlobalConfig.Instance.SpecialBlockConfigs.Length; i++)
        {
            UIMonopolyBlock block =
                PrefabUtility.InstantiatePrefab(UIMonopolyBlockPrefab, SpecialBroad) as UIMonopolyBlock;
            EditorUtility.SetDirty(block);
            block!.transform.localPosition = SpecialGrid[SpecialPositionData[i]].localPosition;
            block.name = $"Block {i}";
            block.Init(MonopolyGlobalConfig.Instance.SpecialBlockConfigs[i]);
            UIMonopolySpecialBlocks.Add(block);
        }

        // set sibling index by y position
        List<UIMonopolyBlock> sortUIMonopolyBlocks =
            UIMonopolySpecialBlocks.OrderByDescending(block => block.transform.localPosition.y).ToList();
        for (int i = 0; i < sortUIMonopolyBlocks.Count; i++)
        {
            sortUIMonopolyBlocks[i].transform.SetSiblingIndex(i);
        }
    }
#endif

    #endregion
}

public enum EBroadType
{
    Normal,
    Special,
}