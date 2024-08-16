using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenusManager : MonoBehaviour
{
    public static MenusManager instance;

    public List<Button> returns = new List<Button>();

    int currentMenuIndex = 0;

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

        if (Input.anyKeyDown && EventSystem.current.currentSelectedGameObject == null && currentMenuIndex >= 0 && Selectable.allSelectablesArray.Length > 0)
        {
            Vector2 lastselectablePos = new Vector2(Screen.width, 0);
            Selectable selectabelToSelect = null;

            foreach (Selectable selectable in Selectable.allSelectablesArray)
            {
                if (lastselectablePos.x > selectable.transform.position.x)
                {
                    selectabelToSelect = selectable;
                    lastselectablePos = selectable.transform.position;
                }

                if (lastselectablePos.y < selectable.transform.position.y)
                {
                    selectabelToSelect = selectable;
                    lastselectablePos = selectable.transform.position;
                }
            }

            ChangeCurrentSelectedElement(selectabelToSelect.gameObject);
        }

        // Dev tool
        if (Input.GetKeyDown(KeyCode.U))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void ChangeCurrentMenu(int menuIndex)
    {
        currentMenuIndex = menuIndex;
    }

    public void ChangeCurrentSelectedElement(GameObject selected)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selected);
    }
}
