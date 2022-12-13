using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.AI;

public class GriefPNJInteractable : Carryable
{
    [SerializeField] private GriefRequest deathRequest;
    [SerializeField] private GameObject requestImg;
    [SerializeField] private GameObject corpseToCreate;
    [SerializeField] private Transform startLoc;
    [SerializeField] private Transform endLoc;
    [SerializeField] private NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    private bool isInteractable = true;

    private bool questActivated;

    public RequestDataBase.corpseType griefCorpseType = RequestDataBase.corpseType.NONE;
    public float radius = 10f;
    private float griefDuration = 3f;
    private Coroutine feedback;
    private Coroutine finishGriefQuest;

    public void Awake()
    {
        endLoc = GameObject.FindGameObjectWithTag("GriefEndLoc").GetComponent<Transform>();
        startLoc = GameObject.FindGameObjectWithTag("GriefStartLoc").GetComponent<Transform>();
        requestImg.SetActive(false);
    }

    private void Start()
    {
        transform.position = startLoc.position;
        UIGameOver.onGameOver += UIGameOver_onGameOver;
    }

    private void UIGameOver_onGameOver()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        if (agent.isOnNavMesh)
        {
            if (!agent.isStopped && feedback == null)
            {
                feedback = StartCoroutine(FeedBackPlayerMoves());
            }
        }
    }

    public override void Interact(Player player)
    {
        // check if isinteractable and avoid player to take him if he didn't finish his coroutine
        if (isInteractable && finishGriefQuest == null)
        {
            agent.enabled = false;
            requestImg.SetActive(false);

            // Avoid pnj to activate twice the same quest
            if (!questActivated)
            {
                deathRequest.AcceptRequest();
                questActivated = true;
            }

            player.CarriedObj = this;

            // player carry PNJ
            //player.GetComponent<SpriteRenderer>().sprite = player.spriteCarry;
            player.CarriedObj.gameObject.transform.parent = player.transform;
            player.CarriedObj.gameObject.transform.localPosition = transform.up * 4;
            player.CarriedObj.gameObject.transform.DOLocalRotate(new Vector3(0, 0, -90), 1f);

            // can't interact with pnj
            isInteractable = false;
        }
    }

    public override void PutDown(Player player, bool isTimeOut = false)
    {
        // Give Interactable layer back to be able to interact with him
        gameObject.layer = 7;

        player.CarriedObj.gameObject.transform.DOLocalRotate(new Vector3(0, 0, 0), 1f);
        player.CarriedObj.gameObject.transform.parent = null;

        // Update corpseType the pnj wants
        Collider[] infos = Physics.OverlapSphere(transform.position, radius);
        float min = float.MaxValue;

        for(int i = 0; i < infos.Length; ++i)
        {
            if (infos[i].gameObject.TryGetComponent(out Hole h))
            {
                float dist = Vector3.Distance(infos[i].gameObject.transform.position, transform.position);
                if (dist < min)
                {
                    min = dist;
                    griefCorpseType = h.HeldCorpse.CorpseData.corpseType;
                }
            }
        }

        player.CarriedObj.gameObject.transform.position = new Vector3(player.transform.position.x + player.getPlayerMovement.getOrientation.x * 3f,
                player.transform.position.y, player.transform.position.z + player.getPlayerMovement.getOrientation.y * 3f);

        player.CarriedObj = null;

        isInteractable = true;
    }

    public void CheckLocationWanted(Player player)
    {
        PutDown(player);
        if (deathRequest.griefQuest.TryGetComponent(out GriefQuest dq))
        {
            finishGriefQuest = StartCoroutine(dq.FinishGriefQuest(griefCorpseType));
        }
        player.CarriedObj = null;
    }

    public IEnumerator Grieffing()
    {
        // Pnj has a player as a parent so we put it down
        if(transform.parent != null && transform.parent.TryGetComponent(out Player player))
        {
            PutDown(player);
        }
        yield return new WaitForSeconds(griefDuration);
        StartCoroutine(Walk(false));
    }

    public IEnumerator Walk(bool isWalkingForward)
    {
        isInteractable = false;

        if (!agent.enabled) 
            agent.enabled = true;

        //Arrive Avec sa quete
        if (isWalkingForward)
        {
            gameObject.layer = 7;
            agent.destination  = endLoc.position;
        }
        //a plus de quete et rentre chez lui
        else
        {
            gameObject.layer = 0;
            agent.destination = startLoc.position;
        }
        float distToEnd = Vector3.Distance(agent.destination, transform.position) / agent.speed;
        yield return new WaitForSeconds(distToEnd);

        if (!isWalkingForward)
        {
            requestImg.SetActive(false);
            StartCoroutine(QuestManager.instance.WaitForNewRequest(3, deathRequest));
        }
        else
        {
            requestImg.SetActive(true);
        }

        isInteractable = true;
        questActivated = false;
        finishGriefQuest = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
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

    private void OnDestroy()
    {
        UIGameOver.onGameOver -= UIGameOver_onGameOver;
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
