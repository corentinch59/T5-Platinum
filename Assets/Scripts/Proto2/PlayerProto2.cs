using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Jobs;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class PlayerProto2 : MonoBehaviour
{
    [Header("Player Controls")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float interactionDuration;
    [SerializeField] private float raycastRadius;
    [SerializeField] private LayerMask raycastMask;

    [Header("Prefabs to spawn")]
    public GameObject holePrefab;

    [HideInInspector] public Vector3 moveDir;
    [HideInInspector] public Vector3 playerVelocity;
    public bool canMove = true;

    private const float gravityValue = -9.81f;
    private CharacterController controller;
    private Vector2 orientationVect;
    private Vector2 move;
    private Transform canvaQte;
    private Image qteFillImage;
    private IEnumerator myCoroutine;
    private Hole detectedHole;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        canvaQte = gameObject.transform.GetChild(0);
        qteFillImage = canvaQte.GetComponentsInChildren<Image>()[1];
        canvaQte.gameObject.SetActive(false);
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, new Vector3(transform.position.x + orientationVect.x, transform.position.y, transform.position.z + orientationVect.y));
        RaycastHit[] colliders = Physics.SphereCastAll(transform.position, raycastRadius, transform.forward, Mathf.Infinity, raycastMask);
        if(colliders.Length > 0)
        {
            detectedHole = colliders[0].collider.GetComponent<Hole>();
            for (int i = 0; i < colliders.Length; i++)
            {
                float distanceCurrent = (colliders[i].transform.position - transform.position).magnitude;
                float distancePrevious = (detectedHole.transform.position - transform.position).magnitude;

                if(distanceCurrent > distancePrevious)
                {
                    detectedHole = colliders[i].collider.GetComponent<Hole>();
                }
            }
        }
        else
        {
            detectedHole = null;
        }
    }

    private void FixedUpdate()
    {
        moveDir = new Vector3(move.x, 0, move.y);
        controller.Move(moveDir * playerSpeed * Time.fixedDeltaTime);

        if (controller.isGrounded && playerVelocity.y < 0)
            playerVelocity.y = 0f;

        playerVelocity.y += controller.isGrounded ? 0f : gravityValue * Time.fixedDeltaTime;
        controller.Move(playerVelocity * Time.fixedDeltaTime);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (canMove)
        {
            move = ctx.ReadValue<Vector2>();

            if(ctx.ReadValue<Vector2>().sqrMagnitude > (controller.minMoveDistance * controller.minMoveDistance))
            {
                orientationVect = ctx.ReadValue<Vector2>();
                if(Mathf.Abs(orientationVect.x) > Mathf.Abs(orientationVect.y))
                {
                    orientationVect = new Vector2(orientationVect.x + orientationVect.y, 0);
                }
                else
                {
                    orientationVect = new Vector2(0, orientationVect.y + orientationVect.x);
                }
                orientationVect.Normalize();
            }
        }
        else
        {
            move = Vector2.zero;
        }
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        // Start of QTE code
        if (ctx.started)
        {
            Debug.Log("Started");
            myCoroutine = StartTimer();
            StartCoroutine(myCoroutine);
        }
        if (ctx.canceled)
        {
            Debug.Log("Canceled");
            StopCoroutine(myCoroutine);
            canvaQte.gameObject.SetActive(false);
        }
    }

    private IEnumerator StartTimer()
    {
        canvaQte.gameObject.SetActive(true);
        qteFillImage.fillAmount = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < interactionDuration)
        {
            qteFillImage.fillAmount = Mathf.Lerp(0f, 1f, (elapsedTime / interactionDuration));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        Dig(); // Diggy diggy hole
        canvaQte.gameObject.SetActive(false);
        yield return null;
    }

    //End of QTE code

    private void Dig()
    {
        Debug.Log(detectedHole);
        if (detectedHole)
        {
            detectedHole.SetHoleSize = 1;
        }
        else
        {
            Instantiate(holePrefab, transform.position + new Vector3(orientationVect.x, 0, orientationVect.y), Quaternion.identity);
        }

    }
}
