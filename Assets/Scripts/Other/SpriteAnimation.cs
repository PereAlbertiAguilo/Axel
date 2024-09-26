using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] float animationDuration = 1;

    SpriteRenderer _spriteRenderer;

    [HideInInspector] public bool nextIteration = true;

    [SerializeField] bool randomFirstFrame = true;
    public int firstFrame = 0;

    private void Start()
    {
        nextIteration = true;

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        firstFrame = Random.Range(1, sprites.Length);
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
        for (int i = 0; i < sprites.Length; i++)
        {
            int frame = randomFirstFrame ? (((i + firstFrame) > (sprites.Length - 1)) ? (i + firstFrame - sprites.Length) : (i + firstFrame)) : i;

            _spriteRenderer.sprite = sprites[frame];
            yield return new WaitForSeconds(animationDuration / sprites.Length);
        }

        nextIteration = true;
    }
}
