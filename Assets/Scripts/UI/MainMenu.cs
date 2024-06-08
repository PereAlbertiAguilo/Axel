using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Transform cameraFollow;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeCurrentSelectedElement(FindObjectsOfType<Button>()[FindObjectsOfType<Button>().Length - 1].gameObject);
        }
    }

    public void Play(string sceneName)
    {
        StartCoroutine(ChangeSceneDelay(sceneName));
    }

    public void SettingsAndKeyBinds(float offset)
    {
        cameraFollow.position = Vector2.down * offset;
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void Return(float offset)
    {
        cameraFollow.position = Vector2.up * offset;
    }

    public void ChangeCurrentSelectedElement(GameObject selected)
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
