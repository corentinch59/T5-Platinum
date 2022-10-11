using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartTheGame : MonoBehaviour
{

    [SerializeField] private List<Transform> listPos = new List<Transform>();
    [SerializeField] public List<GameObject> players = new List<GameObject>();
    [SerializeField] private PlayerInputManager m;
    
    
    private void Start()
    {
        ChoseYourChara.OnReady += ChoseYourChara_OnReady;
    }

    private void ChoseYourChara_OnReady()
    {
        //On test si tous les joueurs sont ready
        //foreach (ChoseYourChara chara in listPos)
        //{
        //    if (!chara.isReady) return;
        //}

        Debug.Log("ALL READY");
        //Lancer un timer 3 2 1 GO et le jeu 
    }

    public void OnCreateCharacter(PlayerInput playerInput)
    {
        //GameObject MANANA = Instantiate(eeeeee);
        //MANANA.transform.position = new Vector3(5f, 0f, 0f);
        Debug.Log("PlayerInput ID: " + playerInput.playerIndex);

        players.Add(playerInput.gameObject);
        
        switch (m.playerCount)
        {
            case 1:
                m.playerPrefab.transform.position = listPos[0].position;
                break;
            case 2:
                m.playerPrefab.transform.position = listPos[1].position;
                break;
            case 3:
                m.playerPrefab.transform.position = listPos[2].position;
                break;
            case 4:
                m.playerPrefab.transform.position = listPos[3].position;
                break;
            default:
                m.playerPrefab.transform.position = listPos[0].position;
                break;
        }
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("PlayerInput ID: " + playerInput.playerIndex);

        GameObject player;
        player = playerInput.gameObject;

        players.Add(player);


        switch (m.playerCount)
        {
            case 1:
                player.transform.position = listPos[0].position;
                break;
            case 2:
                player.transform.position = listPos[1].position;
                break;
            case 3:
                player.transform.position = listPos[2].position;
                break;
            case 4:
                player.transform.position = listPos[3].position;
                break;
            default:
                player.transform.position = listPos[0].position;
                break;
        }
    }

    private void OnDestroy()
    {
        ChoseYourChara.OnReady -= ChoseYourChara_OnReady;
    }
}
