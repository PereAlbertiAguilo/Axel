using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZIndex : MonoBehaviour
{
    [SerializeField] bool dynamicChange = true;
    [SerializeField] int offset = 0;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.sortingOrder = -(int)((transform.position.y - offset) * 100);
    }

    private void Update()
    {
        if (dynamicChange) spriteRenderer.sortingOrder = -(int)((transform.position.y - offset) * 100);
    }
}
