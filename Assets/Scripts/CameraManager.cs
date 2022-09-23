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

        float x = Mathf.Round(player.transform.position.x / 37) * 37;
        float y = Mathf.Round(player.transform.position.y / 23) * 23;

        transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, transform.position.z), Time.deltaTime *5 );
        }

    }
}
