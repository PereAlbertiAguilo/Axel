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

    public GameObject Message(Transform parent, string message, Color fontColor, float fontSize, float offset, bool fade, float lifeTime, bool animate)
    {
        GameObject instance = Instantiate(popUpMessage, parent);

        instance.transform.localPosition += Vector3.up * offset;

        TextMeshProUGUI messageText = instance.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        DestroyOverTime destroyOverTime = instance.GetComponent<DestroyOverTime>();
        Animator animator = instance.GetComponentInChildren<Animator>();

        if (messageText != null)
        {
            //Color newColor = new Color(fontColor.r, fontColor.g, fontColor.b, fade ? 1 : .5f);
            messageText.text = message;
            messageText.color = fontColor;
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

        if (fade)
        {
            StartCoroutine(FadeOut(messageText, lifeTime));
        }

        return instance;
    }

    public GameObject Message(Transform parent, string message, bool animate)
    {
        return Message(parent, message, Color.white, FONT_SIZE, 0, false, 1, animate);
    }

    public GameObject Message(Transform parent, string message, Color fontColor, float fontSize, bool animate)
    {
        return Message(parent, message, fontColor, fontSize, 0, false, 1, animate);
    }
    
    public GameObject Message(Transform parent, string message, Color fontColor, float fontSize, float offset, bool animate)
    {
        return Message(parent, message, fontColor, fontSize, offset, false, 1, animate);
    }

    public GameObject Message(Transform parent, string message, float lifeTime)
    {
        return Message(parent, message, Color.white, FONT_SIZE, 0, false, lifeTime, false);
    }
    
    public GameObject Message(Transform parent, string message, float lifeTime, float offset)
    {
        return Message(parent, message, Color.white, FONT_SIZE, offset, false, lifeTime, false);
    }

    public GameObject Message(Transform parent, string message, Color fontColor, float fontSize, float lifeTime, bool fade, float offset)
    {
        return Message(parent, message, fontColor, fontSize, offset, fade, lifeTime, false);
    }

    IEnumerator FadeOut(TextMeshProUGUI text, float duration)
    {
        float timeToFade = 10 * duration / 100;
        float currentTime = timeToFade;

        yield return new WaitForSeconds(duration - timeToFade);

        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (text != null) text.color = new Color(text.color.r, text.color.g, text.color.b, (currentTime * 2) / (timeToFade * 2));

            yield return null;
        }
    }
}
