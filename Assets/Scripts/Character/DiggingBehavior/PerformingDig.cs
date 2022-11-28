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
        #region ITERATION_1
        _player.getButtonMashImage.SetActive(false);
        #endregion
        #region ITERATION_2
        _player.getSlider.value = 0;
        _player.getSlider.gameObject.SetActive(false);
        #endregion
        _player.EnableInput("Move");
        _player.TransitionDigging(new StartDigging());
    }

    public override void PerformAction()
    {
        if (internalTaps < numberOfTaps - 1)
        {
            ++internalTaps;
            #region ITERATION_1
            //if(a != null)
            //{
            //    CancelAnimation();
            //    _player.getButtonMashImage.transform.localScale = new Vector3(0.64f, 0.88f, 0.64f);
            //}
            //a = _player.getButtonMashImage.transform.DOScale(_player.getButtonMashImage.transform.localScale * 1.5f, 0.17f);

            //if(b != null)
            //{
            //    CancelAnimation();
            //    _player.getButtonMashImage.transform.localScale = new Vector3(0.64f, 0.88f, 0.64f);
            //}
            //b = _player.getButtonMashImage.transform.DOScale(new Vector3(0.64f, 0.88f, 0.64f), 0.17f);
            //sequence = DOTween.Sequence();
            //sequence.Append(a).Append(b);
            #endregion
            #region ITERATION_2

            _player.getSlider.value = internalTaps / numberOfTaps;

            #endregion

            // TODO: scaling of UI
            // TODO: particules 
            // TODO: SSssshaders
        }
        else
        {
            _player.Dig(1);
            _player.EnableInput("Move");
            #region ITERATION_1
            //CancelAnimation();
            //_player.getButtonMashImage.SetActive(false);
            #endregion
            #region ITERATION_2
            _player.getSlider.gameObject.SetActive(false);
            #endregion
            _player.TransitionDigging(new StartDigging());
        }
    }
}