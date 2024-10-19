using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    [HideInInspector] public float interactDelay = 1f;
    [HideInInspector] public bool hasUses = false;
    [HideInInspector, Min(1)] public int uses = 1;

    public Animator animator;

    protected bool canInteract = false;

    public string id;
    public Room room;

    public virtual void Awake()
    {
    }

    public virtual void Start()
    {
        id = id + SceneManager.GetActiveScene().name + room.roomIndex;
    }

    public virtual void Update()
    {
        if (UserInput.instance.interactInput && canInteract && HasUses())
        {
            Interact();
        }
    }

    public virtual void Interact()
    {
        Audio.instance.PlayOneShot(Audio.Sound.interact, .3f);
    }

    public virtual void SaveData()
    {
        InteractableManager.instance.SaveInteractableStructure(this);
    }

    protected bool HasUses()
    {
        if (hasUses)
        {
            if (uses <= 0) return false;
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

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && uses >= 1)
        {
            StartCoroutine(InteractDelay());
            animator.SetBool("IsInRange", true);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && uses >= 1)
        {
            StopAllCoroutines();
            canInteract = false;
            animator.SetBool("IsInRange", false);
        }
    }
}
