using DG.Tweening;
using System.Collections;
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

    public string griefName = "";
    public float radius = 10f;
    private float griefDuration = 3f;
    private Coroutine feedback;

    public void Awake()
    {
        endLoc = GameObject.FindGameObjectWithTag("GriefEndLoc").GetComponent<Transform>();
        startLoc = GameObject.FindGameObjectWithTag("GriefStartLoc").GetComponent<Transform>();
    }

    private void Start()
    {
        transform.position = startLoc.position;

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
        if (isInteractable)
        {
            gameObject.layer = 0;
            agent.enabled = false;
            requestImg.SetActive(false);
            deathRequest.AcceptRequest();

            player.CarriedObj = this;

            // player carry PNJ
            player.GetComponent<SpriteRenderer>().sprite = player.spriteCarry;
            player.CarriedObj.gameObject.transform.parent = player.transform;
            player.CarriedObj.gameObject.transform.localPosition = transform.up * 4;
            player.CarriedObj.gameObject.transform.DOLocalRotate(new Vector3(0, 0, -90), 1f);

            // can't interact with pnj
            isInteractable = false;
        }
    }

    public override void PutDown(Player player, bool isTimeOut = false)
    {
        gameObject.layer = 7; // -> interactable Layer
        //player.carriedObj.gameObject.SetActive(true);
        player.CarriedObj.gameObject.transform.DOLocalRotate(new Vector3(0, 0, 0), 1f);
        player.CarriedObj.gameObject.transform.parent = null;

        player.GetComponent<SpriteRenderer>().sprite = player.playerNotCarrying;

        // Update name and loc that the pnj wants
        Collider[] infos = Physics.OverlapBox(transform.position, transform.position * radius);
        float min = float.MaxValue;

        for(int i = 0; i < infos.Length; ++i)
        {
            if (infos[i].gameObject.TryGetComponent(out Corpse c))
            {
                float dist = Vector3.Distance(infos[i].gameObject.transform.position, transform.position);
                if (dist < min)
                {
                    min = dist;
                    griefName = c.corpseData.name;
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
            Debug.Log("Name found:" + griefName);
            StartCoroutine(dq.FinishGriefQuest(griefName));
        }
        player.CarriedObj = null;
    }

    public IEnumerator Grieffing()
    {
        yield return new WaitForSeconds(griefDuration);
        StartCoroutine(Walk(false));
    }

    public IEnumerator Walk(bool isWalkingForward)
    {
        isInteractable = false;

        float distToEnd = 0;

        if (!agent.enabled) agent.enabled = true;

        //Arrive Avec sa quete
        if (isWalkingForward)
        {
            requestImg.SetActive(true);
            agent.destination  = endLoc.position;
        }
        //a plus de quete et rentre chez lui
        else
        {
            requestImg.SetActive(false);
            agent.destination = startLoc.position;
        }
        distToEnd = Vector3.Distance(agent.destination, transform.position) / agent.speed;
        yield return new WaitForSeconds(distToEnd);
        if(!isWalkingForward) StartCoroutine(QuestManager.instance.WaitForNewRequest(3, deathRequest));
        isInteractable = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, transform.position * radius);
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
}
