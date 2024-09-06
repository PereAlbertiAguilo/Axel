using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOverTime : MonoBehaviour
{
    public float lifeTime = 2f;

    private void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(Deactivate), lifeTime);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
