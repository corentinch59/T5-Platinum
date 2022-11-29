using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDigging : DiggingBehavior
{
    public override void CancelAction(){}

    public override void PerformAction()
    {
        _player.DisableInput("Move");
        #region ITERATION_1
        //_player.getButtonMashImage.SetActive(true);
        #endregion
        #region ITERATION_2
        //_player.getSlider.gameObject.SetActive(true);
        //_player.getSlider.value = 0;
        #endregion
        #region ITERATION_3
        _player.getMainRect.gameObject.SetActive(true);
        _player.getIteration3Rect.localScale = Vector3.zero;
        #endregion
        _player.TransitionDigging(new PerformingDig(_player.getNumbersOfTaps));
    }
}