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

        MenusManager.instance.ChangeScene(nextFloor);
    }
}
