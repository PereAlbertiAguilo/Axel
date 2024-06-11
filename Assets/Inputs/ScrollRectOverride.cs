using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
 
public class ScrollRectOverride : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{

    public ScrollRect EdgesScroll;

    public void OnBeginDrag(PointerEventData data)
    {
    }

    public void OnDrag(PointerEventData data)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnEndDrag(PointerEventData data)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}

