using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    private GameStateManager gameStateManager;
    private bool paused = false;
    private bool playerSpawned = false;
    private GameObject player;
    public GameObject canvas;

    [SerializeField] private GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        gameStateManager = GetComponent<GameStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameStateManager.currentGameState == GameState.Playing)
        {
            PauseGame(true);
        }else if (Input.GetKeyDown(KeyCode.Escape) && gameStateManager.currentGameState == GameState.Paused)
        {
            PauseGame(false);
        }

        if(playerSpawned == false && gameStateManager.currentGameState == GameState.Playing)
        {
            SpawnPlayer();
        }

        if(playerSpawned == true && gameStateManager.currentGameState == GameState.GeneratingDungeon){
            Destroy(player);
            playerSpawned = false;
        }
    }

    private void SpawnPlayer(){
        player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        playerSpawned = true;
        canvas.gameObject.SetActive(true);
    }

    void PauseGame(bool pause)
    {
        Debug.Log(pause);
        if (pause == true)
        {
            Time.timeScale = 0;
            gameStateManager.currentGameState = GameState.Paused;
            paused = true;
        }
        else
        {
            Time.timeScale = 1;
            gameStateManager.currentGameState = GameState.Playing;
            paused = false;
        }
    }
}
