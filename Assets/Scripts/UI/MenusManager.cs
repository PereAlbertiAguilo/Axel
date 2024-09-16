using System.Collections;
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

    public IEnumerator ChangeSceneDelay(string sceneName)
    {
        Time.timeScale = 1;
        FadeBlack.instance.FadeToBlack();

        Audio.instance.StopAllCoroutines();
        Audio.instance.StartCoroutine(Audio.instance.FadeAudioOut(Audio.instance.musicAudioSource, .5f));

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }
}
