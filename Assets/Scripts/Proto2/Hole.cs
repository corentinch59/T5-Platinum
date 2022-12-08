using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Hole : MonoBehaviour, IInteractable
{
    [SerializeField] private float bubbleHeight = 2.75f;

    private int HoleSize = 1;
    private bool imageShown = false;
    private Vector3 originalSize;
    private Tween tween;
    private Image bubbleIMage;
    private Corpse heldCorpse;
    private Sprite originalSprite;
      private BoxCollider colliderHole;
    private RectTransform bubbleParent;
    private SpriteRenderer spriteRenderer;

    public Corpse HeldCorpse => heldCorpse;

    public int SetHoleSize { 
        get => HoleSize;

        set => ModifyHoleSize(value); 
    }

    [Header("Hole stuff")]
    [Tooltip("The ratio that will determine how much the hole grows in size when digging more")] public float scaleAmountToAdd = 0.2f;
    [Tooltip("How quick the hole grows in size when digging more.")] public float scaleAnimDuration = 1;
    [Tooltip("How quick the hole reseals itself before showing a tomb.")] public float scaleAnimTombDuration = 0.5f;
    [Tooltip("How quick the tomb will be show.")] public float tombSpawnDuration = 1;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        colliderHole = GetComponent<BoxCollider>();
        originalSize = colliderHole.size;
        bubbleParent = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
        bubbleIMage = bubbleParent.GetChild(0).GetComponent<Image>();
        bubbleParent.gameObject.SetActive(false);
    }

    private void ModifyHoleSize(int modifier)
    {
        if(modifier > 0 && HoleSize < 3)
        {
            HoleSize += modifier;
            if(tween == null)
            {
                tween = transform.DOScale(transform.localScale + new Vector3(scaleAmountToAdd, scaleAmountToAdd, 0) * modifier, scaleAnimDuration).SetEase(Ease.InBounce);
            }
            else
            {
                if(tween.IsComplete())
                    tween = null;
            }
            
        }
        else if (modifier < 0)
        {
            StartCoroutine(BurryAnim(scaleAnimDuration));
        }
    }

    private IEnumerator BurryAnim(float duration)
    {
        Tween animation = transform.DOScale(0, duration).SetEase(Ease.InBounce);
        yield return new WaitForSeconds(animation.Duration());
        if(heldCorpse == null)
            Destroy(gameObject);
    }

    public void Interact(Player player)
    {

        if (player.CarriedObj != null)
        {
            if(heldCorpse == null)
            {
                if (player.CarriedObj.TryGetComponent(out Corpse corpse))
                {
                    #region stop drag sounds
                    SoundManager.instance.Stop("DragMud");
                    SoundManager.instance.Stop("DragStone");
                    SoundManager.instance.Stop("DragDirt");
                    #endregion
                    if (player.CarriedObj.TryGetComponent(out BigCorpse bc))
                    {
                        if(SetHoleSize > 1)
                        {
                            // Stop tiredVfx
                            if (bc.Players[0].TiredVFX != null)
                            {
                                bc.Players[0].TiredVFX.Stop();
                            }

                            player.CarriedObj.gameObject.layer = 7; // <- carriedObj is interactable
                            // Detach both players
                            bc.Players[0].CarriedObj = null;
                            if(bc.Players[1] != null)
                            {
                                // Stop tiredVfx
                                if (bc.Players[1].TiredVFX != null)
                                {
                                    bc.Players[1].TiredVFX.Stop();
                                }
                                bc.Players[1].CarriedObj = null;
                                bc.Interact(bc.Players[1]);
                            }
                            bc.Interact(bc.Players[0]);
                            StartCoroutine(BurryingCorpse(corpse));
                            heldCorpse = corpse;
                            bubbleIMage.sprite = heldCorpse.SpriteRenderer.sprite;
                            ShowBubble();
                            corpse.gameObject.SetActive(false);
                            HoleSize = 1;
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        corpse.IsInteractable = false;
                        player.CarriedObj.transform.parent = null;
                        player.getPlayerMovement.SpriteRenderer.sprite = player.playerNotCarrying;
                    }
                    corpse.OutlineImg.SetActive(false); // <- deactivate exclamation point
                    player.CarriedObj.gameObject.layer = 7; // <- carriedObj is interactable
                    StartCoroutine(BurryingCorpse(corpse));
                    heldCorpse = corpse;
                    bubbleIMage.sprite = heldCorpse.SpriteRenderer.sprite;
                    ShowBubble();
                    player.CarriedObj = null;
                    corpse.gameObject.transform.position = transform.position;
                    corpse.gameObject.SetActive(false);
                }
            }
            else
            {
                if (player.CarriedObj.TryGetComponent(out GriefPNJInteractable griefPNJ))
                {
                    griefPNJ.CheckLocationWanted(player);
                }
            }
        }
        else
        {
            if (heldCorpse == null)
            {
                // Grow hole size
                SetHoleSize = 1;
            }
            else
            {
                // Dig Up Corpse
                player.DiggingBehavior.PerformAction();
                int randomint = UnityEngine.Random.Range(1, 4);
                SoundManager.instance.Play("Dig" + randomint);
                if (player.DiggingBehavior is StartDigging)
                {
                    //player.DiggingBehavior.OnDigCompleted

                    Vector3 posCorpse = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);
                    heldCorpse.gameObject.layer = 7; // <- is Interactable
                    heldCorpse.IsInteractable = true;
                    heldCorpse.transform.position = posCorpse;
                    heldCorpse.gameObject.SetActive(true);
                    heldCorpse.tag = "Corpse";

                    // reset the size to avoid dotween animation problem
                    if (heldCorpse.CorpseData.size > 0)
                    {
                        heldCorpse.transform.localScale = new Vector3(2, 2, 2);
                    }
                    else
                    {
                        heldCorpse.transform.localScale = new Vector3(1, 1, 1);
                    }

                    //player.CarriedObj = heldCorpse;
                    // if we dug up a big or a little body
                    if (heldCorpse.CorpseData.size > 0)
                    {
                        HoleSize = 1;
                        //heldCorpse.GetComponent<BigCorpse>().Interact(player);
                    }
                    else
                    {
                        //heldCorpse.Interact(player);
                    }
                    StartCoroutine(BurryingCorpse(heldCorpse));
                    Hidebubble();
                    heldCorpse = null;
                }
            }
        }
    }

    public IEnumerator BurryingCorpse(Corpse corpse)
    {
        
        Vector3 holepos = transform.position;
        // burry corpse
        if(heldCorpse == null)
            Burry();

        // destroy corpse and change sprite Hole
        // Cant' interact with the corpse anymore
        //corpse.gameObject.layer = 0;

        if (corpse.ThisQuest != null)
        {
            corpse.CorpseData = corpse.UpdateRequestLocalisation();
            StartCoroutine(corpse.ThisQuest.FinishQuest(corpse.CorpseData));
        }
        else
        {
            if(heldCorpse == null)
            {
                int randomInt1 = Random.Range(1, 5);
                SoundManager.instance.Play("TombSpawn" + randomInt1);

                transform.localScale = new Vector3(0f, 0f, 0f);
                transform.position = new Vector3(holepos.x, holepos.y, holepos.z);

                yield return new WaitForSeconds(tombSpawnDuration);

                // Test tombsprite -> HoleScript
                int randomSprite = UnityEngine.Random.Range(0, corpse.TombSprite.Length);
                spriteRenderer.sprite = corpse.TombSprite[randomSprite];

                if (randomSprite == 0)
                {
                    transform.DOMove(new Vector3(holepos.x, holepos.y + 1f, holepos.z), 0.5f);
                }
                else if (randomSprite == 1)
                {
                    transform.DOMove(new Vector3(holepos.x, holepos.y + 1f, holepos.z), 0.5f);
                    colliderHole.size = new Vector3(colliderHole.size.x, colliderHole.size.y - 1f, colliderHole.size.z);
                }
                transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
            }
            else
            {
                //Dig up
                colliderHole.size = originalSize;
                spriteRenderer.sprite = originalSprite;
                gameObject.layer = 0;
                yield return new WaitForSeconds(2f);
                gameObject.layer = 7;
                SetHoleSize = 1;
            }
            yield break;
        }

        transform.localScale = new Vector3(0f, 0f, 0f);
        transform.position = new Vector3(holepos.x, holepos.y, holepos.z);

        int randomInt2 = Random.Range(1, 5);
        SoundManager.instance.Play("TombSpawn" + randomInt2);

        yield return new WaitForSeconds(tombSpawnDuration);

        // Test tombsprite -> HoleScript
        int randomsprite = UnityEngine.Random.Range(0, corpse.TombSprite.Length);
        spriteRenderer.sprite = corpse.TombSprite[randomsprite];

        if (randomsprite == 0)
        {
            transform.DOMove(new Vector3(holepos.x, holepos.y + 1f, holepos.z), 0.5f);
        } else if(randomsprite == 1)
        {
            transform.DOMove(new Vector3(holepos.x, holepos.y + 1, holepos.z), 0.5f);
            colliderHole.size = new Vector3(colliderHole.size.x, colliderHole.size.y - 1f, colliderHole.size.z);
        }
        transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
    }

    public void Burry()
    {
        StartCoroutine(BurryAnim(scaleAnimTombDuration));
    }

    public void ShowBubble()
    {
        if (imageShown || heldCorpse == null)
            return;

        bubbleParent.gameObject.SetActive(true);
        bubbleParent.transform.localScale = Vector3.zero;
        bubbleParent.transform.localPosition = Vector3.zero;
        bubbleParent.transform.DOScale( new Vector3( 1f,1f,1f), 0.5f);
        bubbleParent.transform.DOLocalMove(new Vector3 (0f, bubbleHeight, 0f), 0.5f);
        imageShown = true;
    }

    public void Hidebubble()
    {
        if (!imageShown)
            return;

        bubbleParent.transform.DOLocalMove(Vector3.zero, 0.5f);
        bubbleParent.transform.DOScale(Vector3.zero, 0.5f);
        imageShown = false;
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
