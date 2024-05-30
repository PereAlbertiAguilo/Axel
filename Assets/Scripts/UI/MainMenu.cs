using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Transform cameraFollow;

    public void Play(string sceneName)
    {
        StartCoroutine(ChangeSceneDelay(sceneName));
    }

    public void Settings()
    {
        cameraFollow.position = Vector2.down * 18f;
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void Return()
    {
        cameraFollow.position = Vector2.zero;
    }

    void ChangeCurrentSelectedElement(GameObject selected)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selected);
    }

    IEnumerator ChangeSceneDelay(string sceneName)
    {
        Time.timeScale = 1;
        FadeBlack.instance.FadeToBlack();

        yield return new WaitForSeconds(.42f);

        SceneManager.LoadScene(sceneName);
    }
}
