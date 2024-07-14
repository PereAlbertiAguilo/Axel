using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPoint : MonoBehaviour
{
    [SerializeField] float healthToAdd = 10f;
    [SerializeField] float waitTime = 1f;

    Animator _animator;

    bool canInteract = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (UserInput.instance.interactInput && canInteract)
        {
            PlayerController.instance.AddHealth(healthToAdd);
        }
    }

    IEnumerator InteractDelay(bool enter)
    {
        yield return new WaitForSeconds(waitTime);

        canInteract = enter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(InteractDelay(true));
            _animator.SetBool("IsInRange", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopAllCoroutines();
            canInteract = false;
            _animator.SetBool("IsInRange", false);
        }
    }
}
