using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float forceDash;
    [SerializeField] private float dashTime;

    private Rigidbody _rigidbody;
    private CharacterController _controller;
    private PlayerMovement _playerMouvement;

    private Coroutine currentCoroutine = null;

    private bool isDashing = false;
    private bool DidIHitSomething = false;

    private CharacterController collisionCharacter;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _controller = GetComponent<CharacterController>();
        _playerMouvement = GetComponent<PlayerMovement>();
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && (_playerMouvement.getMove.normalized.x != 0 || _playerMouvement.getMove.normalized.y != 0))
        {
            if (currentCoroutine != null) return;
            currentCoroutine = StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        _controller.enabled = false;
        isDashing = true;
        _playerMouvement.isDashing = isDashing;
        //StopCoroutine(_playerMouvement.FeedBackPlayerMoves());


        _rigidbody.velocity += new Vector3(_playerMouvement.getMove.normalized.x * forceDash, 0f,_playerMouvement.getMove.normalized.y * forceDash);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(new Vector3(1.5f, 0.5f), 0.35f))
            .Append(transform.DOScale(new Vector3(1f,1f), 0.375f));

        yield return new WaitForSeconds(dashTime);

        _controller.enabled = true;
        isDashing = false;
        _playerMouvement.isDashing = isDashing;

        if (DidIHitSomething && collisionCharacter != null)
        {
            collisionCharacter.enabled = true;
        }

        DidIHitSomething = false;
        currentCoroutine = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDashing)
        {
            DidIHitSomething = true;
            //Debug.Log("Stuned");
          
            if (collision.gameObject.GetComponent<CharacterController>())
            {
                collisionCharacter = collision.gameObject.GetComponent<CharacterController>();
                collisionCharacter.enabled = false;


                _rigidbody.velocity = new Vector3(-_playerMouvement.getMove.normalized.x * (forceDash / 2), 0f, 
                    -_playerMouvement.getMove.normalized.y * (forceDash / 2));


                collision.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(_playerMouvement.getMove.normalized.x * (forceDash / 2), 0f,
                    _playerMouvement.getMove.normalized.y * (forceDash / 2));
            }
            //fait l'effet hector
        }
    }
}
