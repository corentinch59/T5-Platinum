using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BigCorpse : MonoBehaviour, IInteractable
{
    [SerializeField] private float carrySpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float angleThreshold;

    private Player[] players;
    private Vector2 player1_move;
    private Vector2 player2_move;
    private float distanceBetweenPlayers;
    private CharacterController controller;

    float timeCounter;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();    
    }

    private void Start()
    {
        players = new Player[2];
    }

    private void Update()
    {
        if (players[0] != null)
            player1_move = players[0].getPlayerMovement.getMove;

        if (players[1] != null)
            player2_move = players[1].getPlayerMovement.getMove;

    }

    private void FixedUpdate()
    {
        if (players[0] != null || players[1] != null)
        {
            Vector3 corpseMovement = new Vector3(player1_move.x + player2_move.x, 0, player1_move.y + player2_move.y);
            controller.Move(corpseMovement * carrySpeed * Time.fixedDeltaTime);
        }

        if (players[0] != null)
        {
            timeCounter += Time.deltaTime;
            float x = Mathf.Cos(timeCounter * 2f) * 1f;
            float z = Mathf.Sin(timeCounter * 2f) * 1f;
            transform.position +=
                new Vector3(
                    x,
                    0,
                    -z
                    );
        }

        //players[0].getPlayerMovement.getController.Move(nextPosition);
    }

    public void AttachToCorpse(Player player)
    {
        for (int i = 0; i < players.Length; ++i)
        {
            if (players[i] == null)
            {
                players[i] = player;
                if(i == 1)
                {
                    Vector2 dirBetweenPlayers = players[0].transform.position - players[1].transform.position;
                    distanceBetweenPlayers = dirBetweenPlayers.magnitude;
                }
                break;
            }
        }
        player.getPlayerMovement.canMove = false;
        
        player.transform.parent = transform;
        Debug.Log($"Attached {player.name} to a big corpse.");
    }

    public void DetachFromCorpse(Player player)
    {
        for (int i = 0; i < players.Length; ++i)
        {
            if(player == players[i])
            {
                players[i] = null;
            }
        }
        player.getPlayerMovement.canMove = true;
        player.transform.parent = null;
        Debug.Log($"detached {player.name} from a big corpse.");
    }

    public void Interact(Player player)
    {
        if (players.Contains(player))
        {
            DetachFromCorpse(player);
        }
        else
        {
            AttachToCorpse(player);
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine()
    }
}
