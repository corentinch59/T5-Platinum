using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public delegate void PlayerJoinedhandler(GameObject player, int index);
public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _players = new GameObject[4];
    public GameObject[] players
    {
        get { return _players; }
    }

    private static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<PlayerManager>();

            return _instance;
        }
    }

    private PlayerInputManager playerInputManager;

  

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (_instance == null)
        {
            _instance = this;
        }
        else {
            Destroy(this);
            return;
        }
    }

    public static event PlayerJoinedhandler OnPlayerJoining;


    private void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();

        ChoseYourChara.OnRemovePlayer += ChoseYourChara_OnRemovePlayer;
        StartTheGame.OnStartTheGame += StartTheGame_OnStartTheGame;
    }

    private void StartTheGame_OnStartTheGame()
    {
        playerInputManager.enabled = false;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("PlayerInput ID: " + playerInput.playerIndex);

        //On recupere le GameObject Du player
        GameObject player;
        player = playerInput.gameObject;

        //Ajout du player dans le tableau + on met dans une position libre de gauche a droite
        for (int i = 0; i < _players.Length; i++)
        {
            if (_players[i] == null) //test si une place est libre de gauche a droite 
            {
                _players[i] = player;
                OnPlayerJoining?.Invoke(player, i);
                break;//On break car on a trouve une place
            }
        }
    }

    private void OnPlayerLeft(PlayerInput player)
    {
        //On enleve le player du tableau + on le detruit
        for (int i = 0; i < _players.Length; i++)
        {
            if (_players[i] == player)
            {
                _players[i] = null;

            }
        }
        Destroy(player);
    }

    private void ChoseYourChara_OnRemovePlayer(GameObject player)
    {
        //On enleve le player du tableau + on le detruit
        for (int i = 0; i < _players.Length; i++)
        {
            if (_players[i] == player)
            {
                _players[i] = null;

            }
        }
        Destroy(player);
    }

    private void OnDestroy()
    {
        ChoseYourChara.OnRemovePlayer -= ChoseYourChara_OnRemovePlayer;
    }
}
