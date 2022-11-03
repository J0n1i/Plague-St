using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEncounter : MonoBehaviour
{
    private GameObject player, boss;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            player = col.gameObject;
            boss = GameObject.Find("Boss(Clone)");
            StartCoroutine(BossEncounterCo());
        }
    }



    IEnumerator BossEncounterCo()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Boss Encounter");
    }
}
