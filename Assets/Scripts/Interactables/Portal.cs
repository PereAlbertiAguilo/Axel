using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Interactable
{
    [SerializeField] string nextFloor;

    public override void Interact()
    {
        base.Interact();

        GameManager.instance.ClearDataForNextFloor();

        MenusManager.instance.ChangeScene(nextFloor);

        if (hasUses) uses--;
    }
}
