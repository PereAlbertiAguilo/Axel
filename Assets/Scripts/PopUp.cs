using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public static PopUp instance;

    const float FONT_SIZE = .5f;

    GameObject popUpMessage;

    private void Awake()
    {
        instance = this;

        popUpMessage = Resources.Load("PopUpMessage") as GameObject;
    }

    public GameObject Message(Transform parent, string message, Color fontColor, float fontSize, float offset, float lifeTime, bool animate)
    {
        GameObject instance = Instantiate(popUpMessage, parent);

        instance.transform.localPosition += Vector3.up * offset;

        TextMeshProUGUI messageText = instance.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        DestroyOverTime destroyOverTime = instance.GetComponent<DestroyOverTime>();
        Animator animator = instance.GetComponentInChildren<Animator>();

        if (messageText != null)
        {
            Color newColor = new Color(fontColor.r, fontColor.g, fontColor.b, .75f);
            messageText.text = message;
            messageText.color = newColor;
            messageText.fontSize = fontSize;
        }
        
        if(destroyOverTime != null)
        {
            destroyOverTime.lifeTime = lifeTime;
        }

        if(animator != null)
        {
            animator.enabled = animate;
        }

        return instance;
    }

    public GameObject Message(Transform parent, string message, bool animate)
    {
        return Message(parent, message, Color.white, FONT_SIZE, 0, 1, animate);
    }

    public GameObject Message(Transform parent, string message, Color fontColor, float fontSize, bool animate)
    {
        return Message(parent, message, fontColor, fontSize, 0, 1, animate);
    }
    
    public GameObject Message(Transform parent, string message, Color fontColor, float fontSize, float offset, bool animate)
    {
        return Message(parent, message, fontColor, fontSize, offset, 1, animate);
    }

    public GameObject Message(Transform parent, string message, float lifeTime)
    {
        return Message(parent, message, Color.white, FONT_SIZE, 0, lifeTime, false);
    }
    
    public GameObject Message(Transform parent, string message, float lifeTime, float offset)
    {
        return Message(parent, message, Color.white, FONT_SIZE, offset, lifeTime, false);
    }
}
