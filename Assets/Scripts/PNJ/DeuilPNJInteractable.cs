using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeuilPNJInteractable : Carryable
{
    [SerializeField] private DeuilRequest deathRequest;
    [SerializeField] private GameObject requestImg;
    [SerializeField] private GameObject corpseToCreate;
    [SerializeField] private Transform startLoc;
    [SerializeField] private Transform endLoc;
    private bool isInteractable = true;

    public string griefName = "";
    public RequestDataBase.localisation griefLoc = RequestDataBase.localisation.NONE;
    public float radius = 10f;

    public void Awake()
    {
        endLoc = GameObject.FindGameObjectWithTag("GriefEndLoc").GetComponent<Transform>();
        startLoc = GameObject.FindGameObjectWithTag("GriefStartLoc").GetComponent<Transform>();
    }

    private void Start()
    {
       

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            // display quest when arriving in pos
            DisplayQuest();
        }
        /*
        if (Input.GetKeyDown(KeyCode.J))
        {
            transform.position = startLoc.position;
            StartCoroutine(Walk(false));
        }
        */
    }

    private void DisplayQuest()
    {
        requestImg.SetActive(true);
    }

    public override void Interact(PlayerTest player)
    {
        if (isInteractable)
        {
            requestImg.SetActive(false);
            deathRequest.AcceptRequest();

            player.carriedObj = this;
            player.carriedObj.gameObject.SetActive(false);


            //Destroy(this); // enable = false not working

            // move back
            Debug.Log("Quest accepted");
            isInteractable = false;
        }
    }

    public override void PutDown(PlayerTest player)
    {
        if (!isInteractable)
        {
            player.carriedObj.gameObject.SetActive(true);

            // Update name and loc that the pnj wants
            Collider[] infos = Physics.OverlapSphere(transform.position, radius);
            float min = float.MaxValue;

            foreach(Collider info in infos)
            {
                Debug.Log(info.gameObject.name);
                if(info.gameObject.TryGetComponent(out Corpse c))
                {
                    float dist = Vector3.Distance(info.gameObject.transform.position, transform.position);
                    if (dist < min)
                    {
                        min = dist;
                        griefName = c.corpseData.name;
                        griefLoc = c.corpseData.localisation;
                    }
                }
            }

            player.carriedObj.gameObject.transform.position = new Vector3(player.transform.position.x + player.playerMovement.orientationVect.x * 3f,
                    player.transform.position.y, player.transform.position.z + player.playerMovement.orientationVect.y * 3f);

            if (deathRequest.griefQuest.TryGetComponent(out DeuilQuest dq))
            {
                StartCoroutine(dq.FinishGriefQuest(griefName, griefLoc));
            }

            player.carriedObj = null;

            // this.moveback()
            StartCoroutine(Walk(true));
        }
    }
    private IEnumerator Walk(bool isWalkingBack)
    {
        if (isWalkingBack)
        {
            transform.DOMove(startLoc.position, 1);
            requestImg.SetActive(false);
        }
        else
        {
            // il avance
            deathRequest.SetRequest();
            transform.DOMove(endLoc.position, 1);
        }

        yield return new WaitForSeconds(1);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
