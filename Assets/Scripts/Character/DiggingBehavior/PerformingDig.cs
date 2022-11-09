using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PerformingDig : DiggingBehavior
{
    private int numberOfTaps;
    private int internalTaps;

    public PerformingDig(int nb)
    {
        numberOfTaps = nb;
        internalTaps = 0;
    }

    public override void CancelAction()
    {
        _player.EnableInput("Move");
        _player.TransitionDigging(new StartDigging());
    }

    public override void PerformAction()
    {
        if (internalTaps < numberOfTaps)
        {
            ++internalTaps;
            // TODO: scaling of UI
            // TODO: particules 
            // TODO: SSssshaders
        }
        else
        {
            _player.Dig(1);
            _player.EnableInput("Move");
            _player.TransitionDigging(new StartDigging());
        }
    }
}