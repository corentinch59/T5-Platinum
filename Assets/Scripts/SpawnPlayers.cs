using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites = new List<Sprite>();
    
    private PlayerInputManager playerInputManager;




    private void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        GameObject player = playerInput.gameObject;

        player.GetComponent<SpriteRenderer>().sprite = _sprites[playerInputManager.playerCount - 1];
        player.GetComponent<Player>().id = playerInputManager.playerCount;
    }
}
