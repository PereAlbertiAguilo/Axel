using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    Sprite[] effectSprites;

    public static PopUp instance;

    const float FONT_SIZE = .5f;

    GameObject popUpMessage;
    GameObject popUpSprite;

    private void Awake()
    {
        instance = this;

        popUpMessage = Resources.Load("PopUps/PopUpMessage") as GameObject;
        popUpSprite = Resources.Load("PopUps/PopUpSprite") as GameObject;
        effectSprites = Resources.LoadAll<Sprite>($"Effects/EfectSprites/EffectIcons");
    }

    public GameObject Message(Transform parent, string message, Color fontColor, float fontSize, float offset, bool fade, float lifeTime, bool animate)
    {
        GameObject instance = Instantiate(popUpMessage, parent);

        instance.transform.localPosition += Vector3.up * offset;

        if(instance.transform.GetChild(0).GetChild(0).TryGetComponent(out TextMeshProUGUI messageText))
        {
            messageText.text = message;
            messageText.color = fontColor;
            messageText.fontSize = fontSize;

            if (fade) StartCoroutine(FadeOut(messageText, lifeTime));
        }
        
        if(instance.TryGetComponent(out DestroyOverTime destroyOverTime)) destroyOverTime.lifeTime = lifeTime;

        if(instance.transform.GetChild(0).TryGetComponent(out Animator animator)) animator.enabled = animate;

        return instance;
    }

    public GameObject Sprite(Transform parent, EffectParameters.Type effectName, Color spriteColor, float size, float offset, bool fade, float lifeTime, bool animate)
    {
        GameObject instance = Instantiate(popUpSprite, parent);

        instance.transform.localPosition += Vector3.up * offset;

        if (instance.transform.GetChild(0).GetChild(0).TryGetComponent(out Image image))
        {
            image.sprite = effectSprites[(int)effectName];
            image.color = spriteColor;
            image.transform.localScale = Vector3.one * size;

            if (fade) StartCoroutine(FadeOut(image, lifeTime));
        }

        if (instance.TryGetComponent(out DestroyOverTime destroyOverTime)) destroyOverTime.lifeTime = lifeTime;

        if (instance.transform.GetChild(0).TryGetComponent(out Animator animator)) animator.enabled = animate;

        return instance;
    }

    #region MessageRegion
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
    #endregion

    IEnumerator FadeOut(MaskableGraphic graphic, float duration)
    {
        float timeToFade = 10 * duration / 100;
        float currentTime = timeToFade;

        yield return new WaitForSeconds(duration - timeToFade);

        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (graphic != null) graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, (currentTime * 2) / (timeToFade * 2));

            yield return null;
        }
    }
}
