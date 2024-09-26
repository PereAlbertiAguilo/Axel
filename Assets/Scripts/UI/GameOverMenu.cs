using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverPanel;

    public TextMeshProUGUI time, floor, murderer;

    public GameObject restartButton;

    public static GameOverMenu instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetGameOverPanel()
    {
        gameOverPanel.SetActive(true);

        foreach (Transform t in HudManager.instance.transform)
        {
            t.gameObject.SetActive(false);
        }

        MenusManager.instance.ChangeCurrentSelectedElement(restartButton);

        time.text = "Time: " + HudManager.instance.Timer();
        floor.text = "Last floor: " + HudManager.instance.FloorName(false);
        murderer.text = "Died to: " + PlayerController.instance.lastHitByName;

        GameManager.instance.ResetGameData();
    }

    public void PlayAgain()
    {
        GameManager.instance.RestartGame();
    }

    public void Menu()
    {
        PauseMenu.instance.Menu();
    }
}
