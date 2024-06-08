using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public KeyCode normalAttackKey;
    public KeyCode throwAttackKey;
    public KeyCode dashKey;
    public KeyCode reloadKey;
    public KeyCode pauseKey;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData() 
    {
        normalAttackKey = KeyCode.Mouse0;
        throwAttackKey = KeyCode.Mouse1;
        dashKey = KeyCode.Space;
        reloadKey = KeyCode.R;
        pauseKey = KeyCode.Escape;
    }
}
