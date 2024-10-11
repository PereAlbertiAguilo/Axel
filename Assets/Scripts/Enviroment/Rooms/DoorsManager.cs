using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsManager : MonoBehaviour
{
    public Animator animatorUp;
    public Animator animatorDown;
    public Animator animatorRight;
    public Animator animatorLeft;

    public Door doorUp;
    public Door doorDown;
    public Door doorRight;
    public Door doorLeft;

    public void OpenDoors(string stateName, bool doorState)
    {
        if (!doorUp.isLocked) animatorUp.SetBool(stateName, doorState);
        if (!doorDown.isLocked) animatorDown.SetBool(stateName, doorState);
        if (!doorRight.isLocked) animatorRight.SetBool(stateName, doorState);
        if (!doorLeft.isLocked) animatorLeft.SetBool(stateName, doorState);
    }
}
