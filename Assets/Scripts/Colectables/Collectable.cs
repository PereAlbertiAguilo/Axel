using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    // Custom Inspector properties
    [HideInInspector] public CollectiblesManager.Types type;
    [HideInInspector] public int amount = 1;
    [HideInInspector] public AudioClip pickUpSound;
    [HideInInspector, Range(0, 1)] public float volumeScale;

    // Hiden properties
    [HideInInspector] public Room room;
    [HideInInspector] public int index;
    [HideInInspector] public string prefabPath;
    [HideInInspector] public bool pickedUp = false;

    private void Start()
    {
        if (pickedUp) gameObject.SetActive(false);
    }

    public void PickUpColectable(CollectiblesManager collectiblesManager)
    {
        collectiblesManager.SetCollectable(type, amount);

        if (pickUpSound != null) Audio.instance.PlayOneShot(pickUpSound, volumeScale, true);

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
