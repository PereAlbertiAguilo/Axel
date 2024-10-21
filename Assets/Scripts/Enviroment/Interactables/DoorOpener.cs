using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorOpener : Interactable
{
    [Space]

    public Door door;

    [Space]

    public int planksAmount = 1;

    public TextMeshProUGUI planksAmountText;

    public AudioClip useSound;

    public override void Start()
    {
        base.Start();

        planksAmountText.text = "" + planksAmount + ((planksAmount == 1) ? " PLANK" : " PLANKS");
    }

    public override void Interact()
    {
        base.Interact();

        if(CollectiblesManager.instance.planks >= planksAmount && RoomManager.instance.currentRoom.roomCleared)
        {
            door.isLocked = false;
            Room lockedRoom = RoomManager.instance.currentRoom;
            lockedRoom.OpenDoors();

            CollectiblesManager.instance.SetCollectable(CollectiblesManager.Types.planks, -planksAmount);

            animator.SetBool("Use", true);

            Destroy(gameObject, 1.1f);

            RoomManager.instance.SaveRoomStructures();

            if (useSound != null) Audio.instance.PlayOneShot(useSound, .65f, true);
            RoomManager.instance.currentRoom.BridgeSFX();

            if (hasUses) uses--;
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
