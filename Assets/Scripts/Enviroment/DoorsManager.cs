using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsManager : MonoBehaviour
{
    public Animator animatorUp;
    public Animator animatorDown;
    public Animator animatorRight;
    public Animator animatorLeft;

    public void OpenDoor(string doorStateParameter, bool doorState)
    {
        animatorUp.SetBool(doorStateParameter, doorState);
        animatorDown.SetBool(doorStateParameter, doorState);
        animatorRight.SetBool(doorStateParameter, doorState);
        animatorLeft.SetBool(doorStateParameter, doorState);
    }
}
