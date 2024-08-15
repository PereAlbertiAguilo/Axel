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

        if (!Application.isFocused && paused)
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
        MenusManager.instance.ChangeCurrentSelectedElement(null);
        Cursor.visible = false;
        UpdateAnimators(true);
        UpdateEnemies(true);
        paused = false;
        PlayerController.instance.canMove = true;
    }

    // Pauses the game
    public void Pause()
    {
        MenusManager.instance.ChangeCurrentSelectedElement(resumeButton.gameObject);
        Cursor.visible = true;
        UpdateAnimators(false);
        UpdateEnemies(false);
        paused = true;
        PlayerController.instance.canMove = false;
    }

    void UpdateAnimators(bool activate)
    {
        foreach(Animator a in FindObjectsOfType<Animator>(true))
        {
            if (a == FadeBlack.instance._animator) continue;
            a.enabled = activate;
        }
        foreach(SpriteAnimation a in FindObjectsOfType<SpriteAnimation>())
        {
            if(!activate) a.StopAllCoroutines();
            a.enabled = activate;
            a.nextIteration = activate;
        }
    }

    void UpdateEnemies(bool activate)
    {
        foreach (Enemy e in FindObjectsOfType<Enemy>())
        {
            e.canMove = activate;
        }

        foreach (Projectile projectile in FindObjectsOfType<Projectile>())
        {
            projectile.enabled = activate;
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
