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

    protected void CancelAnimation()
    {
        a.Kill();
        b.Kill();
        sequence.Kill();
    }
}
