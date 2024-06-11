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

    public bool paused = false;

    PlayerController playerController;

    public static PauseMenu instance;

    private void Awake()
    {
        instance = this;

        playerController = FindObjectOfType<PlayerController>();
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

        if (Input.anyKeyDown && EventSystem.current.currentSelectedGameObject == null && paused)
        {
            ChangeCurrentSelectedElement(FindObjectsOfType<Button>()[FindObjectsOfType<Button>().Length - 1].gameObject);
        }

        if (!Application.isPlaying && !paused)
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
        ChangeCurrentSelectedElement(null);
        UpdateAnimators(true);
        UpdateEnemies(true);
        paused = false;
        playerController.canMove = true;
    }

    // Pauses the game
    public void Pause()
    {
        ChangeCurrentSelectedElement(resumeButton.gameObject);
        UpdateAnimators(false);
        UpdateEnemies(false);
        paused = true;
        playerController.canMove = false;
    }

    public void ChangeCurrentSelectedElement(GameObject selected)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selected);
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
        foreach (EnemyController e in FindObjectsOfType<EnemyController>())
        {
            e.canMove = activate;
            e.DeactivateFollowState();
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
