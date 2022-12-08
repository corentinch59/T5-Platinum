using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class DigUpPNJInteractable : MonoBehaviour, IInteractable
{
    [Header("Score")]
    [SerializeField][Range(0, 20)] private int scoreToAdd = 5;
    [SerializeField][Range(-20, 0)] private int scoreToRemove = -5;
    private bool pnjActivated; // Temporary

    [Space]
    [SerializeField] private DigUpRequest digUpRequest;
    [SerializeField] private NavMeshAgent agent;

    [Header("Locations")]
    [SerializeField] private Transform questLoc;
    [SerializeField] private Transform returnLoc;

    private Coroutine feedback;

    private void Start()
    {
        transform.position = returnLoc.position;
        gameObject.layer = 0;
    }

    // Temporary
    private void Update()
    {
        if(QuestManager.instance.questFinished.Count > 0 && !pnjActivated)
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
            Destroy(digUpRequest.RequestInUI);
            digUpRequest.RequestCorpseImg.transform.DOScale(0, 0.5f);
            gameObject.layer = 0; // <- can't be interact with
            transform.DOJump(transform.position, 3f, 3, 3f);
            StartCoroutine(Walk(false)); // go back and return later with a new dig up quest

            // Have to detach player from corpse or Big one
            if(c.TryGetComponent(out BigCorpse bc))
            {
                if (bc.Players[1] != null)
                {
                    bc.Interact(bc.Players[1]);
                }
                bc.Interact(bc.Players[0]);
            }
            else
            {
                player.getPlayerMovement.SpriteRenderer.sprite = player.playerNotCarrying;
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
            yield return new WaitForSeconds((Vector3.Distance(transform.position, agent.destination) / agent.speed));
            digUpRequest.SetDigUpRequest();
            gameObject.layer = 7; // <- can be interact with
        }
        //a plus de quete et rentre chez lui
        else
        {
            agent.destination = returnLoc.position;
            yield return new WaitForSeconds(5f);
            // if there are bodies to dig up then continue
            if(QuestManager.instance.questFinished.Count > 0)
            {
                StartCoroutine(Walk(true));
            }
            else
            {
                pnjActivated = false;
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
}
