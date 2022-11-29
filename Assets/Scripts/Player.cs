using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float distanceSpawnHole = 2f;

    public LayerMask graveLayer;
    public float distGraveCreation;

    private PlayerMovement playerMovement;
    public PlayerMovement getPlayerMovement => playerMovement;

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
    public float radiusSphere = 5f;
    public LayerMask interactableLayer;

    [Header("Hole Section")]
    [SerializeField] [Tooltip("The distance at which a hole is detected.")] private float raycastRadius;
    public GameObject holePrefab;
    [SerializeField] private int numberOfTaps;
    public int getNumbersOfTaps => numberOfTaps;

    private DiggingBehavior diggingBehavior;
    private PlayerVFX vfx;
    #region ITERATION_1
    private GameObject buttonMashImage;
    public GameObject getButtonMashImage => buttonMashImage;
    #endregion
    #region ITERATION_2
    private Slider sliderMashButton;
    public Slider getSlider => sliderMashButton;
    #endregion
    #region ITERATION_3
    private RectTransform mainRect;
    public RectTransform getMainRect => mainRect;
    private RectTransform iteration3rect;
    public RectTransform getIteration3Rect => iteration3rect;
    #endregion

    private void Start() 
    {
        playerMovement = GetComponent<PlayerMovement>();
        raycastBehavior = new RaycastEmptyHand();
        vfx = GetComponent<PlayerVFX>();
        TransitionDigging(new StartDigging());
        #region ITERATION_1
        //buttonMashImage = transform.GetChild(transform.childCount - 1).gameObject;
        //buttonMashImage.SetActive(false);
        #endregion
        #region ITERATION_2
        sliderMashButton = transform.GetChild(transform.childCount - 2).GetChild(0).GetComponent<Slider>();
        #endregion
        #region ITERATION_3
        mainRect = transform.GetChild(transform.childCount - 2).GetChild(1).GetComponent<RectTransform>();
        iteration3rect = transform.GetChild(transform.childCount - 2).GetChild(1).GetChild(2).GetComponent<RectTransform>();
        #endregion
    }

    private void Update()
    {
        //Debug player vision
        Debug.DrawLine(transform.position, transform.position + transform.forward * distGraveCreation, Color.red);

        objectFound = raycastBehavior.PerformRaycast(transform.position, raycastRadius, interactableLayer);
        //Test Outline
        if (objectFound != null && objectFound != lastObjectFound)
        {
            if(objectFound.GetComponent<SpriteRenderer>() != null)
            {
                CallOutline(true, objectFound.GetComponent<SpriteRenderer>());
                if (lastObjectFound != null)
                {
                    CallOutline(false,lastObjectFound.GetComponent<SpriteRenderer>());
                }
                lastObjectFound = objectFound;
            }
        }
        else if (objectFound == null && lastObjectFound != null)
        {
             CallOutline(false,lastObjectFound.GetComponent<SpriteRenderer>());
             lastObjectFound = null;
        }
 
    }

    public void InteractInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if(objectFound != null)
            {
                Debug.Log("Object found : " + objectFound.name);
                if (objectFound.TryGetComponent(out Hole hole))
                {
                    hole.Interact(this);
                }
                else if (objectFound.TryGetComponent(out GriefPNJInteractable griefPnj) && carriedObj == null)
                {
                    griefPnj.Interact(this);
                }
                else if (objectFound.TryGetComponent(out Corpse corpse))
                {
                    if (carriedObj == null && corpse.IsInteractable)
                    {
                        corpse.Interact(this);
                    }
                    else if (carriedObj != null && carriedObj.TryGetComponent(out GriefPNJInteractable pnj))
                    {
                        pnj.CheckLocationWanted(this);
                    }
                }
                else if (objectFound.TryGetComponent(out BigCorpse bigcorpse) && carriedObj == null)
                {
                    bigcorpse.Interact(this);
                }
            }
            else if (objectFound == null && carriedObj == null)
            {
                diggingBehavior.PerformAction();
            }
        }
    }

    public void DashCancelInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if(carriedObj != null)
            {
                if (carriedObj.TryGetComponent(out Corpse corpse))
                {
                    corpse.PutDown(this);
                }
                else if (carriedObj.TryGetComponent(out GriefPNJInteractable griefPnj))
                {
                    griefPnj.PutDown(this);
                }
            }
            else if (objectFound != null && objectFound.TryGetComponent(out Hole hole) && carriedObj == null)
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
        else if(objectFound == null)
        {
            // Instantiate Hole to where the player is looking
            Instantiate(holePrefab, new Vector3(transform.position.x + getPlayerMovement.getOrientation.x * distanceSpawnHole, transform.position.y - 1f,
                transform.position.z + getPlayerMovement.getOrientation.y * distanceSpawnHole), holePrefab.transform.rotation);
        }
    }

    private void CallOutline(bool active, SpriteRenderer renderer)
    {
        StartCoroutine(vfx.Outline(active, renderer));
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, radiusSphere);
    }
}