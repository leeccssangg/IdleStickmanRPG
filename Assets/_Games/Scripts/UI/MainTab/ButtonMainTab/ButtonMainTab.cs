using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonMainTab : MonoBehaviour{
    [SerializeField] private ButtonTabType m_ButtonTabTabType;
    public RectTransform m_Target;
    
    //Images
    [SerializeField] private Image m_ImageButton;
    [SerializeField] private Sprite[] m_ButtonSprites;
    
    [SerializeField] private RectTransform m_ImageButtonRect;
    [SerializeField] private Transform m_CloseImage;
    
    [SerializeField] private LayoutElement m_LayoutElement;
    
    private RectTransform m_RectTransform;
    private Button  m_SelectButton;
    private UnityAction<ButtonTabType> m_OnSelectCallback;
    private Vector2 m_VectorDeselect;
    private Vector2 m_VectorSelect;
    private float m_OnDeselectedX;
    private float m_OnSelectedX;
    public ButtonTabType ButtonTabTabType => m_ButtonTabTabType;
    private void Awake(){
        m_RectTransform = GetComponent<RectTransform>();
        m_SelectButton = GetComponent<Button>();
        m_SelectButton.onClick.AddListener(OnClick);
        
        float x = m_Target.rect.width / 5;
        float defaultX = 192;
        if(x > defaultX) {
            x = defaultX;
        }
        m_OnSelectedX = x * 1.25f;
        m_OnDeselectedX = (x * 5 - x * 1.25f) / 4;
        m_VectorSelect = new Vector2(m_OnSelectedX,182.8f);
        m_VectorDeselect = new Vector2(m_OnDeselectedX,182.8f);
    }
    public void Setup(UnityAction<ButtonTabType> onSelectCallback){
        m_OnSelectCallback = onSelectCallback;
    }
    public void OnClick(){
        m_OnSelectCallback?.Invoke(m_ButtonTabTabType);
    }
    public void OnSelect(){
        m_ImageButton.sprite = m_ButtonSprites[0];
        UpdateLayoutElement(2);
        ScaleSelf(true);
    }
    public void OnDeselect(){
        UpdateLayoutElement(1);
        m_ImageButton.sprite = m_ButtonSprites[1];
        ScaleSelf(false);
    }
    private void UpdateLayoutElement(float size){
        m_LayoutElement.DOFlexibleSize(new Vector2(size,0), 0.25f);
    }
    private void ScaleSelf(bool isSelect){
        m_ImageButtonRect.transform.DOKill();
        m_CloseImage.DOKill();
        if(isSelect){
            m_ImageButtonRect.transform.DOScale(1.25f, 0.2f);
            m_ImageButtonRect.DOAnchorPosY(130, 0.2f);
            m_CloseImage.DOScale(1, 0.25f).From(0).SetEase(Ease.OutBack);
        } else{
            m_ImageButtonRect.transform.DOScale(1f, 0.2f);
            m_ImageButtonRect.DOAnchorPosY(90, 0.2f);
            m_CloseImage.DOScale(0, 0.2f).From(1);
        }
    }
}