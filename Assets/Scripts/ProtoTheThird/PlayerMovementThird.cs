using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementThird : MonoBehaviour
{
    [Header("Player Controls")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float digDistance;
    [SerializeField] private float interactionRange;

    [HideInInspector] public Vector3 moveDir;
    [HideInInspector] public Vector3 playerVelocity;
    public bool canMove = true;

    private const float gravityValue = -9.81f;
    private CharacterController controller;
    private Vector2 orientation;
    private Vector2 move;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
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
            orientation = ctx.ReadValue<Vector2>();
            orientation = orientation.normalized;
        }
        else
        {
            move = Vector2.zero;
        }
        //Debug.Log(orientation);
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        Debug.Log("Interacted");
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);

        Vector2 vec = playerPos + orientation.normalized;
        //RaycastHit hit = Physics.Raycast(transform.position, vec, out hit);
        RaycastHit2D hit = Physics2D.Linecast(transform.position, vec);

        if (ctx.started)
        {

        }
        else if (ctx.performed)
        {
            //Mon code 
            Debug.Log("Performer");
        }
        else if (ctx.canceled)
        {
            //chais po
            Debug.Log("Cancel comme mon twitter");
        }

        Debug.DrawLine(playerPos, vec, Color.blue, 2f);
        Debug.DrawRay(playerPos, vec, Color.red, 2f);
    }
}
