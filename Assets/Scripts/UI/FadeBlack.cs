using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBlack : MonoBehaviour
{
    public static FadeBlack instance;

    public Animator _animator;

    private void Awake()
    {
        instance = this;

        _animator = GetComponent<Animator>();
    }

    // Activates a fede TO black aimation
    public void FadeToBlack()
    {
        Time.timeScale = 1;

        _animator.Play("FadeToBlack");
    }

    // Activates a fede FROM black aimation
    public void FadeFromBlack()
    {
        Time.timeScale = 1;

        _animator.Play("FadeFromBlack");
    }

    public void FadeInFadeOut(float delay)
    {
        FadeToBlack();

        Invoke(nameof(FadeFromBlack), delay);
    }
}
