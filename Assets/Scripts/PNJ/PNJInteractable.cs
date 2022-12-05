using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class PNJInteractable : MonoBehaviour
{
    [SerializeField] private DigRequest request;
    [SerializeField] private GameObject requestImg;
    [SerializeField] private GameObject corpseToCreate;
    private GameObject corpseCreated;
    public GameObject CorpseCreated => corpseCreated;
    [SerializeField] private NavMeshAgent agent;

    [Header("PnjChecksSpot")]
    [SerializeField] private float radius;
    [SerializeField] private LayerMask corpseLayer;

    private Coroutine feedback;

    [HideInInspector] public Transform returnLoc;
    [HideInInspector] public Transform questLoc;
    private void Update()
    {
        if (agent.hasPath && feedback == null)
        {
            feedback = StartCoroutine(FeedBackPlayerMoves());
        }
    }

    public void AddNewQuest()
    {
        requestImg.SetActive(false);
        request.AcceptDigRequest();

        // spawn Corpse To Bury
        Vector3 spawn = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
        corpseCreated = Instantiate(corpseToCreate, spawn, Quaternion.identity);
        corpseCreated.transform.localScale = Vector3.zero;

        if (corpseCreated.TryGetComponent(out Corpse c))
        {
            // corpseCreated is taking data from the request
            c.ThisQuest = request.quest.GetComponent<Quest>();
            c.CorpseData = c.UpdateRequestLocalisation(true);
            c.PnjFrom = this;
            c.gameObject.layer = 7;
            StartCoroutine(c.DeactivateExclamationPointIfTimeOutQuest(c.ThisQuest.QuestTime)); // <- Deactivate outline at the end of the timer

            // GameFeel
            if ((int)c.ThisQuest.requestInfos.siz > 0)
            {
                // Big corpse
                c.transform.position = new Vector3(c.transform.position.x, c.transform.position.y + 2f, c.transform.position.z);
                corpseCreated.transform.DOScale(new Vector3(2, 2, 2), 0.5f);
                InitBigCorpse(c);
            }
            else
            {
                // small corpse
                c.transform.position = new Vector3(c.transform.position.x, c.transform.position.y + 0.7f, c.transform.position.z);
                corpseCreated.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
            }
        }

        StartCoroutine(Walk(false));
    }

    private void InitBigCorpse(Corpse c)
    {
        c.BigCorpse = c.gameObject.AddComponent<BigCorpse>();
        c.BigCorpse.CarrySpeed = 3f;
        c.BigCorpse.RotationSpeed = 4f;
        c.BigCorpse.AngleThreshold = 20f;
    }

    private bool CheckIfAlreadyACorpse()
    {
        Collider[] thereAreCorpseAround = Physics.OverlapSphere(transform.position, radius, corpseLayer);
        if(thereAreCorpseAround.Length > 0)
        {
            for(int i = 0; i < thereAreCorpseAround.Length;++i)
            {
                if (thereAreCorpseAround[i].TryGetComponent(out Corpse c) && corpseCreated.TryGetComponent(out Corpse cc) && cc.CorpseData.corpseType == c.CorpseData.corpseType)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public IEnumerator Walk(bool isWalkingForward)
    {
        //Arrive Avec sa quete
        if (isWalkingForward)
        {
            requestImg.SetActive(true);
            agent.destination = questLoc.position;
            yield return new WaitForSeconds((Vector3.Distance(transform.position, agent.destination) / agent.speed));
            if (!CheckIfAlreadyACorpse())
            {
                AddNewQuest();
            }
            else
            {
                Debug.Log("Il y a un corps là");
                GameManager.Instance.NewPNJComingWithQuest();
                yield return StartCoroutine(Walk(false));
            }
        }
        //a plus de quete et rentre chez lui
        else
        {
            requestImg.SetActive(false);
            //QuestManager.instance.activeQuests.Remove(request.requestInfo);
            //QuestManager.instance.questFinished.Add(request.requestInfo);
            agent.destination = returnLoc.position;
            yield return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
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
}
