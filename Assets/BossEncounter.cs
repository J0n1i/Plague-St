using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossEncounter : MonoBehaviour
{
    private GameObject player, boss, mainCamera, bossRoom;
    private bool encounterStarted = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            player = col.gameObject;
            boss = GameObject.Find("Boss(Clone)");
            if (boss == null)
            {
                boss = GameObject.Find("Boss 1(Clone)");
            }
            mainCamera = Camera.main.gameObject;
            if (encounterStarted == false)
            {
                encounterStarted = true;
                StartCoroutine(BossEncounterCo());
            }
        }
    }



    IEnumerator BossEncounterCo()
    {
        Debug.Log("Boss Encounter started");


        player.GetComponent<PlayerMovement>().inputEnabled = false;

        yield return new WaitForSeconds(0.6f);

        mainCamera.GetComponent<CameraManager>().ShakeDamage();
        bossRoom = GameObject.FindGameObjectWithTag("bossroom");
        for (int i = 0; i < 4; i++)
        {
            bossRoom.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(1.5f);

        //Pan camera to boss
        mainCamera.GetComponent<CameraManager>().followTarget = boss;
        yield return new WaitForSeconds(2f);

        //pan camera to player and enable input
        mainCamera.GetComponent<CameraManager>().followTarget = player;
        player.GetComponent<PlayerMovement>().inputEnabled = true;


        //Enable boss AI
        if (boss.GetComponent<BossEnemy>() != null)
        {
            boss.GetComponent<BossEnemy>().chaseRadius = 100f;
        }
        else
        {
            boss.GetComponent<BossEnemy1>().chaseRadius = 100f;
            boss.GetComponent<BossEnemy1>().attackRadius = 100f;
        }

    }


    public void BossDeath()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().inputEnabled = false;
        player.GetComponent<PlayerMovement>().triggerCollider.enabled = false;
        Debug.Log("Boss Died");
        StartCoroutine(BossDeathCo());
        DontDestroyOnLoad(player);
        GameObject.Find("HeartContainers").GetComponent<HeartManager>().revivePlayer();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            BossDeath();
        }

        if (fadeScreen != null)
        {
            fadeScreen.color = Color.Lerp(fadeScreen.color, Color.black, 4 * Time.deltaTime);
        }
    }


    private Image fadeScreen;
    IEnumerator BossDeathCo()
    {
        yield return new WaitForSeconds(3f);
        fadeScreen = GameObject.Find("FadeOut").GetComponent<Image>();

        yield return new WaitForSeconds(3f);
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().inputEnabled = true;
        player.GetComponent<PlayerMovement>().triggerCollider.enabled = true;
        SceneManager.LoadScene(2);
    }


}
