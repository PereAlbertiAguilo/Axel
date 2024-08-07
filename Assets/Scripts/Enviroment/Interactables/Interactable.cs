using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [HideInInspector] public float interactDelay = 1f;
    [HideInInspector] public bool hasUses = false;
    [HideInInspector, Min(1)] public int uses = 1;

    protected Animator _animator;

    protected bool canInteract = false;

    public virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public virtual void Update()
    {
        if (UserInput.instance.interactInput && canInteract && HasInteracted())
        {
            Interact();
        }
    }

    public virtual void Interact() { }

    protected bool HasInteracted()
    {
        if (hasUses)
        {
            if (uses <= 0) return false;
            uses--;
            return true;
        }
        else
        {
            return true;
        }
    }

    IEnumerator InteractDelay()
    {
        yield return new WaitForSeconds(interactDelay);

        canInteract = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && uses >= 1)
        {
            StartCoroutine(InteractDelay());
            _animator.SetBool("IsInRange", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && uses >= 1)
        {
            StopAllCoroutines();
            canInteract = false;
            _animator.SetBool("IsInRange", false);
        }
    }
}
