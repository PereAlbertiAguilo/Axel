using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InteractableStructure
{
    public bool hasUses;
    public int uses;
    public string id;

    public int[] values = new int[3];
}
