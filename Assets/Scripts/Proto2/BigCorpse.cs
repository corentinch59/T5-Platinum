using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class BigCorpse : MonoBehaviour, IInteractable
{
    [SerializeField] private float carrySpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float angleThreshold;

    private Player[] players;
    private Vector2 player1_move;
    private Vector2 player2_move;
    private float distanceBetweenPlayers;
    private float distanceNoPlayer;
    private CharacterController controller;

    private Vector3 corpseMovement;
    private Vector3 directionP1;
    private Vector3 directionP2;
    private Vector2 directionP1Normalized;
    private Vector2 directionP2Normalized;

    #region Get/Set
    public Player[] Players { get { return players; } set { players = value; } }
    public CharacterController Controller { get { return controller; } set { controller = value; } }
    public float CarrySpeed { get { return carrySpeed; } set { carrySpeed = value; } }
    public float RotationSpeed { get { return rotationSpeed; } set { rotationSpeed = value; } }
    public float AngleThreshold { get { return angleThreshold; } set { angleThreshold = value; } }
    #endregion

    private void Start()
    {
        players = new Player[2];
        controller = GetComponent<CharacterController>();
        controller.enabled = true;
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
        corpseMovement = new Vector3(player1_move.x + player2_move.x, 0, player1_move.y + player2_move.y);

        if(controller != null)
            controller.Move(corpseMovement * carrySpeed * Time.fixedDeltaTime);

        if (players[1] != null)
        {
            Vector3 direction = players[0].transform.position - players[1].transform.position;
            transform.position = (direction / 2) + players[1].transform.position;

           /* float rotationAngle = Vector2.SignedAngle(Vector2.up, new Vector2(direction.x, direction.z));
            Vector3 tempRotation = transform.localEulerAngles;
            tempRotation.y = -rotationAngle;
            transform.localEulerAngles = tempRotation;*/
        }
        else if (players[0] != null)
        {
            Vector3 direction = players[0].transform.position - transform.position;

            /*float rotationAngle = Vector2.SignedAngle(Vector2.up, new Vector2(direction.x, direction.z));
            Vector3 tempRotation = transform.localEulerAngles;
            tempRotation.y = -rotationAngle;
            transform.localEulerAngles = tempRotation;*/
        }

        if (players[0] != null)
        {
            players[0].getPlayerMovement.getController.Move(corpseMovement * carrySpeed * Time.fixedDeltaTime);
        }

        if (players[1] != null)
        {
            players[1].getPlayerMovement.getController.Move(corpseMovement * carrySpeed * Time.fixedDeltaTime);

            if (player1_move != Vector2.zero && player2_move != Vector2.zero && Vector2.Angle(player1_move, player2_move) <= angleThreshold)
            {

            }
            else
            {
                PerformMovementRotation();
            }
        }
        else
        {
            PerformMovementRotation();
        }
    }
    private void PerformMovementRotation()
    {
        if (player1_move != Vector2.zero)
        {
            directionP1Normalized = player1_move.normalized;

            if (players[1] != null)
            {
                Vector3 targetPosition = new Vector3(
                    players[1].transform.position.x + directionP1Normalized.x * distanceBetweenPlayers,
                    0,
                    players[1].transform.position.z + directionP1Normalized.y * distanceBetweenPlayers
                    );

                Vector3 tempPos = (targetPosition - players[0].transform.position).normalized * rotationSpeed * Time.fixedDeltaTime;

                Vector3 rightDirection = ((tempPos + players[0].transform.position) - players[1].transform.position).normalized * distanceBetweenPlayers;

                directionP1 = (rightDirection + players[1].transform.position) - players[0].transform.position;
            }
            else
            {
                Vector3 targetPosition = new Vector3(
                    transform.position.x + directionP1Normalized.x * distanceNoPlayer,
                    0,
                    transform.position.z + directionP1Normalized.y * distanceNoPlayer
                    );

                Vector3 tempPos = (targetPosition - players[0].transform.position).normalized * rotationSpeed * Time.fixedDeltaTime;

                Vector3 rightDirection = ((tempPos + players[0].transform.position) - transform.position).normalized * distanceNoPlayer;

                directionP1 = (rightDirection + transform.position) - players[0].transform.position;
            }

            players[0].getPlayerMovement.getController.Move(directionP1);
        }

        if (players[1] != null && player2_move != Vector2.zero)
        {
            directionP2Normalized = player2_move.normalized;
            Vector3 targetPosition = new Vector3(
                    players[0].transform.position.x + directionP2Normalized.x * distanceBetweenPlayers,
                    0,
                    players[0].transform.position.z + directionP2Normalized.y * distanceBetweenPlayers
                    );

            Vector3 tempPos = (targetPosition - players[1].transform.position).normalized * rotationSpeed * Time.fixedDeltaTime;

            Vector3 rightDirection = ((tempPos + players[1].transform.position) - players[0].transform.position).normalized * distanceBetweenPlayers;

            directionP2 = (rightDirection + players[0].transform.position) - players[1].transform.position;

            players[1].getPlayerMovement.getController.Move(directionP2);
        }
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
                    gameObject.layer = 0; // <- not interactable for now
                }
                else
                {
                    Vector3 dirNoPlayer = transform.position - players[0].transform.position;
                    distanceNoPlayer = dirNoPlayer.magnitude;
                }
                break;
            }
        }
        player.getPlayerMovement.canMove = false;
        //Debug.Log($"Attached {player.name} to a big corpse.");
    }

    public void DetachFromCorpse(Player player)
    {
        for (int i = 0; i < players.Length; ++i)
        {
            if(player == players[i])
            {
                if(i == 0 && players[1] != null)
                {
                    players[0] = players[1];
                    players[1] = null;
                }
                else
                {
                    players[i] = null;
                }
            }
        }
        player1_move = Vector2.zero;
        player2_move = Vector2.zero;
        player.getPlayerMovement.canMove = true;
        Debug.Log($"Detached {player.name} from a big corpse.");
    }

    public void SetVibrations(PlayerInput playerInput, float frequencyLeftHaptic, float frequencyRightHaptic)
    {
    }

    public IEnumerator SetVibrationsCoroutine(PlayerInput playerInput, float frequencyLeftHaptic, float frequencyRightHaptic)
    {
        yield break;
    }



    //private void OnDrawGizmos()
    //{
    //P1 Debug
    //if (players[0] != null && players[1] != null)
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(players[0].transform.position, players[1].transform.position);

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawLine(players[0].transform.position, new Vector3(player1_move.x, 0, player1_move.y) + players[0].transform.position);
    //    Gizmos.DrawLine(players[1].transform.position, new Vector3(player1_move.x, 0, player1_move.y) + players[1].transform.position);

    //    Gizmos.color = Color.blue;
    //    Vector3 targetPosition = new Vector3(
    //        players[1].transform.position.x + directionNormalized.x * distanceBetweenPlayers,
    //        0,
    //        players[1].transform.position.z + directionNormalized.y * distanceBetweenPlayers
    //        );
    //    Gizmos.DrawLine(players[1].transform.position, targetPosition);

    //    Gizmos.color = Color.magenta;
    //    Vector3 tempPos = (targetPosition - players[0].transform.position).normalized * rotationSpeed * Time.fixedDeltaTime;
    //    Gizmos.DrawLine(players[0].transform.position, tempPos + players[0].transform.position);

    //    Gizmos.color = Color.black;
    //    Vector3 rightDirection = ((tempPos + players[0].transform.position) - players[1].transform.position).normalized * distanceBetweenPlayers;
    //    Gizmos.DrawLine(players[1].transform.position, rightDirection + players[1].transform.position);

    //    Gizmos.color = Color.yellow;
    //    Vector3 direction = (rightDirection + players[1].transform.position) - players[0].transform.position;
    //    Gizmos.DrawLine(players[0].transform.position, direction + players[0].transform.position);
    //}

    // P2 debug
    //if (players[0] != null && players[1] != null)
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(players[0].transform.position, players[1].transform.position);

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawLine(players[0].transform.position, new Vector3(player2_move.x, 0, player2_move.y) + players[0].transform.position);
    //    Gizmos.DrawLine(players[1].transform.position, new Vector3(player2_move.x, 0, player2_move.y) + players[1].transform.position);

    //    Gizmos.color = Color.blue;
    //    Vector3 targetPosition = new Vector3(
    //                players[0].transform.position.x + directionP2Normalized.x * distanceBetweenPlayers,
    //                0,
    //                players[0].transform.position.z + directionP2Normalized.y * distanceBetweenPlayers
    //                );
    //    Gizmos.DrawLine(players[0].transform.position, targetPosition);
    //}
    //}

}
