using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float health;
    public float healthCurrent;
    public float damage;
    public float damageCurrent;
    public float speed;
    public float speedCurrent;
    public float defense;
    public float defenseCurrent;
    public float attackSpeed;
    public float attackSpeedCurrent;

    public WeaponManager.Element weaponElement;
    public WeaponManager.Type weaponType;

    public float weaponAddedDamage;
    public float weaponAddedAttackSpeed;

    public int effectPower;

    public string currentFloor;

    public RoomStructure[] roomStructures;
    public CollectableStructure[] collectableStructures;
    public InteractableStructure[] interactableStructures;

    public Vector2 playerPos;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData() 
    {
        health = 50;
        healthCurrent = 50;
        damage = 5;
        damageCurrent = 5;
        speed = 60;
        speedCurrent = 60;
        defense = 50;
        defenseCurrent = 50;
        attackSpeed = 1;
        attackSpeedCurrent = 1;

        weaponElement = WeaponManager.Element.water;
        weaponType = WeaponManager.Type.sword;

        effectPower = 2;

        currentFloor = "FirstFloor";

        playerPos = new Vector2(0, 0.5f);
    }
}
