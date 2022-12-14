using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PerformingDig : DiggingBehavior
{
    private float numberOfTaps;
    private float internalTaps;

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
        #region ITERATION_3
        CancelAnimation();
        _player.getMainRect.gameObject.SetActive(false);
        #endregion
        _player.EnableInput("Move");
        _player.DisableArms();
        _player.TransitionDigging(new StartDigging());
        _player.DestroyCrackHole();
    }

    public override void PerformAction()
    {
        if (internalTaps < numberOfTaps - 1)
        {
            ++internalTaps;
            _player.TriggerVibration();
            _player.AnimateCrackHole(internalTaps);
            _player.AnimateDigging();
            _player.getVFX.hitImpact.gameObject.SetActive(true);
            _player.getVFX.hitImpact.Play();
            #region ITERATION_3
            if (a != null)
            {
                CancelAnimation();
                _player.getIteration3Rect.localScale = new Vector3(internalTaps / numberOfTaps, internalTaps / numberOfTaps, internalTaps / numberOfTaps); 
            }
            a = _player.getIteration3Rect.DOScale(_player.getIteration3Rect.localScale * 1.2f, 0.25f);

            if (b != null)
            {
                CancelAnimation();
                _player.getIteration3Rect.localScale = new Vector3(internalTaps / numberOfTaps, internalTaps / numberOfTaps, internalTaps / numberOfTaps);
            }
            b = _player.getIteration3Rect.DOScale(new Vector3(internalTaps / numberOfTaps, internalTaps / numberOfTaps, internalTaps / numberOfTaps), 0.25f);
            sequence = DOTween.Sequence();
            sequence.Append(a).Append(b);
            #endregion
        }
        else
        {
            _player.Dig(1);
            _player.EnableInput("Move");
            _player.DisableArms();
            #region ITERATION_3
            CancelAnimation();
            _player.getMainRect.gameObject.SetActive(false);
            #endregion
            _player.TransitionDigging(new StartDigging());
        }
    }
}