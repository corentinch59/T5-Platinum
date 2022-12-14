using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class DigUpPNJInteractable : MonoBehaviour, IInteractable
{
    [Header("Score")]
    private bool pnjActivated; // Temporary

    [Space]
    [SerializeField] private DigUpRequest digUpRequest;
    [SerializeField] private NavMeshAgent agent;

    [Header("Locations")]
    [SerializeField] private Transform questLoc;
    [SerializeField] private Transform returnLoc;
    [Header("Outline")]
    [SerializeField] private GameObject outlineObject;
    

    [Header("Timer")]
    [SerializeField] private float timerPnjComes;
    private float timerPnjComesSet;

    private Coroutine feedback;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        timerPnjComesSet = timerPnjComes;
        transform.position = returnLoc.position;
        outlineObject.SetActive(false);
        gameObject.layer = 0;
    }

    // Temporary
    private void Update()
    {
        // First instance
        timerPnjComes -= Time.deltaTime;
        if(QuestManager.instance.questFinished.Count > 0 && timerPnjComes <= 0 && !pnjActivated)
        {
            pnjActivated = true;
            StartCoroutine(Walk(true));
            
        }

        if (agent.isOnNavMesh)
        {
            if (!agent.isStopped && feedback == null)
            {
                feedback = StartCoroutine(FeedBackPlayerMoves());
            }
        }
    }

    public void Interact(Player player)
    {
        // if good quest : same body as asked
        if(player.CarriedObj.TryGetComponent(out Corpse c) && c.CorpseData.corpseType == digUpRequest.RequestInfo.corps)
        {
            if(player.TiredVFX != null)
            {
                player.TiredVFX.Stop();
            }
            Destroy(digUpRequest.RequestInUI);
            gameObject.layer = 0; // <- can't be interact with
            Tween jump = transform.DOJump(transform.position, 3f, 3, 3f);
            jump.onComplete += () => { StartCoroutine(Walk(false)); };// go back and return later with a new dig up quest

            // Have to detach player from corpse or Big one
            if(c.TryGetComponent(out BigCorpse bc))
            {
                if (bc.Players[1] != null)
                {
                    bc.Interact(bc.Players[1]);
                }
                bc.Interact(bc.Players[0]);
            }
            Destroy(player.CarriedObj.gameObject);

            QuestManager.instance.activeDigUpQuests.Remove(digUpRequest.RequestInfo);
            digUpRequest.RequestInfo = null;
            QuestManager.instance.UpdateScore(true);

            # region stop drag sound
            SoundManager.instance.Stop("DragMud");
            SoundManager.instance.Stop("DragStone");
            SoundManager.instance.Stop("DragDirt");
            #endregion
        }
        // if not good quest -> score-- but stays
        else
        {
            transform.DOShakePosition(3f, new Vector3(2, 0, 0), 5, 10, false, true, ShakeRandomnessMode.Harmonic);
            QuestManager.instance.UpdateScore(false);
        }
    }

    public IEnumerator Walk(bool isWalkingForward)
    {
        //Arrive Avec sa quete
        if (isWalkingForward)
        {
            agent.destination = questLoc.position;
            spriteRenderer.flipX = false;
            yield return new WaitForSeconds((Vector3.Distance(transform.position, agent.destination) / agent.speed));
            outlineObject.SetActive(true);
            digUpRequest.SetDigUpRequest();
            gameObject.layer = 7; // <- can be interact with
        }
        //a plus de quete et rentre chez lui
        else
        {
            outlineObject.SetActive(false);
            agent.destination = returnLoc.position;
            spriteRenderer.flipX = true;
            yield return new WaitForSeconds(5f);
            // if there are bodies to dig up then continue
            if(QuestManager.instance.questFinished.Count > 0)
            {
                StartCoroutine(Walk(true));
            }
            else
            {
                pnjActivated = false;
                timerPnjComes = timerPnjComesSet;
            }
        }
    }

    private IEnumerator FeedBackPlayerMoves()
    {
        transform.DOScaleX(1.8f, 0.3f);
        transform.DOScaleY(2.3f, 0.3f);
        yield return new WaitForSeconds(0.3f);
        transform.DOScaleX(2f, 0.3f);
        transform.DOScaleY(2f, 0.3f);
        yield return new WaitForSeconds(0.3f);
        feedback = null;
    }
    
        public List<int> playersID;
    [HideInInspector] public int numbersOfPlayers;
    [HideInInspector] public bool isOutline;
    
    public void AddInteractablePlayers(int playerID)
    {
        playersID.Add(playerID);
        numbersOfPlayers++;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetFloat("_NumberOfPlayers", numbersOfPlayers);
        UpdateOutline(spriteRenderer);
        if(!isOutline)
            ActivateOutline(spriteRenderer);
    }

    public void UpdateOutline(SpriteRenderer renderer)
    {
        switch (numbersOfPlayers)
        {
            case 1:
            {
                renderer.material.SetVector("_IDs", new Vector4(playersID[0], 0,0,0));
                break;
            }
            case 2:
            {
                renderer.material.SetVector("_IDs", new Vector4(playersID[0], playersID[1],0,0));
                break;
            }
            case 3:
            {
                renderer.material.SetVector("_IDs", new Vector4(playersID[0], playersID[1],playersID[2],0));
                break;
            }
            case 4:
            {
                renderer.material.SetVector("_IDs", new Vector4(playersID[0], playersID[1],playersID[2],playersID[3]));
                break;
            }
        }
    }

    private void ActivateOutline(SpriteRenderer renderer)
    {
        isOutline = true;
        renderer.material.SetFloat("_IsOuline", 1);
    }
    
    public void RemoveInteractablePlayer(int playerID)
    {
        playersID.Remove(playerID);
        numbersOfPlayers--;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateOutline(spriteRenderer);
        spriteRenderer.material.SetFloat("_NumberOfPlayers", numbersOfPlayers);
        if (isOutline && numbersOfPlayers <= 0)
        {
            numbersOfPlayers = 0;
            DesactivateOutline(spriteRenderer);
        }
    }

    private void DesactivateOutline(SpriteRenderer renderer)
    {
        isOutline = false;
        renderer.material.SetFloat("_PlayerInterractableID", 0);
        renderer.material.SetFloat("_IsOuline", 0);
    }
}
