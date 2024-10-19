using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableManager : MonoBehaviour
{
    public List<Interactable> interactables = new List<Interactable>();

    public static InteractableManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(GetInteractables());
    }

    public void SaveInteractableStructure(Interactable interactable)
    {
        List<InteractableStructure> interactableStructures = DataPersistenceManager.instance.gameData.interactableStructures.ToList();

        if (interactableStructures.Exists(x => x.id == interactable.id))
        {
            InteractableStructure savedStructure = interactableStructures.Find(x => x.id == interactable.id);

            savedStructure.hasUses = interactable.hasUses;
            savedStructure.uses = interactable.uses;
            savedStructure.id = interactable.id;
        }
        else
        {
            InteractableStructure interactableStructure = new InteractableStructure();

            interactableStructure.hasUses = interactable.hasUses;
            interactableStructure.uses = interactable.uses;
            interactableStructure.id = interactable.id;

            interactableStructures.Add(interactableStructure);
        }
        
        DataPersistenceManager.instance.gameData.interactableStructures = interactableStructures.ToArray();
    }

    public void UnsaveInteractableStructure(Interactable interactable)
    {
        List<InteractableStructure> interactableStructures = DataPersistenceManager.instance.gameData.interactableStructures.ToList();

        interactableStructures.RemoveAll(x => x.id == interactable.id);

        DataPersistenceManager.instance.gameData.interactableStructures = interactableStructures.ToArray();
    }

    public void LoadInteractablesStructures()
    {
        InteractableStructure[] interactableStructures = DataPersistenceManager.instance.gameData.interactableStructures;

        foreach (InteractableStructure interactableStructure in interactableStructures)
        {
            Interactable interactable = interactables.Find(x => x.id == interactableStructure.id);

            if (interactable != null)
            {
                interactable.hasUses = interactableStructure.hasUses;
                interactable.uses = interactableStructure.uses;
            }
        }
    }

    public IEnumerator GetInteractables()
    {
        yield return new WaitUntil(() => RoomManager.instance.generationComplete);

        foreach (Interactable interactable in FindObjectsOfType<Interactable>(true))
        {
            interactables.Add(interactable);
        }

        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name))
        {
            LoadInteractablesStructures();
        }
    }

    public int GetInteractableValue(string id, int index, int fallBack)
    {
        InteractableStructure interactableStructure = null;

        foreach (InteractableStructure structure in DataPersistenceManager.instance.gameData.interactableStructures)
        {
            if (structure.id == id)
            {
                interactableStructure = structure;
                break;
            }
        }

        if (interactableStructure != null)
        {
            return interactableStructure.values[index];
        }

        return fallBack;
    }

    public void SetInteractableValues(string id, int value, int index)
    {
        InteractableStructure interactableStructure = DataPersistenceManager.instance.gameData.interactableStructures.ToList().Find(x => x.id == id);
        if (interactableStructure != null)
        {
            interactableStructure.values[index] = value;
        }
    }
}
