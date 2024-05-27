using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsManager : MonoBehaviour
{
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OpenDoors(bool[] doorsState)
    {
        for (int i = 0; i < doorsState.Length; i++)
        {
            if (!doorsState[i])
            {
                continue;
            }

            _animator.SetBool(_animator.GetParameter(i).name, doorsState[i]);
        }
    }
}
