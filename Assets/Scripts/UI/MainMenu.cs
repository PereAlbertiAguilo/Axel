using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Transform cameraFollow;

    public void Play(string sceneName)
    {
        StartCoroutine(ChangeSceneDelay(sceneName));
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void MovePanel(float offset)
    {
        cameraFollow.position = Vector3.up * offset;
    }

    IEnumerator ChangeSceneDelay(string sceneName)
    {
        Time.timeScale = 1;
        FadeBlack.instance.FadeToBlack();

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }
}
