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
        _player.getSlider.gameObject.SetActive(true);
        _player.getSlider.value = 0;
        #endregion
        _player.TransitionDigging(new PerformingDig(_player.getNumbersOfTaps));
    }
}