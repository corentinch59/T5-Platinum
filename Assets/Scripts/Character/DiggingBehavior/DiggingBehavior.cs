using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DiggingBehavior
{
    protected Player _player;
    protected Tween a;
    protected Tween b;
    protected Sequence sequence;

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    public abstract void PerformAction();

    public abstract void CancelAction();

    protected void Animatebutton()
    {
        if (a != null)
        {
            a.Kill();
            sequence.Kill();
            _player.getButtonMashImage.transform.localScale = new Vector3(0.64f, 0.88f, 0.64f);
        }
        a = _player.getButtonMashImage.transform.DOScale(_player.getButtonMashImage.transform.localScale * 1.5f, 0.25f).SetEase(Ease.InBounce);
        if (b != null)
        {
            b.Kill();
            sequence.Kill();
            _player.getButtonMashImage.transform.localScale = new Vector3(0.64f, 0.88f, 0.64f);
        }
        b = _player.getButtonMashImage.transform.DOScale(new Vector3(0.64f, 0.88f, 0.64f), 0.25f).SetEase(Ease.OutBounce);

        sequence = DOTween.Sequence();
        sequence.Append(a).Append(b);
    }
}
