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

    public bool paused = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Resume();
    }

    private void Update()
    {
        if (UserInput.instance.pauseInput)
        {
            if (paused)
            {
                if (transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    resumeButton.onClick.Invoke();
                    Resume();
                }
            }
            else
            {
                pauseButton.onClick.Invoke();
                Pause();
            }
        }

        if (!Application.isFocused && !paused)
        {
            pauseButton.onClick.Invoke();
            Pause();
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
        MenusManager.instance.ChangeCurrentSelectedElement(null);
        Cursor.visible = false;
        PlayerController.instance.canMove = true;
        Time.timeScale = 1;
        Audio.instance.UpdateSFX(false);
    }

    // Pauses the game
    public void Pause()
    {
        paused = true;
        MenusManager.instance.ChangeCurrentSelectedElement(resumeButton.gameObject);
        Cursor.visible = true;
        PlayerController.instance.canMove = false;
        Time.timeScale = 0;
        Audio.instance.UpdateSFX(true);
    }

    // Goes to the main menu
    public void Menu()
    {
        MenusManager.instance.StartCoroutine(MenusManager.instance.ChangeSceneDelay("MainMenu"));
    }
}
