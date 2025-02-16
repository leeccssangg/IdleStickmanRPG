using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UIMonopolyResourceClaim : MonoBehaviour
{
    [field: SerializeField] public RectTransform RectTransform {get; private set;}
    [field: SerializeField] public CanvasGroup MainView {get; private set;}
    [field: SerializeField] public TextMeshProUGUI TextResourceClaim {get; private set;}
    [field: SerializeField] private Transform Target { get; set; }
    private UnityAction OnComplete { get; set; }

    public void Setup(Transform target ,EBlockType resourceType, string resourceValue, UnityAction onComplete)
    {
        Target = target;
        TextResourceClaim.text = $"{IconText.Get(resourceType.ToString())} {resourceValue}";
        OnComplete = onComplete;

        StartClaimAnim();
    }
    private void StartClaimAnim()
    {
        Vector3 targetPosition = Target.position;
        RectTransform.position = targetPosition;
        MainView.alpha = 0;
        RectTransform.DOMoveY(targetPosition.y + 50, 0.75f).SetEase(Ease.OutBack);
        MainView.DOFade(1, 0.5f)
            .SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                MainView.DOFade(0, 0.5f)
                    .SetEase(Ease.InSine)
                    .SetDelay(0.5f)
                    .OnComplete(() =>
                    {
                        OnComplete?.Invoke();
                    });
            });
    }
}