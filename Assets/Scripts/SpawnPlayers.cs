using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public delegate void PlayerJoinHandler();
public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites = new List<Sprite>();
    [SerializeField] private List<Transform> listPos = new List<Transform>();
    [SerializeField] private List<PlayerInput> players = new List<PlayerInput>();

    private PlayerInputManager playerInputManager;

    public static event PlayerJoinHandler OnAllPlayerJoin;

    private void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        GameObject player = playerInput.gameObject;
        players.Add(playerInput);
        //player.transform.position = listPos[playerInput.playerIndex].position;


        player.GetComponent<SpriteRenderer>().sprite = _sprites[playerInputManager.playerCount - 1];
        player.GetComponent<Player>().id = playerInputManager.playerCount;

        player.gameObject.transform.position = listPos[playerInput.playerIndex].position;

        StartWhenAllPlayer(playerInput);
    }


    private void StartWhenAllPlayer(PlayerInput player)
    {
        if (player.playerIndex == 0)//Changer pour la build et override le player
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].SwitchCurrentActionMap("Player");
                OnAllPlayerJoin?.Invoke();
            }
        }
        else return;
    }
}
