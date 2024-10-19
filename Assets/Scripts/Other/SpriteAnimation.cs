using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class SpriteAnimation : MonoBehaviour
{
    public float animationDuration = 1;
    public float speed = 1;

    [HideInInspector] public bool nextIteration = true;
    [HideInInspector] public int firstFrame = 0;

    [SerializeField] Sprite[] sprites;
    [SerializeField] bool randomFirstFrame = true;

    SpriteRenderer _spriteRenderer;

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
            yield return new WaitForSeconds((animationDuration / sprites.Length) / speed);
        }

        nextIteration = true;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        nextIteration = true;
    }
}
