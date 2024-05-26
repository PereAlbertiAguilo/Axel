using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button pauseButton;
    [SerializeField] Button resumeButton;

    bool paused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                resumeButton.onClick.Invoke();
                Resume();
            }
            else
            {
                pauseButton.onClick.Invoke();
                Pause();
            }
        }

        // Dev tool
        if (Input.GetKeyDown(KeyCode.U))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Resumes the game
    public void Resume()
    {
        paused = false;
        Time.timeScale = 1;
    }

    // Pauses the game
    public void Pause()
    {
        paused = true;
        Time.timeScale = 0;
    }

    // Restarts the game
    //public void Restart()
    //{
    //    StartCoroutine(ChangeSceneDelay(SceneManager.GetActiveScene().name));
    //}

    // Goes to the main menu
    public void Menu()
    {
        StartCoroutine(ChangeSceneDelay("MainMenu"));
    }

    // Adds a delay before changeing the scene
    IEnumerator ChangeSceneDelay(string sceneName)
    {
        FadeBlack.instance.FadeToBlack();

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(sceneName);
    }
}
