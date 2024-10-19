using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Room room;
    public CollectiblesManager.Types collectable;

    public int amount = 1;

    public AudioClip pickUpSound;

    public int index;
    public string prefabPath;

    public bool pickedUp = false;

    private void Start()
    {
        if (pickedUp) gameObject.SetActive(false);
    }

    public void PickUpColectable(CollectiblesManager collectiblesManager)
    {
        collectiblesManager.SetCollectable(collectable, amount);

        if (pickUpSound != null) Audio.instance.PlayOneShot(pickUpSound, .05f, true);

        pickedUp = true;

        CollectiblesManager.instance.SaveCollectibles();

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out CollectiblesManager collectiblesManager))
        {
            PickUpColectable(collectiblesManager);
        }
    }
}
