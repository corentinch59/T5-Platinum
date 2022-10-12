using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartTheGame : MonoBehaviour
{

    [SerializeField] private List<Transform> listPos = new List<Transform>();
    [SerializeField] private GameObject[] players = new GameObject[4];
    [SerializeField] private PlayerInputManager m;
    
    
    private void Start()
    {
        ChoseYourChara.OnReady += ChoseYourChara_OnReady;
        ChoseYourChara.OnRemovePlayer += ChoseYourChara_OnRemovePlayer;        
    }

    private void ChoseYourChara_OnRemovePlayer(GameObject player)
    {
        //On enleve le player du tableau + on le detruit
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == player)
            {
                players[i] = null;
            }
        }
        Destroy(player);
    }

    private void ChoseYourChara_OnReady()
    {
        //On test si tous les joueurs sont ready
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null && !players[i].GetComponent<ChoseYourChara>().isReady)
            {
                return;
            }
        }

        Debug.Log("ALL READY");
        //Lancer un timer 3 2 1 GO et le jeu 
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("PlayerInput ID: " + playerInput.playerIndex);

        //On recupere le GameObject Du player
        GameObject player;
        player = playerInput.gameObject;


        //Ajout du player dans le tableau + on met dans une position libre de gauche a droite
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null) //test si une place est libre de gauche a droite 
            {
                players[i] = player;
                player.transform.position = listPos[i].position;
                break;//On break car on a trouve une place
            }
        }
    }

    private void OnDestroy()
    {
        ChoseYourChara.OnRemovePlayer -= ChoseYourChara_OnRemovePlayer;
        ChoseYourChara.OnReady -= ChoseYourChara_OnReady;
    }
}
