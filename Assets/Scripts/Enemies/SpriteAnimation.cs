using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] float animationDuration = 1;

    SpriteRenderer _spriteRenderer;

    bool nextIteration = false;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        Invoke(nameof(StartAnimation), Random.value * 2);
    }

    private void Update()
    {
        if (nextIteration)
        {
            nextIteration = false;
            StartCoroutine(Animate());
        }
    }

    IEnumerator Animate()
    {
        foreach (Sprite sprite in sprites)
        {
            _spriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(animationDuration / sprites.Length);
        }

        nextIteration = true;
    }

    void StartAnimation()
    {
        nextIteration = true;
    }
}
