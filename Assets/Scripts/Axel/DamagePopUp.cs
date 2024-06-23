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
            DamageManager damageManager = collision.transform.parent.GetComponent<DamageManager>();
            if (damageManager.isPlayer) damageManager.UpdateDamage(collision.transform.parent.GetComponent<PlayerAttack>() != null ?
                StatsManager.instance.GetStat(StatsManager.StatType.nDamage).statValue : StatsManager.instance.GetStat(StatsManager.StatType.tDamage).statValue);

            GameObject text = Instantiate(popUpText, transform.position, Quaternion.identity).transform.GetChild(0).gameObject;
            text.GetComponent<TextMeshProUGUI>().text = "" + damageManager.damage;
        }
    }
}
