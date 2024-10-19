using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class AnimatorState
{
    public static IEnumerator ChangeState(Animator animator, string newState)
    {
        if (!string.IsNullOrEmpty(newState) && animator != null)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(newState)) animator.Play("Empty");

            yield return new WaitForEndOfFrame();

            animator.Play(newState);
        }
    }
}
