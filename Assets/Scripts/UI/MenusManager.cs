using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenusManager : MonoBehaviour
{
    public static MenusManager instance;

    public List<Button> returns = new List<Button>();

    int currentMenuIndex = 0;

    [HideInInspector] public bool inTransition;

    public bool canChangeSelectableWithMouse = true;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (UserInput.instance.pauseInput)
        {
            if(currentMenuIndex > 0)
            {
                returns[currentMenuIndex - 1].onClick.Invoke();
            }
        }

        if (EventSystem.current.IsPointerOverGameObject() && canChangeSelectableWithMouse)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();

            EventSystem.current.RaycastAll(eventData, results);

            GameObject lastSelectedObject = EventSystem.current.gameObject;

            foreach (RaycastResult result in results)
            {
                if(lastSelectedObject != FindComponentInAllParents<Button>(result.gameObject.transform))
                {
                    ChangeCurrentSelectedElement(FindComponentInAllParents<Button>(result.gameObject.transform));
                    return;
                }
                if (lastSelectedObject != FindComponentInAllParents<Slider>(result.gameObject.transform))
                {
                    ChangeCurrentSelectedElement(FindComponentInAllParents<Slider>(result.gameObject.transform));
                    return;
                }
                if (lastSelectedObject != FindComponentInAllParents<Toggle>(result.gameObject.transform))
                {
                    ChangeCurrentSelectedElement(FindComponentInAllParents<Toggle>(result.gameObject.transform));
                    return;
                }
                if (lastSelectedObject != FindComponentInAllParents<Scrollbar>(result.gameObject.transform))
                {
                    ChangeCurrentSelectedElement(FindComponentInAllParents<Scrollbar>(result.gameObject.transform));
                    return;
                }
            }
        }
    }
    
    public void ChangeCurrentMenu(int menuIndex)
    {
        currentMenuIndex = menuIndex;
    }

    public GameObject FindComponentInAllParents<T>(Transform transform)
    {
        GameObject lastSelectedObject = EventSystem.current.gameObject;

        Transform selectable = transform;

        T result;

        while (selectable != transform.root)
        {
            selectable = selectable.parent;

            result = selectable.GetComponent<T>();

            if (result != null)
            {
                //print("" + selectable.gameObject.name + " Is " + typeof(T));
                return selectable.gameObject;
            }
        }

        result = transform.gameObject.GetComponentInChildren<T>();

        return result != null ? (result as GameObject) : lastSelectedObject;
    }

    public void ChangeCurrentSelectedElement(GameObject selected)
    {
        if (selected == null) return;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selected);
    }

    IEnumerator ChangeSceneDelay(string sceneName)
    {
        inTransition = true;

        Time.timeScale = 1;
        FadeBlack.instance.FadeToBlack();

        Audio.instance.StopAllCoroutines();
        Audio.instance.StartCoroutine(Audio.instance.FadeAudioOut(Audio.instance.musicAudioSource, .5f));

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(ChangeSceneDelay(sceneName));
    }
}
