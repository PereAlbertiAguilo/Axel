using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    [SerializeField] GameObject popUpText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Orb"))
        {
            PopUp(StatsManager.instance.GetStat(StatsManager.StatType.nDamage).statValue, .5f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PopUp(GetComponent<DamageManager>().damage, .5f);
        }
    }

    public void PopUp(float damage, float textSize)
    {
        TextMeshProUGUI text = Instantiate(popUpText, transform.position, Quaternion.identity).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.fontSize = textSize;
        text.text = "" + Math.Round(damage, 2);
    }
}
