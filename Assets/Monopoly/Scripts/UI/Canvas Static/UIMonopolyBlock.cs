using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using TW.Utility.CustomType;
using TW.Utility.Extension;
using UnityEngine;
using UnityEngine.UI;

public class UIMonopolyBlock : MonoBehaviour
{
    [field: SerializeField] public BlockConfig BlockConfig {get; private set;}
    [field: SerializeField] public Transform MainView {get; private set;}
    [field: SerializeField] public Image ImageBlock {get; private set;}
    [field: SerializeField] public TextMeshProUGUI TextValue {get; private set;}
    [field: SerializeField] public BigNumber BaseValue { get; set; }
    [ShowInInspector] public EBlockType BlockType => BlockConfig.BlockType;
    [field: SerializeField] public GameObject UpStageBlock {get; private set;}
    [field: SerializeField] public Transform[] UpStageAnim {get; private set;}
    [field: SerializeField] public GameObject TrapBlock {get; private set;}
    [field: SerializeField] public Image ImageTrapBlock {get; private set;}
    [field: SerializeField] public Sprite[] TrapSprites {get; private set;}
    [field: SerializeField] public GameObject ReloadStage {get; private set;}
    [field: SerializeField] public Transform ImageReloadStage {get; private set;}
    [field: SerializeField] public Image ImageReloadBg {get; private set;}
    private BlockConfig DefaultBlockConfig { get; set; }
    public void Init(BlockConfig blockConfig)
    {
        BlockConfig = blockConfig;
        UpdateUI();
    }
    public void ChangeToTrap()
    {
        if (BlockConfig.BlockType == EBlockType.Trap) return;
        DefaultBlockConfig = BlockConfig;
        BlockConfig = MonopolyGlobalConfig.Instance.TrapBlockConfig;
        UpdateUI();
    }

    public void ChangeRandomHighLightBlock()
    {
        BlockConfig newConfig = MonopolyGlobalConfig.Instance.HighlightBlockConfigs.Where(x => x.BlockType != BlockType).GetRandomElement();
        ReloadStage.SetActive(true);
        ImageReloadBg.color = Color.clear;
        ImageReloadBg.DOColor(Color.white, 0.5f).SetLoops(2, LoopType.Yoyo);
        ImageReloadStage.DOLocalRotate(new Vector3(0, 0, -360), 1, RotateMode.FastBeyond360);
        ImageReloadStage.DOScale(1.5f, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            ChangeBlockConfig(newConfig);
            ImageReloadStage.DOScale(1, 0.5f).SetEase(Ease.InCubic).OnComplete(() =>
            {
                ReloadStage.SetActive(false);
            });
        });

    }
    public void ChangeBlockConfig(BlockConfig blockConfig)
    {
        BlockConfig = blockConfig;
        UpdateUI();
    }
    private void UpdateUI()
    {
        BaseValue = BlockConfig.GetValue();
        ImageBlock.sprite = BlockConfig.BlockIcon;
        
        TextValue.text = BlockConfig.IsShowText ? BlockConfig.GetValueText() : "";
    }
    [Button]
    public void ResetBlock()
    {
        UpStageBlock.SetActive(false);
        TrapBlock.SetActive(false);
        
        if (BlockType == EBlockType.Trap && DefaultBlockConfig != null)
        {
            BlockConfig = DefaultBlockConfig;
            UpdateUI();
        }
    }
    public void StartUpStageAnim()
    {
        UpStageAnim.ForEach(x =>
        {
            x.localPosition = Vector3.zero;
        });
        UpStageAnim[0].DOLocalMoveY(150, 1).SetEase(Ease.OutElastic).SetDelay(0.3f).OnStart(() => UpStageBlock.SetActive(true));
        UpStageAnim[1].DOLocalMoveY(100, 1).SetEase(Ease.OutElastic).SetDelay(0.3f);
        UpStageAnim[2].DOLocalMoveY(50, 1).SetEase(Ease.OutElastic).SetDelay(0.3f);
    }
    public void StartTrapAnim()
    {
        int spriteIndex = 0;
        DOTween.To(() => spriteIndex, x => spriteIndex = x, TrapSprites.Length - 1, 0.5f)
            .SetDelay(0.3f)
            .OnStart(() => TrapBlock.SetActive(true))
            .OnUpdate(() => ImageTrapBlock.sprite = TrapSprites[spriteIndex]);
    }
    public void OnCharacterLand()
    {
        MainView.DOScaleX(1.1f, 0.1f).SetLoops(2, LoopType.Yoyo);
        MainView.DOScaleY(0.9f, 0.1f).SetLoops(2, LoopType.Yoyo);
        MainView.DOLocalMoveY(-10, 0.1f).SetLoops(2, LoopType.Yoyo);
    }
}
