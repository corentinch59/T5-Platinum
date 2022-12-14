using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _animatorFlipped;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private SpriteRenderer _rendererFlipped;

    public IEnumerator StartSmokeAnime()
    {
        _animator.SetBool("RestartAnim", true);
        _animatorFlipped.SetBool("RestartAnim", true);
        yield return new WaitForSeconds(1f);
        _animator.SetBool("RestartAnim", false);
        _animatorFlipped.SetBool("RestartAnim", false);
    }

    public void StopAnim()
    {
        _animator.SetBool("RestartAnim", false);
        _animatorFlipped.SetBool("RestartAnim", false);
    }
}
