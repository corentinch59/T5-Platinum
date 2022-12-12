using Cinemachine.Utility;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;
using UnityEngine.VFX;

public delegate void VibrationDelegate(Vector2 value, float duration);

public class Player : MonoBehaviour
{
    [SerializeField] private Vector2 holeVibrationStrength;
    [SerializeField] private float holeVibrationDuration;
    [SerializeField] private VisualEffect tiredVFX; // when dragging big body

    public LayerMask locationLayers;
    private PlayerMovement playerMovement;
    private PlayerVibration playerVibration;

    #region DELEGATES
    public event VibrationDelegate onVibration;
    #endregion
    #region GETTERS_AND_SETTERS
    public int getNumbersOfTaps => numberOfTaps;
    public DiggingBehavior DiggingBehavior => diggingBehavior;
    public PlayerMovement getPlayerMovement => playerMovement;
    public PlayerVibration getPlayerVibration => playerVibration;
    public PlayerVFX getVFX => vfx;
    public VisualEffect TiredVFX => tiredVFX;
    public GameObject getCrack { get { if (lastCrack != null) return lastCrack; return null; } }
    public GameObject getObjectFound => objectFound;
    #endregion
    #region CARRY_AND_RAYCAST_VALUES
    public Sprite playerNotCarrying;
    public Sprite spriteCarry;
    private Carryable carriedObj;
    public Carryable CarriedObj
    {
        get { return carriedObj; }
        set { carriedObj = value; }
    }

    private IRaycastBehavior raycastBehavior;
    [SerializeField] private GameObject objectFound;
    [SerializeField] private GameObject lastObjectFound;
    [Header("Debug")]
    public LayerMask interactableLayer;
    #endregion
    #region HOLE
    [Header("Hole Section")]
    [SerializeField] [Tooltip("The distance at which a hole is detected.")] private float raycastRadius;
    public GameObject holePrefab;
    [SerializeField] private int numberOfTaps;

    private DiggingBehavior diggingBehavior;
    private PlayerVFX vfx;
    public GameObject crackToInstantiate;
    private GameObject lastCrack;
    private Tween crackTweenScale;
    private Tween crackTweenShake;
    private Tween crackTweenSpawn;
    private DG.Tweening.Sequence crackSequence;
    #region ITERATION_3
    private RectTransform mainRect;
    public RectTransform getMainRect => mainRect;
    private RectTransform iteration3rect;
    public RectTransform getIteration3Rect => iteration3rect;
    #endregion
    #endregion

    [HideInInspector]
    public int id;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerVibration = GetComponent<PlayerVibration>();
        raycastBehavior = new RaycastEmptyHand();
        vfx = GetComponent<PlayerVFX>();
        TransitionDigging(new StartDigging());
        #region ITERATION_3
        mainRect = transform.GetChild(transform.childCount - 1).GetChild(1).GetComponent<RectTransform>();
        iteration3rect = transform.GetChild(transform.childCount - 1).GetChild(1).GetChild(1).GetComponent<RectTransform>();
        #endregion
    }

    private void Update()
    {
        if (carriedObj != null)
        {
            if (carriedObj.TryGetComponent(out Corpse c))
            {
                objectFound = raycastBehavior.PerformRaycast(transform.position, raycastRadius, interactableLayer, new string[] { "Hole", "DigUpPNJ" });

                if(c.CorpseData.size > 0)
                {
                    // Drag sound
                    GameObject area = raycastBehavior.PerformRaycast(c.transform.position, raycastRadius, locationLayers);
                    if (area != null && playerMovement.getMove.sqrMagnitude > 0)
                    {
                        if (area.tag == "Water")
                        {
                            if (!SoundManager.instance.GetSound("DragMud").source.isPlaying)
                            {
                                SoundManager.instance.Play("DragMud");
                            }
                        }
                        else if (area.tag == "Shrine")
                        {
                            if (!SoundManager.instance.GetSound("DragStone").source.isPlaying)
                            {
                                SoundManager.instance.Play("DragStone");
                            }
                        }
                        else
                        {
                            if (!SoundManager.instance.GetSound("DragDirt").source.isPlaying)
                            {
                                SoundManager.instance.Play("DragDirt");
                            }
                        }
                    }
                    else
                    {
                        SoundManager.instance.Stop("DragMud");
                        SoundManager.instance.Stop("DragStone");
                        SoundManager.instance.Stop("DragDirt");
                    }

                    if (objectFound != null && objectFound.TryGetComponent(out Hole h) && h.SetHoleSize <= 1)
                    {
                        return;
                    }
                }
            }
            if (carriedObj.TryGetComponent(out GriefPNJInteractable griefPnj))
            {
                objectFound = raycastBehavior.PerformRaycast(transform.position, raycastRadius, interactableLayer, new string[] { "Hole" }, new string[] { "DigUpPNJ" });
            }
        }
        else
        {
            objectFound = raycastBehavior.PerformRaycast(transform.position, raycastRadius, interactableLayer, null, new string[] { "DigUpPNJ" });
        }

        // Outline & Bubble
        if (objectFound != null && objectFound != lastObjectFound)
        {
            if (objectFound.GetComponent<SpriteRenderer>() != null)
            {
                Outline(objectFound, true, id);
                Outline(lastObjectFound, false, id);
                if (objectFound.TryGetComponent(out Hole a))
                    a.ShowBubble();
                if (lastObjectFound != null && lastObjectFound.TryGetComponent(out Hole b))
                    b.Hidebubble();

                lastObjectFound = objectFound;
            }
        }
        else if (objectFound == null && lastObjectFound != null)
        {
            Outline(lastObjectFound, false, id);
            if(lastObjectFound.TryGetComponent(out Hole a))
                a.Hidebubble();
            lastObjectFound = null;
        }
    }

    private void Outline(GameObject obj, bool active, int playerID)
    {
        if (obj != null)
        {
            CallOutline(active, obj.GetComponent<SpriteRenderer>(), id);
        }
    }

    public void InteractInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (objectFound != null)
            {
                //Debug.Log("Object found : " + objectFound.name);
                if (objectFound.TryGetComponent(out Hole hole))
                {
                    hole.Interact(this);
                }
                else if (objectFound.TryGetComponent(out GriefPNJInteractable griefPnj) && carriedObj == null)
                {
                    objectFound.layer = 0; // <- not interactable for now
                    griefPnj.Interact(this);

                    int randomint = UnityEngine.Random.Range(1, 3);
                    SoundManager.instance.Play("Pickup_Npc" + randomint);
                }
                else if (objectFound.TryGetComponent(out Corpse corpse) && corpse.BigCorpse == null)
                {
                    objectFound.layer = 0; // <- not interactable for now
                    if (carriedObj == null && corpse.IsInteractable)
                    {
                        corpse.Interact(this);
                        int randomint = UnityEngine.Random.Range(1, 3);
                        SoundManager.instance.Play("Grab_Corpse" + randomint);
                    }
                }
                else if (objectFound.TryGetComponent(out BigCorpse bigcorpse) && carriedObj == null)
                {
                    // tired drops from player's head
                    if (tiredVFX != null)
                    {
                        tiredVFX.Play();
                    }
                    bigcorpse.Interact(this);
                    carriedObj = bigcorpse.gameObject.GetComponent<Corpse>();
                    carriedObj.Interact(this);
                }
                else if (objectFound.TryGetComponent(out DigUpPNJInteractable digUpPnj) && carriedObj != null)
                {
                    digUpPnj.Interact(this);
                }
            }
            else if (objectFound == null && carriedObj == null)
            {
                diggingBehavior.PerformAction();
                int randomint = UnityEngine.Random.Range(1, 4);
                SoundManager.instance.Play("Dig" + randomint);
            }
        }
    }

    public void DashCancelInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (carriedObj != null)
            {
                #region stop drag sounds
                SoundManager.instance.Stop("DragMud");
                SoundManager.instance.Stop("DragStone");
                SoundManager.instance.Stop("DragDirt");
                #endregion

                carriedObj.gameObject.layer = 7; // <- Interactable layer 
                if (carriedObj.TryGetComponent(out Corpse corpse))
                {
                    if (carriedObj.TryGetComponent(out BigCorpse bc))
                    {
                        // tired drops from player's head
                        if (tiredVFX != null)
                        {
                            tiredVFX.Stop();
                        }
                        carriedObj = null;
                        if (corpse.ThisQuest != null)
                            corpse.ThisQuest.DesactivateOulineUI();
                        bc.Interact(this);

                    }
                    else
                    {
                        carriedObj.gameObject.layer = 7; // <- Interactable layer 
                        int randomint = UnityEngine.Random.Range(1, 3);
                        SoundManager.instance.Play("Drop_Corpse" + randomint);
                        corpse.PutDown(this);
                    }
                }
                else if (carriedObj.TryGetComponent(out GriefPNJInteractable griefPnj))
                {
                    carriedObj.gameObject.layer = 7; // <- Interactable layer 
                    griefPnj.PutDown(this);
                }
            }
            else if (objectFound != null && objectFound.TryGetComponent(out Hole hole) && hole.HeldCorpse == null && carriedObj == null)
            {
                Dig(-1);
            }
            else
            {
                diggingBehavior.CancelAction();
                // player dash
            }
        }
    }

    public void Dig(int modifier)
    {
        if (objectFound != null && objectFound.TryGetComponent(out Hole hole))
        {
            hole.SetHoleSize = modifier;
        }
        else if (objectFound == null)
        {
            // Instantiate Hole to where the player is looking
            /*Instantiate(holePrefab, new Vector3(transform.position.x + getPlayerMovement.getOrientation.x * distanceSpawnHole, transform.position.y - 1f,
                transform.position.z + getPlayerMovement.getOrientation.y * distanceSpawnHole), holePrefab.transform.rotation);*/
            Instantiate(holePrefab, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), holePrefab.transform.rotation);
            DestroyCrackHole();

        }
    }

    public void SetCrackHole()
    {
        if(crackToInstantiate != null)
        {
            /*lastCrack = Instantiate(crackToInstantiate, new Vector3(
                transform.position.x + getPlayerMovement.getOrientation.x * distanceSpawnHole,
                transform.position.y - 1f,
                transform.position.z + getPlayerMovement.getOrientation.y * distanceSpawnHole),
            crackToInstantiate.transform.rotation);*/
            lastCrack = Instantiate(crackToInstantiate, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z),
            crackToInstantiate.transform.rotation);
            lastCrack.transform.localScale = Vector3.zero;
            lastCrack.transform.DOScale(new Vector3(0.5f,0.5f,0.5f), 0.5f);
        }
    }

    public void AnimateCrackHole(float tap)
    {
        if (lastCrack == null)
        {
            Debug.Log("no crack to animate");
            return;
        }

        if (crackTweenSpawn != null)
            crackTweenSpawn.Kill();

        if(crackTweenScale != null)
            crackTweenScale.Kill();

        if(crackTweenShake != null)
            crackTweenShake.Kill();

        crackTweenScale = lastCrack.transform.DOScale(new Vector3(1.5f,1.5f,1.5f) * tap / numberOfTaps * 5f, 0.25f);
        crackTweenShake = lastCrack.transform.DOShakePosition(
            duration: 0.25f,
            strength: 0.2f
            );

        crackSequence = DOTween.Sequence();
        crackSequence.Append(crackTweenScale).Join(crackTweenShake);
    }

    public void DestroyCrackHole()
    {
        crackTweenSpawn.Kill();
        crackTweenScale.Kill();
        crackTweenShake.Kill();
        crackSequence.Kill();
        Destroy(lastCrack);
    }

    private void CallOutline(bool active, SpriteRenderer renderer, int playerID)
    {
        StartCoroutine(vfx.Outline(active, renderer, playerID));
    }

    public void TransitionDigging(DiggingBehavior newdiggingBehavior)
    {
        diggingBehavior = newdiggingBehavior;
        diggingBehavior.SetPlayer(this);
    }

    public void EnableInput(string input)
    {
        playerMovement.getPlayerInput.currentActionMap.FindAction(input).Enable();
    }

    public void DisableInput(string input)
    {
        playerMovement.getPlayerInput.currentActionMap.FindAction(input).Disable();
    }

    public void TriggerVibration()
    {
        onVibration?.Invoke(holeVibrationStrength, holeVibrationDuration);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, raycastRadius);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Tree"))// && !lampPlaying)
        {
            //lampPlaying = true;
            StartCoroutine(StopAnim(hit.gameObject));
        }
    }

    private IEnumerator StopAnim(GameObject hit)
    {
        hit.transform.parent.GetChild(1).transform.GetChild(0).GetComponent<Animator>().SetBool("Collision 0", true);
        hit.transform.parent.GetChild(1).transform.GetChild(1).GetComponent<Animator>().SetBool("Collision 0", true);
        yield return new WaitForSeconds(2f);
        hit.transform.parent.GetChild(1).transform.GetChild(0).GetComponent<Animator>().SetBool("Collision 0", false);
        hit.transform.parent.GetChild(1).transform.GetChild(1).GetComponent<Animator>().SetBool("Collision 0", false);
        //lampPlaying = false;
    }
}