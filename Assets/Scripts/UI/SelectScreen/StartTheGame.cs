using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using DG.Tweening;
using TMPro;

public delegate void StartTheGameHandler(GameObject[] players);
public class StartTheGame : MonoBehaviour
{
    [SerializeField] private List<Transform> listPos = new List<Transform>();
    [SerializeField] private GameObject[] players = new GameObject[4];
    [SerializeField] private PlayerInputManager m;
    //[SerializeField] private VisualEffect vEffect;
    [SerializeField] private TextMeshProUGUI timerTMP;

    [Tooltip("Name of the scene to load")][SerializeField] private string nameScene;

    private Tween t;
    private Coroutine currentCoroutine;

    public static event StartTheGameHandler OnStartTheGame;
    
    private void Start()
    {
        ChoseYourChara.OnReady += CheckIfAllPlayerOnReady;
        ChoseYourChara.UnReady += ChoseYourChara_UnReady;
        ChoseYourChara.OnRemovePlayer += ChoseYourChara_OnRemovePlayer;        
    }

    private void CheckIfAllPlayerOnReady()
    {
        //On test si tous les joueurs sont ready
        int x = 0;

        //vEffect.Play();

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null && !players[i].GetComponent<ChoseYourChara>().isReady)
            {
                return;
            }
            else if (players[i] == null) x++;
        }

        if (x == players.Length) return;

        //Lancer un timer 3 2 1 GO et le jeu
        currentCoroutine = StartCoroutine(StartGameCoroutine());
        //StartGameCoroutine();
    }

    private void ChoseYourChara_UnReady()
    {
        if (currentCoroutine != null)
        {
            t.Kill();
            StopCoroutine(StartGameCoroutine());
            StopAllCoroutines();

            timerTMP.gameObject.SetActive(false);
            currentCoroutine = null;
        }
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

        CheckIfAllPlayerOnReady();
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("PlayerInput ID: " + playerInput.playerIndex);

        ChoseYourChara_UnReady();

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

    private IEnumerator StartGameCoroutine()
    {
        timerTMP.gameObject.SetActive(true);
        timerTMP.transform.localScale = Vector3.one;
        timerTMP.text = "3";
        t = timerTMP.transform.DOScale(2f, 1f);
        yield return t.WaitForCompletion();

        timerTMP.transform.localScale =  Vector3.one;
        timerTMP.text = "2";
        t = timerTMP.transform.DOScale(2f, 1f);
        yield return t.WaitForCompletion();


        timerTMP.transform.localScale = Vector3.one;
        timerTMP.text = "1";
        t = timerTMP.transform.DOScale(2f, 1f);
        yield return t.WaitForCompletion();

        timerTMP.text = "GO";



        
        //Changer de scene + repositionner les joueurs
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
            {
                
                players[i].GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
                players[i].GetComponent<ChoseYourChara>().enabled = false;
                players[i].GetComponent<PlayerMovement>().enabled = true;
                players[i].GetComponent<CharacterController>().enabled = true;

                players[i].transform.DOScale(1f, 0.5f);
            }
        }
        ChangeSceneClass.ChangeScene(nameScene);
        currentCoroutine = null;

    }
    private void OnDestroy()
    {
        ChoseYourChara.OnRemovePlayer -= ChoseYourChara_OnRemovePlayer;
        ChoseYourChara.OnReady -= CheckIfAllPlayerOnReady;
        ChoseYourChara.UnReady -= ChoseYourChara_UnReady;
    }
}
