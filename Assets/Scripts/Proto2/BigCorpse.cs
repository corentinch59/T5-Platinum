using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCorpse : MonoBehaviour
{
    private PlayerProto2[] players;
    private float distanceBetweenPlayers;
    private Vector2 player1_move;
    private Vector2 player2_move;

    private void Start()
    {
        players = new PlayerProto2[2];
    }

    private void FixedUpdate()
    {
        player1_move = players[0].getMove;
        player2_move = players[1].getMove;
    }

    public void AttachToCorpse(PlayerProto2 player)
    {
        for (int i = 0; i < players.Length; ++i)
        {
            if (players[i] == null)
            {
                players[i] = player;
            }
        }
    }

    public void DettachFromCorpse(PlayerProto2 player)
    {
        for (int i = 0; i < players.Length; ++i)
        {
            if(player == players[i])
            {
                players[i] = null;
            }
        }
    }
}
