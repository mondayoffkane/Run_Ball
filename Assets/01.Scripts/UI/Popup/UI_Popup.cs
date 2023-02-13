using System;
using DG.Tweening;
using UnityEngine;

public abstract class UI_Popup : UI_Base
{
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopupUI()
    {
        _tr?.DOKill();
        Managers.UI.ClosePopupUI(this);
    }

    private Transform _tr;
    protected bool _isTransition = false;
    protected void OpenPop(Transform tr, Action action = null, bool isForced = false)
    {
        if (!isForced && _isTransition) return;

        _isTransition = true;
        _tr = tr;
        tr.localScale = Vector3.zero;
        tr.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetUpdate(true).onComplete = () =>
        {
            _isTransition = false;
            action?.Invoke();
        };
    }
    protected void ClosePop(Transform tr, Action action = null, bool isForced = false)
    {
        if (!isForced && _isTransition) return;

        _isTransition = true;
        _tr = tr;
        tr.DOScale(0, 0.3f).SetEase(Ease.InBack).SetUpdate(true).onComplete = () =>
        {
            _isTransition = false;
            ClosePopupUI();
            action?.Invoke();
        };
    }
}
