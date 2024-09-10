using DG.Tweening;
using System;
using UnityEngine;

public abstract class UIElement : MonoBehaviour
{
    private Action onHidden;
    public abstract bool ManualHide { get; }
    public abstract bool DestroyOnHide { get; }
    public abstract bool UseBehindPanel { get; }
    [SerializeField] protected GameObject holder;
    [SerializeField] protected RectTransform popup;
    public virtual void Show(Action hidden)
    {
        onHidden = hidden;
        Show();
    }
    public virtual void Show()
    {
        GameUI.Instance.Submit(this);
        holder?.SetActive(true);
    }
    public virtual void Hide()
    {
        GameUI.Instance.Unsubmit(this);
        onHidden?.Invoke();
        if (DestroyOnHide)
        {
            GameUI.Instance.Unregister(this);
            Destroy(gameObject);
        }
        else holder?.SetActive(false);

    }
    protected virtual void Awake()
    {
        GameUI.Instance.Register(this);
    }
    protected virtual void DOMoveIn(RectTransform element, bool x, bool y, Action onShow = null, float moveX = 1000, float moveY = 2000, float duration = 0.3f)
    {
        if (x)
            element.DOAnchorPosX(moveX, duration).SetEase(Ease.InOutBack).From().OnComplete(() =>
            {
                onShow?.Invoke();
            });
        if (y)
            element.DOAnchorPosY(moveY, duration).SetEase(Ease.InOutBack).From().OnComplete(() =>
            {
                onShow?.Invoke();
            });
    }
    protected virtual void DOMoveOut(RectTransform element, bool x, bool y, Action onHide = null, float moveX = 1000, float moveY = 2000, float duration = 0.3f)
    {
        Vector2 temp = element.anchoredPosition;
        if (x)
            element.DOAnchorPosX(-moveX, duration).SetEase(Ease.InBack).OnComplete(() =>
            {
                onHide?.Invoke();
                element.anchoredPosition = temp;
            });
        if (y)
            element.DOAnchorPosY(-moveY, duration).SetEase(Ease.InBack).OnComplete(() =>
            {
                onHide?.Invoke();
                element.anchoredPosition = temp;
            });
    }
}