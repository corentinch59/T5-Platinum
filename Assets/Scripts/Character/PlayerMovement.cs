using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("PlayerControls")]
    [SerializeField] private float playerSpeed;

    private CharacterController controller;

    private const float gravityValue = -9.81f;

    [HideInInspector] public Vector3 moveDir;
    [HideInInspector] public Vector3 playerVelocity;

    public Vector2 orientationVect;
    private Vector2 move;

    public bool canMove = true;

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
            if(ctx.ReadValue<Vector2>() != Vector2.zero && controller.minMoveDistance <= ctx.ReadValue<Vector2>().magnitude * playerSpeed)
            {
                orientationVect = ctx.ReadValue<Vector2>();

                if (Mathf.Abs(orientationVect.x) > Mathf.Abs(orientationVect.y))
                {
                    orientationVect = new Vector2(orientationVect.x + orientationVect.y, 0);
                }
                else
                {
                    orientationVect = new Vector2(0, orientationVect.y + orientationVect.x);
                }
                orientationVect.Normalize();
                //Debug.Log("Orientation : " + orientationVect);
            }
        }
        else
        {
            move = Vector2.zero;
        }
    }
}
