using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PerformingDig : DiggingBehavior
{
    private int numberOfTaps;
    private int internalTaps;
    private Tween a;
    private Tween b;
    private Sequence sequence;

    public PerformingDig(int nb)
    {
        numberOfTaps = nb;
        internalTaps = 0;
    }

    public override void CancelAction()
    {
        _player.getButtonMashImage.SetActive(false);
        _player.EnableInput("Move");
        _player.TransitionDigging(new StartDigging());
    }

    public override void PerformAction()
    {
        if (internalTaps < numberOfTaps)
        {
            ++internalTaps;
            Animatebutton();
            // TODO: scaling of UI
            // TODO: particules 
            // TODO: SSssshaders
        }
        else
        {
            _player.Dig(1);
            _player.EnableInput("Move");
            a.Kill();
            b.Kill();
            _player.getButtonMashImage.SetActive(false);
            _player.TransitionDigging(new StartDigging());
        }
    }
}