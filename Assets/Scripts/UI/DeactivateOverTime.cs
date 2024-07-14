using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOverTime : MonoBehaviour
{
    [SerializeField] float lifeTime = 2f;

    private void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(Deactivate), lifeTime);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
