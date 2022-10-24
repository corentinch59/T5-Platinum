using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using DG.Tweening;
using TMPro;

public delegate void StartTheGameHandler();
public class StartTheGame : MonoBehaviour
{
    [SerializeField] private List<Transform> listPos = new List<Transform>();
    [SerializeField] private List<ChoseYourChara> selectionPlayers = new List<ChoseYourChara>();
    //[SerializeField] private PlayerInputManager m;
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
        PlayerManager.OnPlayerJoining += PlayerManager_OnPlayerJoining;
    }

    private void PlayerManager_OnPlayerJoining(GameObject player, int index)
    {
        ChoseYourChara_UnReady();
        player.transform.position = listPos[index].position;
        selectionPlayers.Add(player.GetComponent<ChoseYourChara>());
    }

    private void CheckIfAllPlayerOnReady()
    {
        //On test si tous les joueurs sont ready
        int x = 0;

        //vEffect.Play();

        for (int i = 0; i < selectionPlayers.Count; i++)
        {
            if (selectionPlayers[i] != null && !selectionPlayers[i].isReady)
            {
                return;
            }
            //else if (selectionPlayers[i] == null) x++;
        }

        if (x == selectionPlayers.Count) return;

        //Lancer un timer 3 2 1 GO et le jeu
        currentCoroutine = StartCoroutine(StartGameCoroutine());
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

        OnStartTheGame?.Invoke();
        ChangingScene();
        currentCoroutine = null;

    }

    private void ChangingScene()
    {
        for (int i = 0; i < selectionPlayers.Count; i++)
        {
            if (selectionPlayers[i] != null)
            {
                selectionPlayers[i].GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");

                selectionPlayers[i].GetComponent<PlayerMovement>().enabled = true;
                selectionPlayers[i].GetComponent<PlayerTest>().enabled = true;
                selectionPlayers[i].GetComponent<CharacterController>().enabled = true;
                selectionPlayers[i].transform.DOScale(1f, 0.5f);

                Destroy(selectionPlayers[i]);
            }
        }
        ChangeSceneClass.ChangeScene(nameScene);
    }
    private void OnDestroy()
    {
        ChoseYourChara.OnReady -= CheckIfAllPlayerOnReady;
        ChoseYourChara.UnReady -= ChoseYourChara_UnReady;
        PlayerManager.OnPlayerJoining -= PlayerManager_OnPlayerJoining;
    }
}
