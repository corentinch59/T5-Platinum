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

    private bool isInteractable = true;

    public string griefName = "";
    public float radius = 10f;
    private float griefDuration = 3f;

    public void Awake()
    {
        endLoc = GameObject.FindGameObjectWithTag("GriefEndLoc").GetComponent<Transform>();
        startLoc = GameObject.FindGameObjectWithTag("GriefStartLoc").GetComponent<Transform>();
    }

    private void Start()
    {
        transform.position = startLoc.position;
        //StartCoroutine(Walk(true));

    }

    private void Update()
    {
        //agent.
        /*
        if (Input.GetKeyDown(KeyCode.O))
        {
            // display quest when arriving in pos
            DisplayQuest();
        }
        
        if (Input.GetKeyDown(KeyCode.J) && QuestManager.instance.questFinished.Count > 0)
        {
            transform.position = startLoc.position;
            StartCoroutine(Walk(true));
        }
        */
    }

    private void DisplayQuest()
    {
        requestImg.SetActive(true);
    }

    //public override void Interact(PlayerTest player)
    //{
    //    if (isInteractable)
    //    {
    //        agent.enabled = false;
    //        requestImg.SetActive(false);
    //        deathRequest.AcceptRequest();

    //        player.carriedObj = this;
    //        player.interactableObj = null;
    //        player.isCarrying = true;

    //        // player carry PNJ
    //        player.GetComponent<SpriteRenderer>().sprite = player.spriteCarry;
    //        player.carriedObj.gameObject.transform.parent = player.transform;
    //        player.carriedObj.gameObject.transform.localPosition = transform.up * 14;
    //        player.carriedObj.gameObject.transform.DOLocalRotate(new Vector3(0, 0, -90), 1f);
    //        //player.carriedObj.gameObject.SetActive(false);

    //        //Destroy(this); // enable = false not working

    //        // move back
    //        isInteractable = false;
    //    }
    //}

    //public override void PutDown(PlayerTest player, bool isTimeOut = false)
    //{
    //    if (!isInteractable)
    //    {
    //        //player.carriedObj.gameObject.SetActive(true);
    //        player.carriedObj.gameObject.transform.DOLocalRotate(new Vector3(0, 0, 0), 1f);
    //        player.carriedObj.gameObject.transform.parent = null;

    //        player.isCarrying = false;

    //        player.GetComponent<SpriteRenderer>().sprite = player.playerNotCarrying;
    //        // Update name and loc that the pnj wants
    //        Collider[] infos = Physics.OverlapSphere(transform.position, radius);
    //        float min = float.MaxValue;

    //        foreach (Collider info in infos)
    //        {
    //            //Debug.Log(info.gameObject.name);
    //            if (info.gameObject.TryGetComponent(out Corpse c))
    //            {
    //                float dist = Vector3.Distance(info.gameObject.transform.position, transform.position);
    //                if (dist < min)
    //                {
    //                    min = dist;
    //                    griefName = c.corpseData.name;
    //                }
    //            }
    //        }

    //        player.carriedObj.gameObject.transform.position = new Vector3(player.transform.position.x + player.playerMovement.getOrientation.x * 3f,
    //                player.transform.position.y, player.transform.position.z + player.playerMovement.getOrientation.y * 3f);

    //        if (!isTimeOut)
    //        {
    //            if (deathRequest.griefQuest.TryGetComponent(out GriefQuest dq))
    //            {
    //                StartCoroutine(dq.FinishGriefQuest(griefName));
    //            }
    //        }

    //        player.carriedObj = null;

    //        // this.moveback()
    //    }
    //}

    public IEnumerator Grieffing()
    {
        yield return new WaitForSeconds(griefDuration);
        StartCoroutine(Walk(false));
    }

    public IEnumerator Walk(bool isWalkingForward)
    {
        float distToEnd = 0;

        if (!agent.enabled) agent.enabled = true;

        //Arrive Avec sa quete
        if (isWalkingForward)
        {
            requestImg.SetActive(true);
            agent.destination  = endLoc.position;

            //transform.DOMove(endLoc.position, 2);
            //yield return new WaitForSeconds(2);
        }
        //a plus de quete et rentre chez lui
        else
        {
            requestImg.SetActive(false);
            agent.destination = startLoc.position;

            //transform.DOMove(startLoc.position, 2);
            //yield return new WaitForSeconds(2);
        }
        distToEnd = Vector3.Distance(agent.destination, transform.position) / agent.speed;
        yield return new WaitForSeconds(distToEnd);
        if(!isWalkingForward) StartCoroutine(QuestManager.instance.WaitForNewRequest(3, deathRequest));
        isInteractable = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
