using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDigging : DiggingBehavior
{
    public override void CancelAction(){}

    public override void PerformAction()
    {
        _player.DisableInput("Move");
        _player.SetCrackHole();
        #region ITERATION_3
        _player.getMainRect.gameObject.SetActive(true);
        _player.getIteration3Rect.localScale = Vector3.zero;
        #endregion
        _player.TransitionDigging(new PerformingDig(_player.getNumbersOfTaps));
    }
}