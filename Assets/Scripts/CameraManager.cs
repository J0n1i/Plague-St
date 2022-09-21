using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameObject player;
    private GameStateManager gameStateManager;
    // Start is called before the first frame update
    void Start()
    {
        gameStateManager = GameObject.FindWithTag("GameManager").GetComponent<GameStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null){
            player = GameObject.FindWithTag("Player");
        }
        
        if(gameStateManager.currentGameState == GameState.Playing){
            transform.position = Vector2.Lerp(transform.position, player.transform.position, Time.deltaTime * 5);
        }
    }
}
