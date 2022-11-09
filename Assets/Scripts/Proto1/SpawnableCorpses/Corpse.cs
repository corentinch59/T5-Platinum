using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Corpse : Carryable
{
    [HideInInspector] public CorpseData corpseData;
    [HideInInspector] public Quest thisQuest;

    [SerializeField] private float radius = 10f;
    [SerializeField] private LayerMask localisationsLayer;
    [SerializeField] private LayerMask interactableHole;
    [SerializeField] private Sprite[] tombSprite = new Sprite[5];
    
    [SerializeField] private Transform piloteLocation;
    [SerializeField] private Transform coPiloteLocation;
    private PlayerTest[] players = new PlayerTest[2];

    private SpriteRenderer spriteRenderer;
    private Hole holeToBurry;

    private bool isInHole;
    private bool canBurry;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (players[0] != null && players[1] != null)
            transform.LookAt(Camera.main.transform);

        Collider[] hits = Physics.OverlapSphere(transform.position, radius, interactableHole);
        if (hits.Length > 0)
        {
            foreach (Collider hit in hits)
            {
                if (hit.gameObject.TryGetComponent(out Hole hole))
                {
                    canBurry = true;
                    holeToBurry = hole;
                }
            }
        }
        else
        {
            canBurry = false;
        }
        //Debug.DrawRay(transform.position, Vector3.down * 20, Color.black);
    }

    public override void Interact(PlayerTest player)
    {
        GameManager.Instance.NewPNJComingWithQuest(thisQuest._request._pnjInteractable);

        // To avoid dotween problem with player increasing scale of this (as a child)
        if(thisQuest.requestInfos.siz > 0)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // DEBUG CARRING W/ OTHER PLAYER
        if ((int)thisQuest.requestInfos.siz > 0)
        {
            // snap player to the corpse
            if(Vector3.Distance(player.transform.position, piloteLocation.position) <= 5)
            {
                if (players[0] == null)
                {
                    player.playerMovement.canMove = false;
                    player.gameObject.transform.DOPause(); //-> worked a bit
                    SetVibrations(player.playerMovement.PlayerInput, 0.1f, 0.1f);

                    if (player.carriedObj == null)
                    {
                        player.carriedObj = this;
                        player.interactableObj = null;
                        player.isCarrying = true;
                    }
                    players[0] = player;
                    player.transform.position = piloteLocation.position;
                    player.playerMovement.ChangeInput("Pilote");
                    player.playerMovement.IsPilote.SetActive(true);

                    if (players[1] != null)
                        player.playerMovement.positionCopilote = players[1].transform;
                }
            }
            else if(Vector3.Distance(player.transform.position, coPiloteLocation.position) <= 5)
            {
                if (players[1] == null)
                {
                    //Debug.Log("Player is co-pilote");
                    player.playerMovement.canMove = false;
                    player.gameObject.transform.DOPause(); //-> worked a bit
                    SetVibrations(player.playerMovement.PlayerInput, 0.1f, 0.1f);

                    if (player.carriedObj == null)
                    {
                        player.carriedObj = this;
                        player.interactableObj = null;
                        player.isCarrying = true;
                    }
                    players[1] = player;
                    //player.transform.DOMove(coPiloteLocation.position, 0.5f, true).SetEase(Ease.Linear);
                    player.transform.position = coPiloteLocation.position;
                    player.playerMovement.ChangeInput("Co-Pilote");
                    player.playerMovement.IsCoPilote.SetActive(true);

                    if (players[0] != null)
                        players[0].playerMovement.positionCopilote = player.transform;
                }
            }

            // If everyone is up to carry the body then they can move
            if ((int)thisQuest.requestInfos.siz + 1 == players.Length)
            {
                int playersSet = 0;
                for(int i = 0; i < players.Length; ++i)
                {
                    if (players[i] != null)
                        playersSet++;
                }
                if(playersSet > 1)
                {
                    players[0].playerMovement.canMove = true;
                    players[1].playerMovement.canMove = true;
                    transform.parent = players[1].transform;
                    players[1].transform.parent = players[0].transform;
                }
            }
        } else if((int)thisQuest.requestInfos.siz <= 0)
        {
            if (player.carriedObj == null)
            {
                player.carriedObj = this;
                player.interactableObj = null;
                player.isCarrying = true;
            }
            player.playerMovement.SpriteRenderer.sprite = player.spriteCarry;
            player.carriedObj.transform.parent = player.transform;
            player.carriedObj.transform.localPosition = Vector3.up * 2f;
        }
    }

    public override void PutDown(PlayerTest player, bool isTimeOut = false)
    {
        // To avoid dotween problem with player increasing scale of this (as a child)
        if (thisQuest.requestInfos.siz > 0)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // DEBUG CARRYING W/ OTHER PLAYER
        player.playerMovement.ChangeInput("Player");

        for(int i = 0; i < players.Length; i++)
        {
            if (players[i] == player)
            {
                if(i == 0)
                {
                    // Pilote is leaving
                    //Debug.Log("Pilote leaves");
                    player.playerMovement.canMove = true;
                    player.playerMovement.IsPilote.SetActive(false);
                    if (players[1] != null)
                    {
                        transform.parent = players[1].transform;
                        players[1].transform.parent = null;
                        players[1].playerMovement.canMove = false;
                    }
                    else
                    {
                        transform.parent = null;
                    }
                    player.playerMovement.positionCopilote = null;
                }
                else
                {
                    // Copilote is leaving
                    //Debug.Log("Co-Pilote leaves");
                    player.playerMovement.IsCoPilote.SetActive(false);
                    player.playerMovement.canMove = true;
                    if (players[0] != null)
                    {
                        players[0].playerMovement.canMove = false;
                        players[0].playerMovement.positionCopilote = null;
                        player.transform.parent = null;
                        transform.parent = players[0].transform;
                    }
                    else
                    {
                        transform.parent = null;
                    }
                }
                players[i] = null;
            }
        }

        if (player.playerMovement.canMove && players[0] == null && players[1] == null) // if one player -> put the body anywhere he wants to
        {
            if(canBurry)
            {
                player.carriedObj.transform.parent = null;

                // update CorpseData
                corpseData = UpdateLocalisation();
                isInHole = true;
                StartCoroutine(BurryingCorpse(holeToBurry));
            }
            else
            {
                //put down corpse in front of a player -> use rotation but now just t.right
                player.carriedObj.gameObject.transform.position = new Vector3(player.transform.position.x + player.playerMovement.orientationVect.x * 3f,
                    player.transform.position.y, player.transform.position.z + player.playerMovement.orientationVect.y * 3f);
                player.carriedObj.transform.parent = null;

                if (isInHole)
                {
                    // update CorpseData
                    corpseData = UpdateLocalisation();
                }
            }
        }
        else
        {
            player.playerMovement.canMove = true;
        }

        player.isCarrying = false;

        // Visual Debug 
        player.playerMovement.SpriteRenderer.sprite = player.playerNotCarrying;

        if (thisQuest != null)
        {
            // check if the corpse correspond to the quest -> finish quest
            if (isInHole)
            {
                corpseData = UpdateRequestLocalisation();
            }
            StartCoroutine(thisQuest.FinishQuest(corpseData));
        }

        player.carriedObj = null;
    }

    private IEnumerator BurryingCorpse(Hole hole)
    {
        Vector3 holepos = hole.transform.position;
        // burry corpse
        hole.Burry();
        gameObject.layer = 0;

        transform.localScale = new Vector3(0f, 0f, 0f);
        transform.position = new Vector3(holepos.x, holepos.y - 3, holepos.z);

        yield return new WaitForSeconds(2f);

        // grave
        int randomsprite = UnityEngine.Random.Range(0, tombSprite.Length);
        spriteRenderer.sprite = tombSprite[randomsprite];

        //Sequence sequence = DOTween.Sequence();

        transform.DOMove(new Vector3(holepos.x, holepos.y, holepos.z), 1f);
        transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
    }

    private CorpseData UpdateLocalisation()
    {
        CorpseData newLoc = new CorpseData();

        newLoc.name = corpseData.name;
        newLoc.size = corpseData.size;
        newLoc.corpseType = corpseData.corpseType;
        newLoc.coffinType = corpseData.coffinType;
        newLoc.specificity = corpseData.specificity;

        Collider[] corpsInAreas = Physics.OverlapSphere(transform.position, radius, localisationsLayer);

        float min = float.MaxValue;

        foreach (Collider col in corpsInAreas)
        {
            float dist = Vector3.Distance(col.gameObject.transform.position, transform.position);
            if (dist < min)
            {
                min = dist;
                newLoc.localisation = AddLocalisation(col.gameObject.tag);
            }
        }
        return newLoc;
    }

    [ContextMenu("Update Localisations")]
    public CorpseData UpdateRequestLocalisation()
    {
        CorpseData newLoc = new CorpseData();

        newLoc.name = thisQuest.requestInfos.corpseName;
        newLoc.size = thisQuest.requestInfos.siz;
        newLoc.corpseType = thisQuest.requestInfos.corps;
        newLoc.coffinType = thisQuest.requestInfos.cof;
        newLoc.specificity = thisQuest.requestInfos.spec;

        Collider[] corpsInAreas = Physics.OverlapSphere(transform.position, radius, localisationsLayer);

        float min = float.MaxValue;

        foreach (Collider col in corpsInAreas)
        {
            float dist = Vector3.Distance(col.gameObject.transform.position, transform.position);
            if (dist < min)
            {
                min = dist;
                newLoc.localisation = AddLocalisation(col.gameObject.tag);
            }
        }
        return newLoc;
    }

    public RequestDataBase.localisation AddLocalisation(string tag)
    {
        switch (tag)
        {
            case "Water": return RequestDataBase.localisation.WATER;
            case "Tree": return RequestDataBase.localisation.TREE;
            case "Shrine": return RequestDataBase.localisation.SHRINE;
            case "Flower": return RequestDataBase.localisation.FLOWER;
            default: return RequestDataBase.localisation.NONE;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
