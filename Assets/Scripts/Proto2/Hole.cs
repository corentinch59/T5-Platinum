using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class Hole : MonoBehaviour, IInteractable
{
    private Corpse heldCorpse;
    private Tween tween;
    private int HoleSize = 1;
    public int SetHoleSize { 
        get => HoleSize;

        set => ModifyHoleSize(value); 
    }

    [Header("Hole stuff")]
    [Tooltip("The ratio that will determine how much the hole grows in size when digging more")] public float scaleAmountToAdd = 0.2f;
    [Tooltip("How quick the hole grows in size when digging more.")] public float scaleAnimDuration = 1;
    [Tooltip("How quick the hole reseals itself before showing a tomb.")] public float scaleAnimTombDuration = 0.5f;
    [Tooltip("How quick the tomb will be show.")] public float tombSpawnDuration = 1;

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
        if (player.CarriedObj != null && heldCorpse == null)
        {
            if (player.CarriedObj.TryGetComponent(out Corpse corpse))
            {
                if(player.CarriedObj.TryGetComponent(out BigCorpse bc))
                {
                    if(SetHoleSize > 1)
                    {
                        player.CarriedObj.gameObject.layer = 7; // <- carriedObj is interactable
                        // Detach both players
                        bc.Players[0].CarriedObj = null;
                        if(bc.Players[1] != null)
                        {
                            bc.Players[1].CarriedObj = null;
                            bc.Interact(bc.Players[1]);
                        }
                        bc.Interact(bc.Players[0]);
                        StartCoroutine(BurryingCorpse(corpse));
                        heldCorpse = corpse;
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
                player.CarriedObj.gameObject.layer = 7; // <- carriedObj is interactable
                heldCorpse = corpse;
                StartCoroutine(BurryingCorpse(corpse));
                player.CarriedObj = null;
            }
            else if(player.CarriedObj.TryGetComponent(out GriefPNJInteractable griefPNJ))
            {
                griefPNJ.CheckLocationWanted(player);
            }
        }
        else
        {
            if (heldCorpse == null)
                SetHoleSize = 1;
        }
    }

    public IEnumerator BurryingCorpse(Corpse corpse)
    {
        Vector3 holepos = transform.position;
        // burry corpse
        Burry();

        // Cant' interact with the corpse anymore
        corpse.gameObject.layer = 0;

        if (corpse.ThisQuest != null)
        {
            corpse.CorpseData = corpse.UpdateRequestLocalisation();
            StartCoroutine(corpse.ThisQuest.FinishQuest(corpse.CorpseData));
        }

        corpse.transform.localScale = new Vector3(0f, 0f, 0f);
        corpse.transform.position = new Vector3(holepos.x, holepos.y - 3, holepos.z);

        yield return new WaitForSeconds(tombSpawnDuration);

        // grave
        int randomsprite = UnityEngine.Random.Range(0, corpse.TombSprite.Length);
        corpse.SpriteRenderer.sprite = corpse.TombSprite[randomsprite];

        if(randomsprite == 0)
        {
            corpse.transform.DOMove(new Vector3(holepos.x, holepos.y + 1f, holepos.z), 0.5f);
        } else if(randomsprite == 1)
        {
            corpse.transform.DOMove(new Vector3(holepos.x, holepos.y + 0.6f, holepos.z), 0.5f);
        }
        corpse.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);

        this.enabled = false;
    }

    public void Burry()
    {
        StartCoroutine(BurryAnim(scaleAnimTombDuration));
    }

    public void SetVibrations(PlayerInput playerInput, float frequencyLeftHaptic, float frequencyRightHaptic)
    {
    }

    public IEnumerator SetVibrationsCoroutine(PlayerInput playerInput, float frequencyLeftHaptic, float frequencyRightHaptic)
    {
        yield break;
    }
}
