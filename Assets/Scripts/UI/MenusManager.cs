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

    [HideInInspector] public bool inTransition;

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
            ChangeScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayerPrefs.DeleteKey(SceneManager.GetActiveScene().name);
            ChangeScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.instance.RestartGame();
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
