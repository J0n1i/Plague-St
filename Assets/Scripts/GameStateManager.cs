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
    private bool GameStatusBool = false;
    public Animator Fade;
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
        if (currentGameState != GameState.GeneratingDungeon)
        {
            Fade.SetBool("BGFading", true);
        }
    }
}
