using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Corpse : Carryable
{
    [SerializeField] private GameObject outlineImg;
    [SerializeField] private CorpseData corpseData;
    [SerializeField] private Quest thisQuest;

    [SerializeField] private float radius = 10f;
    [SerializeField] private LayerMask localisationsLayer;
    [SerializeField] private LayerMask interactableHole;
    [SerializeField] private Sprite[] tombSprite = new Sprite[5];
    [SerializeField] private ParticleSystem bigCorpseTrail;
    [SerializeField] private float durationShakeTween;
    [SerializeField] private float strengthShakeTween;
    [SerializeField] private float bigCorpseSize;
    [SerializeField] private bool isTuto;

    private PNJInteractable pnjFrom;
    private SpriteRenderer spriteRenderer;
    private BigCorpse bigCorpse;
    private Material material;
    private Tween meep;
    private bool isInteractable;
    private bool isAlmostOver;

    #region get/set
    public CorpseData CorpseData { get { return corpseData; } set { corpseData = value; } }
    public Quest ThisQuest { get { return thisQuest; } set { thisQuest = value; } }
    public BigCorpse BigCorpse { get { return bigCorpse; } set { bigCorpse = value; } }
    public GameObject OutlineImg => outlineImg; // <- deactivate outline with the current quest duration
    public Sprite[] TombSprite => tombSprite;
    public SpriteRenderer SpriteRenderer => spriteRenderer;
    public PNJInteractable PnjFrom { get { return pnjFrom; } set { pnjFrom = value; } }
    public bool IsInteractable { get { return isInteractable; } set { isInteractable = value; } }
    public float BigCorpseSize { get { return bigCorpseSize; } set { bigCorpseSize = value; } }
    #endregion

    private void Start()
    {
        //ResetSmoke();
        if (isTuto)
        {
            bigCorpse = GetComponent<BigCorpse>();
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        IsInteractable = true;

        if (corpseData.size == RequestDataBase.size.MEDIUM)
        {
            bigCorpseTrail.gameObject.SetActive(true);
        }

        else bigCorpseTrail.gameObject.SetActive(false);

        
        Material mat = Instantiate(OutlineImg.GetComponent<Image>().material);
        OutlineImg.GetComponent<Image>().material = mat;
        DropSmoke();
    }

    private void Update()
    {
        if (thisQuest != null && thisQuest.timer >= thisQuest.QuestTime/2 && !isAlmostOver)
        {
            OutlineImg.GetComponent<Image>().material.SetFloat("_IsAlmostOver", 1);
            ShakeQuest();
            isAlmostOver = true;
        }

    }

    public override void Interact(Player player)
    {
        ResetSmoke();
        if (thisQuest != null)
        {
            thisQuest.ActivateOulineUI(player.id);
            if(thisQuest.requestInfos.siz <= 0)
            {
                if (player.CarriedObj == null)
                {
                    player.CarriedObj = this;
                }
                transform.parent = player.transform;
                transform.localPosition = Vector3.up * 2f;
                player.getPlayerMovement.SpriteRenderer.sprite = player.spriteCarry;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            if(corpseData.size <= 0)
            {
                if (player.CarriedObj == null)
                {
                    player.CarriedObj = this;
                }
                transform.parent = player.transform;
                transform.localPosition = Vector3.up * 2f;
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(2, 2, 2);
            }
        }
        // Remove tag "Corpse" to avoid the pnj to check if he has to leave cause this is already at its spot
        // And tell another pnj to come give player a quest
        if((gameObject.tag == "Corpse" && QuestManager.instance.allQuests.Count >= 0 && thisQuest != null) || 
            (thisQuest == null && pnjFrom != null))
        {
            gameObject.tag = "Untagged";
            if(thisQuest == null)
            {
                GameManager.Instance.PnjsAlreadyGaveQuest.Remove(pnjFrom);
                pnjFrom = null;
            }
            else
            {
                GameManager.Instance.PnjsAlreadyGaveQuest.Remove(pnjFrom);
                pnjFrom = null;
            }
            if(GameManager.Instance.QuestsSpawning == null)
            {
                GameManager.Instance.QuestsSpawning = GameManager.Instance.NewPNJComingWithQuest();
                GameManager.Instance.StartCoroutine(GameManager.Instance.QuestsSpawning);
            }
        }
    }

    public override void PutDown(Player player, bool isTimeOut = false)
    {
        gameObject.layer = 7; // <-is Interactable
        isInteractable = true;
        DropSmoke();
        if (thisQuest != null)
        {
            thisQuest.DesactivateOulineUI();
        }

        if (player.getPlayerMovement.canMove) // if one player -> put the body anywhere he wants to
        {
            player.CarriedObj.gameObject.transform.position = new Vector3(player.transform.position.x + player.getPlayerMovement.getOrientation.x * 3f,
                player.transform.position.y, player.transform.position.z + player.getPlayerMovement.getOrientation.y * 3f);
            player.CarriedObj.transform.parent = null;
        }
        else
        {
            player.getPlayerMovement.canMove = true;
        }

        player.CarriedObj = null;
        // To avoid dotween problem with player increasing scale of this (as a child)
        if (corpseData.size > 0)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public IEnumerator DeactivateExclamationPointIfTimeOutQuest(float durationQuest)
    {
        yield return new WaitForSeconds(durationQuest);
        outlineImg.SetActive(false);
    }

    public CorpseData UpdateRequestLocalisation(bool locationToo = false)
    {
        CorpseData newLoc = new CorpseData();

        newLoc.name = thisQuest.requestInfos.corpseName;
        newLoc.size = thisQuest.requestInfos.siz;
        newLoc.corpseType = thisQuest.requestInfos.corps;

        if (!locationToo)
        {
            Collider[] corpsInAreas = Physics.OverlapSphere(transform.position, radius, localisationsLayer);

            float min = float.MaxValue;

            for(int i = 0; i < corpsInAreas.Length; ++i)
            {
                float dist = Vector3.Distance(corpsInAreas[i].gameObject.transform.position, transform.position);
                if (dist < min)
                {
                    min = dist;
                    newLoc.localisation = AddLocalisation(corpsInAreas[i].gameObject.tag);
                }
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

    private void DropSmoke()
    {
       StartCoroutine(GetComponent<AnimatorController>().StartSmokeAnime());
    }
    
    private void ResetSmoke()
    {
        GetComponent<AnimatorController>().StopAnim();
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

    private void ShakeQuest()
    {
        meep = thisQuest.transform.DOShakeRotation(
            duration: durationShakeTween,
            strength: strengthShakeTween
            ).SetLoops(-1);
        thisQuest.onQuestDestroy += DestroyTween;
    }

    public void DestroyTween()
    {
        meep.Kill();
    }
}
