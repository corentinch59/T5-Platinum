using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerProto2 : MonoBehaviour
{
    [Header("Player Controls")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float interactionDuration;

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

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        canvaQte = gameObject.transform.GetChild(0);
        qteFillImage = canvaQte.GetComponentsInChildren<Image>()[1];
        canvaQte.gameObject.SetActive(false);
    }

    private void Update()
    {
        
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
            orientationVect = ctx.ReadValue<Vector2>();

            if(Mathf.Abs(orientationVect.x) > Mathf.Abs(orientationVect.y))
            {
                orientationVect = new Vector2(orientationVect.x, 0);
            }
            else
            {
                orientationVect = new Vector2(0, orientationVect.y);
            }
        }
        else
        {
            move = Vector2.zero;
        }
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
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
        canvaQte.gameObject.SetActive(false);
        yield return null;
    }
}
