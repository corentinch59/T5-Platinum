using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DiggingBehavior
{
    protected Player _player;

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    public abstract void PerformAction();

    public abstract void CancelAction();
}
