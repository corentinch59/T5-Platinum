using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDigging : DiggingBehavior
{
    public override void CancelAction(){}

    public override void PerformAction()
    {
        _player.DisableInput("Move");
        _player.TransitionDigging(new PerformingDig(_player.getNumbersOfTaps));
    }
}