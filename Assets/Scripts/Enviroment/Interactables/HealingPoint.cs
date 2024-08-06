using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPoint : Interactable
{
    [SerializeField] float healthToAdd = 10f;

    public override void Interact()
    {
        base.Interact();

        PlayerController.instance.AddHealth(healthToAdd);
    }
}
