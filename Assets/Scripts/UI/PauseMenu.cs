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

        Invoke(nameof(PauseStartDelay), 2f);
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
            else if (Time.timeScale <= 1 && !GameManager.instance.isGameOver && !MenusManager.instance.inTransition && canPause)
            {
                pauseButton.onClick.Invoke();
                Pause();
            }
        }

        if (!Application.isFocused && !isPaused && Time.timeScale != 0 && !GameManager.instance.isGameOver)
        {
            //pauseButton.onClick.Invoke();
            //Pause();
        }
    }

    // Resumes the game
    public void Resume()
    {
        isPaused = false;
        MenusManager.instance.ChangeCurrentSelectedElement(null);
        Cursor.visible = false;
        PlayerController.instance.canMove = true;
        Time.timeScale = 1;
        Audio.instance.UpdateSFX(false);
    }

    // Pauses the game
    public void Pause()
    {
        isPaused = true;
        MenusManager.instance.ChangeCurrentSelectedElement(resumeButton.gameObject);
        Cursor.visible = true;
        PlayerController.instance.canMove = false;
        Time.timeScale = 0;
        Audio.instance.UpdateSFX(true);
    }

    // Goes to the main menu
    public void Menu()
    {
        MenusManager.instance.ChangeScene("MainMenu");
    }

    void PauseStartDelay()
    {
        canPause = true;
    }
}
