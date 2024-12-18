using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    [SerializeField] Button pauseButton;
    [SerializeField] Button resumeButton;

    public bool isPaused = false;

    public bool canPause = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Resume();

        StartCoroutine(PauseStartDelay(1.25f));
    }

    private void Update()
    {
        if (UserInput.instance.pauseInput)
        {
            if (isPaused)
            {
                if (transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    resumeButton.onClick.Invoke();
                    Resume();
                }
            }
            else if (!GameManager.instance.isGameOver && !MenusManager.instance.inTransition && canPause)
            {
                pauseButton.onClick.Invoke();
                Pause();
            }
        }

        if (!Application.isFocused && !isPaused && Time.timeScale != 0 && !GameManager.instance.isGameOver && canPause)
        {
            pauseButton.onClick.Invoke();
            Pause();
        }

        // Dev tool
        if (Input.GetKeyDown(KeyCode.U) && RoomManager.instance.generationComplete && canPause)
        {
            MenusManager.instance.ChangeScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.R) && RoomManager.instance.generationComplete && canPause)
        {
            GameManager.instance.RestartGame();
        }
    }

    // Resumes the game
    public void Resume()
    {
        isPaused = false;
        MenusManager.instance.ChangeCurrentSelectedElement(null);
        PlayerController.instance.canMove = true;
        Time.timeScale = 1;
        Audio.instance.UpdateSFX(false);
    }

    // Pauses the game
    public void Pause()
    {
        isPaused = true;
        MenusManager.instance.ChangeCurrentSelectedElement(resumeButton.gameObject);
        PlayerController.instance.canMove = false;
        Time.timeScale = 0;
        Audio.instance.UpdateSFX(true);
    }

    // Goes to the main menu
    public void Menu()
    {
        MenusManager.instance.ChangeScene("MainMenu");
    }

    IEnumerator PauseStartDelay(float duration)
    {
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            canPause = false;

            yield return null;
        }

        canPause = true;
    }
}
