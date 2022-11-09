using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DiggingBehavior
{
    protected PlayerCoco _player;

    public void SetPlayer(PlayerCoco player)
    {
        _player = player;
    }

    public abstract void PerformAction();

    public abstract void CancelAction();
}
