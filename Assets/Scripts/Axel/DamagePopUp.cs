using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(DamageManager))]
public class DamagePopUp : MonoBehaviour
{
    [SerializeField] GameObject popUpText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Orb"))
        {
            DamageManager damageManager = collision.GetComponent<DamageManager>();

            GameObject text = Instantiate(popUpText, transform.position, Quaternion.identity).transform.GetChild(0).gameObject;
            text.GetComponent<TextMeshProUGUI>().text = "" + damageManager.damage;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
