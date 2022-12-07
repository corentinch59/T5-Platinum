using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDigging : DiggingBehavior
{
    public override void CancelAction(){}

    public override void PerformAction()
    {
        _player.TriggerVibration();
        _player.DisableInput("Move");
        if (_player.getObjectFound != null && _player.getObjectFound.TryGetComponent(out Hole h)) { } else { _player.SetCrackHole(); }
        _player.getMainRect.gameObject.SetActive(true);
        _player.getIteration3Rect.localScale = Vector3.zero;
        _player.TransitionDigging(new PerformingDig(_player.getNumbersOfTaps));
    }
}