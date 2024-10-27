using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsTutorial : MonoBehaviour
{
    public GameObject keyboard, gamepad;

    void Update()
    {
        if (UserInput.instance.isKeyboard && !keyboard.activeInHierarchy && gamepad.activeInHierarchy)
        {
            keyboard.SetActive(true);
            gamepad.SetActive(false);
        }
        else if (UserInput.instance.isGamepad)
        {
            keyboard.SetActive(false);
            gamepad.SetActive(true);
        }
    }
}
