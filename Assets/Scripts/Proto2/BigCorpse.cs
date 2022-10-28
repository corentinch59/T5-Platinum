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

    private Vector2 directionNormalized;

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
            players[0].getPlayerMovement.getController.Move(corpseMovement * carrySpeed * Time.fixedDeltaTime);
            players[1].getPlayerMovement.getController.Move(corpseMovement * carrySpeed * Time.fixedDeltaTime);
        }

        if (players[0] != null && players[1] != null)
        {
            if(player1_move != Vector2.zero)
            {
                directionNormalized = player1_move.normalized;
                Vector3 targetPosition = new Vector3(
                    players[1].transform.position.x + directionNormalized.x * distanceBetweenPlayers,
                    0,
                    players[1].transform.position.z + directionNormalized.y * distanceBetweenPlayers
                    );

                Vector3 tempPos = (targetPosition - players[0].transform.position).normalized * rotationSpeed * Time.fixedDeltaTime;

                Vector3 rightDirection = ((tempPos + players[0].transform.position) - players[1].transform.position).normalized * distanceBetweenPlayers;

                Vector3 direction = (rightDirection + players[1].transform.position) - players[0].transform.position;

                players[0].getPlayerMovement.getController.Move(direction);
            }
        }
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
                    Vector3 dirBetweenPlayers = players[0].transform.position - players[1].transform.position;
                    distanceBetweenPlayers = dirBetweenPlayers.magnitude;
                }
                break;
            }
        }
        player.getPlayerMovement.canMove = false;
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
        if (players[0] != null && players[1] != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(players[0].transform.position, players[1].transform.position);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(players[0].transform.position, new Vector3(player1_move.x, 0, player1_move.y) + players[0].transform.position);
            Gizmos.DrawLine(players[1].transform.position, new Vector3(player1_move.x, 0, player1_move.y) + players[1].transform.position);

            Gizmos.color = Color.blue;
            Vector3 targetPosition = new Vector3(
                players[1].transform.position.x + directionNormalized.x * distanceBetweenPlayers,
                0,
                players[1].transform.position.z + directionNormalized.y * distanceBetweenPlayers
                );
            Gizmos.DrawLine(players[1].transform.position, targetPosition);

            Gizmos.color = Color.magenta;
            Vector3 tempPos = (targetPosition - players[0].transform.position).normalized * rotationSpeed * Time.fixedDeltaTime;
            Gizmos.DrawLine(players[0].transform.position, tempPos + players[0].transform.position);

            Gizmos.color = Color.black;
            Vector3 rightDirection = ((tempPos + players[0].transform.position) - players[1].transform.position).normalized * distanceBetweenPlayers;
            Gizmos.DrawLine(players[1].transform.position, rightDirection + players[1].transform.position);

            Gizmos.color = Color.yellow;
            Vector3 direction = (rightDirection + players[1].transform.position) - players[0].transform.position;
            Gizmos.DrawLine(players[0].transform.position, direction + players[0].transform.position);
        }
    }
}
