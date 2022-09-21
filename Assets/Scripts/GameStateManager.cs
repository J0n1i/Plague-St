using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Playing,
    Paused,
    Loading,
    GeneratingDungeon
}


public class GameStateManager : MonoBehaviour
{

    public GameState currentGameState;
    private GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        currentGameState = GameState.Loading;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
