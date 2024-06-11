using Pathfinding.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeybindsMenu : MonoBehaviour
{
    public GameObject currentSelectedGameObject;

    [SerializeField] ScrollRect scrollRect;

    public List<GameObject> scrollItems = new List<GameObject>();

    private void Start()
    {
        foreach (Transform item in scrollRect.content.transform)
        {
            scrollItems.Add(item.gameObject);
        }
    }

    private void Update()
    {
        currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;

        if (currentSelectedGameObject != null && transform.GetChild(0).gameObject.activeInHierarchy && scrollRect.verticalScrollbar.gameObject != currentSelectedGameObject)
        {
            foreach (GameObject item in scrollItems)
            {
                float itemRectY = item.GetComponent<RectTransform>().localPosition.y;

                if (item == currentSelectedGameObject)
                {
                    scrollRect.content.localPosition = Vector3.Lerp(scrollRect.content.localPosition, new Vector3(scrollRect.content.localPosition.x, Mathf.Abs(itemRectY), 0), Time.deltaTime * 5);
                }
                else
                {
                    foreach (Transform childItem in item.transform)
                    {
                        if (childItem.gameObject == currentSelectedGameObject)
                        {
                            scrollRect.content.localPosition = Vector3.Lerp(scrollRect.content.localPosition, new Vector3(scrollRect.content.localPosition.x, Mathf.Abs(itemRectY), 0), Time.deltaTime * 5);
                        }
                    }
                }
            }
        }
    }
}
