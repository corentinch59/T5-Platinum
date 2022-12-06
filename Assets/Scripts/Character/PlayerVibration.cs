using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVibration : MonoBehaviour
{
    private Player player;
    private PlayerInput playerInput;
    private Gamepad playerPad;
    private Coroutine activeCoroutine;

    private void Start()
    {
        player = gameObject.GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        playerPad = playerInput.GetDevice<Gamepad>();

        player.onVibration += DoVibration;
    }

    private void DoVibration(Vector2 value, float duration)
    {
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput is null");
            return;
        }

        if (playerPad == null)
        {
            Debug.LogError("PlayerInput is not a Gamepad");
            return;
        }

        if(activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        activeCoroutine = StartCoroutine(VibrateGamePad(value, duration));
    }

    private IEnumerator VibrateGamePad(Vector2 value, float duration)
    {
        playerPad.ResetHaptics();
        playerPad.ResumeHaptics();
        playerPad.SetMotorSpeeds(value.x, value.y);
        yield return new WaitForSecondsRealtime(duration);
        playerPad.PauseHaptics();
        playerPad.ResetHaptics();
    }
}
