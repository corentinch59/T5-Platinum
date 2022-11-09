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
        private get => HoleSize;

        set => ModifyHoleSize(value); 
    }

    [Header("Hole stuff")]
    [Tooltip("The ratio that will determine how much the hole grows in size when digging more")] public float scaleAmountToAdd = 0.2f;
    [Tooltip("How quick the hole grows in size when digging more.")] public float scaleAnimDuration = 1;

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
            StartCoroutine(BurryAnim());
        }
    }

    private IEnumerator BurryAnim()
    {
        transform.DOScale(0, scaleAnimDuration).SetEase(Ease.InBounce);
        yield return new WaitForSeconds(scaleAnimDuration);
        if(heldCorpse == null)
            Destroy(gameObject);
    }

    public void Interact(Player player)
    {
        if (player.CarriedObj != null && heldCorpse == null)
        {
            if(player.CarriedObj.TryGetComponent(out Corpse corpse))
            {
                corpse.IsInteractable = false;
                player.CarriedObj.transform.parent = null;
                player.CarriedObj = null;
                player.getPlayerMovement.SpriteRenderer.sprite = player.playerNotCarrying;

                heldCorpse = corpse;
                StartCoroutine(BurryingCorpse(corpse));
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
        //corpse.gameObject.layer = 0;

        if (corpse.thisQuest != null)
        {
            corpse.corpseData = corpse.UpdateRequestLocalisation();
            StartCoroutine(corpse.thisQuest.FinishQuest(corpse.corpseData));
        }

        corpse.transform.localScale = new Vector3(0f, 0f, 0f);
        corpse.transform.position = new Vector3(holepos.x, holepos.y - 3, holepos.z);

        yield return new WaitForSeconds(2f);

        // grave
        int randomsprite = UnityEngine.Random.Range(0, corpse.TombSprite.Length);
        corpse.SpriteRenderer.sprite = corpse.TombSprite[randomsprite];

        if(randomsprite == 0)
        {
            corpse.transform.DOMove(new Vector3(holepos.x, holepos.y + 1f, holepos.z), 1f);
        } else if(randomsprite == 1)
        {
            corpse.transform.DOMove(new Vector3(holepos.x, holepos.y + 0.6f, holepos.z), 1f);
        }
        corpse.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);

        this.enabled = false;
    }

    public void Burry()
    {
        StartCoroutine(BurryAnim());
    }

    public void SetVibrations(PlayerInput playerInput, float frequencyLeftHaptic, float frequencyRightHaptic)
    {
    }

    public IEnumerator SetVibrationsCoroutine(PlayerInput playerInput, float frequencyLeftHaptic, float frequencyRightHaptic)
    {
        yield break;
    }
}
