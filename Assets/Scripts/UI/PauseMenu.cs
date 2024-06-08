using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button pauseButton;
    [SerializeField] Button resumeButton;

    bool paused = false;

    PlayerController playerController;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (UserInput.instance.pauseInput)
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

        if (Input.GetMouseButtonDown(0))
        {
            ChangeCurrentSelectedElement(null);
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
        ChangeCurrentSelectedElement(null);
        UpdateAnimators(true);
        paused = false;
        Time.timeScale = 1;
        playerController.canMove = true;
    }

    // Pauses the game
    public void Pause()
    {
        ChangeCurrentSelectedElement(null);
        UpdateAnimators(false);
        paused = true;
        //Time.timeScale = 0;
        playerController.canMove = false;
    }

    void ChangeCurrentSelectedElement(GameObject selected)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selected);
    }

    void UpdateAnimators(bool activate)
    {
        foreach(Animator a in FindObjectsOfType<Animator>())
        {
            if (a == FadeBlack.instance._animator) continue;
            a.enabled = activate;
        }
    }

    // Goes to the main menu
    public void Menu()
    {
        StartCoroutine(ChangeSceneDelay("MainMenu"));
    }

    // Adds a delay before changeing the scene
    IEnumerator ChangeSceneDelay(string sceneName)
    {
        Time.timeScale = 1;
        FadeBlack.instance.FadeToBlack();

        yield return new WaitForSeconds(.42f);

        SceneManager.LoadScene(sceneName);
    }
}
