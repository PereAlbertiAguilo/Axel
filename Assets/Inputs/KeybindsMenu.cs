using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeybindsMenu : MonoBehaviour
{
    GameObject currentSelectedGameObject;

    [SerializeField] GameObject rebindOverlay;
    [SerializeField] ScrollRect scrollRect;

    List<GameObject> scrollItems = new List<GameObject>();

    bool canScroll = false;

    private void Start()
    {
        foreach (Transform item in scrollRect.content.transform)
        {
            scrollItems.Add(item.gameObject);
        }
    }

    private void Update()
    {
        if (PauseMenu.instance != null && rebindOverlay.activeInHierarchy)
        {
            PauseMenu.instance.canPause = false;
        }

        if (Input.mouseScrollDelta.y != 0 || scrollRect.velocity.y != 0 && EventSystem.current.currentSelectedGameObject != scrollRect.verticalScrollbar.gameObject)
        {
            MenusManager.instance.ChangeCurrentSelectedElement(scrollRect.verticalScrollbar.gameObject);
            canScroll = true;
        }
        else if(Input.anyKeyDown)
        {
            canScroll= false;
        }

        MenusManager.instance.canChangeSelectableWithMouse = !scrollRect.gameObject.activeInHierarchy;

        currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;

        if (currentSelectedGameObject != null && transform.GetChild(0).gameObject.activeInHierarchy && 
            scrollRect.verticalScrollbar.gameObject != currentSelectedGameObject && !canScroll)
        {
            foreach (GameObject item in scrollItems)
            {
                float itemRectY = item.GetComponent<RectTransform>().localPosition.y;

                if (item == currentSelectedGameObject)
                {
                    scrollRect.content.localPosition = Vector3.Lerp(scrollRect.content.localPosition, new Vector3(scrollRect.content.localPosition.x, Mathf.Abs(itemRectY), 0), Time.unscaledDeltaTime * 10);
                }
                else
                {
                    foreach (Transform childItem in item.transform)
                    {
                        if (childItem.gameObject == currentSelectedGameObject)
                        {
                            scrollRect.content.localPosition = Vector3.Lerp(scrollRect.content.localPosition, new Vector3(scrollRect.content.localPosition.x, Mathf.Abs(itemRectY), 0), Time.unscaledDeltaTime * 10);
                        }
                    }
                }
            }
        }
    }
}
