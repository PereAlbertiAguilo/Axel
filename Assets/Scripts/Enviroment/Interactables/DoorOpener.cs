using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorOpener : Interactable
{
    [Space]

    public Door door;

    [Space]

    public int planksAmount = 1;

    public TextMeshProUGUI planksAmountText;

    private void Start()
    {
        if(PlayerPrefs.HasKey("DoorOpener" + SceneManager.GetActiveScene().name + door.room.roomIndex))
        {
            planksAmount = PlayerPrefs.GetInt("DoorOpener" + SceneManager.GetActiveScene().name + door.room.roomIndex);
        }
        else
        {
            planksAmount = Random.Range(1, 4);
            PlayerPrefs.SetInt("DoorOpener" + SceneManager.GetActiveScene().name + door.room.roomIndex, planksAmount);
        }


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
        }
        else
        {
            uses++;
        }
    }
}
