using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class InitPlayersInGame : MonoBehaviour
{
    [SerializeField] private List<Transform> listPos = new List<Transform>();
    [SerializeField] private GameObject[] players = new GameObject[4];
    [SerializeField] private CinemachineTargetGroup targetGroupCam;


    private void Start()
    {
        if (PlayerManager.Instance == null) return;

        players = PlayerManager.Instance.players;

        //Ajout du player dans le tableau + on met dans une position libre de gauche a droite
        for (int i = 0; i < PlayerManager.Instance.players.Length; i++)
        {
            if (PlayerManager.Instance.players[i] != null) //test si une place est libre de gauche a droite 
            {
                Debug.Log(PlayerManager.Instance.players[i] + " " + i);
                //players[i] = player;
                PlayerManager.Instance.players[i].transform.position = listPos[i].position;
                targetGroupCam.AddMember(PlayerManager.Instance.players[i].transform, 1f, 0f);
            }
        }
    }
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("PlayerInput ID: " + playerInput.playerIndex);

        //ChoseYourChara_UnReady();

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
                targetGroupCam.AddMember(player.transform, 1f, 0f);
                break;//On break car on a trouve une place
            }
        }
    }
}
