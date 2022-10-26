using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncineratorScript : MonoBehaviour
{
    public static Action OnPlayerHold;
    public static Action OnPlayerLetgo;

    private int numberOfPlayers = 0;
    private bool incineratorOpen;

    private void Start()
    {
        OnPlayerHold += AddPlayer;
        OnPlayerLetgo += substractPlayer;
    }

    private void OpenIncinerator()
    {
        incineratorOpen = true;
        Debug.Log("Incinerator is open.");
        //Begin animation
    }

    private void CloseIncinerator()
    {
        incineratorOpen = false;
        Debug.Log("Incinerator closed");
        //Begin animation
    }

    private static IEnumerator Incinerate(Corpse corpse)
    {
        // Start Timer or something
        yield return new WaitForSeconds(3f);
        Debug.Log("Corpse incinerated.");
    }

    private void AddPlayer()
    {
        ++numberOfPlayers;
        if(numberOfPlayers == 2)
        {
            OpenIncinerator();
        }
    }

    private void substractPlayer()
    {
        --numberOfPlayers;
        if (incineratorOpen)
        {
            CloseIncinerator();
        }
    }
}

