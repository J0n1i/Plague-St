using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{

    [SerializeField] private GameObject startingRoom;

    public GameObject[] upRooms, rightRooms, downRooms, leftRooms, allRooms;

    [Range(0, 100)][SerializeField] private int[] probabilities = new int[4];
    public List<GameObject> rooms;
    [HideInInspector] public List<GameObject> spawnNodes;
    [SerializeField] private bool randomStartingRoom = false;


    [Range(0, 255)][SerializeField] private int minRooms, maxRooms;
    [SerializeField][Range(0, 0.5f)] float generationSpeed = 0;

    private GameStateManager gameStateManager;
    private LevelManager levelManager;

    [SerializeField] private int seed;
    private int calculaitonSeed = 0;

    private float generationTimer = 0;

    [SerializeField] private string decodeSeed = "";


    void Start()
    {

        if (decodeSeed != "")
        {
            DecodeSeed(decodeSeed);
        }

        if (seed == 0)
        {
            seed = Random.seed;
        }

        Debug.Log(EncodeSeed(seed));


        calculaitonSeed = seed;
        Random.InitState(calculaitonSeed);

        levelManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LevelManager>();
        gameStateManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
        GenerateDungeon();
    }

    private string GenerateSeed()
    {
        //generate random 21 digi long hexadecimal
        string seed = "";
        for (int i = 0; i < 21; i++)
        {
            seed += Random.Range(0, 16).ToString("X");
        }
        return seed;
    }


    private string EncodeSeed(int seed)
    {
        string hex = "";
        for (int i = 0; i < probabilities.Length; i++)
        {
            hex += probabilities[i].ToString("X2");
        }

        hex += randomStartingRoom ? "1" : "0";

        hex += minRooms.ToString("X2");

        hex += maxRooms.ToString("X2");

        hex += seed.ToString("X8");

        return hex;
    }

    private void DecodeSeed(string hex) //21 digit long hexadecimal
    {
        for (int i = 0; i < probabilities.Length; i++)
        {
            probabilities[i] = int.Parse(hex.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber) % 101;
        }

        randomStartingRoom = hex.Substring(probabilities.Length * 2, 1) == "1";

        minRooms = int.Parse(hex.Substring(probabilities.Length * 2 + 1, 2), System.Globalization.NumberStyles.HexNumber) % 256;
        maxRooms = int.Parse(hex.Substring(probabilities.Length * 2 + 3, 2), System.Globalization.NumberStyles.HexNumber) % 256;

        seed = int.Parse(hex.Substring(probabilities.Length * 2 + 5, 8), System.Globalization.NumberStyles.HexNumber);
    }



    public void GenerateDungeon()
    {
        gameStateManager.currentGameState = GameState.GeneratingDungeon;
        GameObject newRoom;
        if (randomStartingRoom == false)
        {
            newRoom = Instantiate(startingRoom, transform.GetChild(0));
        }
        else
        {
            newRoom = Instantiate(allRooms[Random.Range(0, allRooms.Length)], transform.GetChild(0));
        }

        rooms.Add(newRoom);
        spawnNodes.AddRange(newRoom.GetComponent<RoomPrefab>().spawnNodes);
        StartCoroutine(GenerationLoop());
    }


    private GameObject newRoom;

    private int RandomRoom()
    {
        calculaitonSeed++;

        if (rooms.Count > maxRooms) //ends room creation if max rooms exceeded
        {
            return 4;
        }

        int sum = 0;
        for (int i = 0; i < probabilities.Length; i++)
        {
            sum += probabilities[i];
        }

        int randomNumber = Random.Range(0, sum);
        int[] j = new int[4];

        j[0] = probabilities[0];
        j[1] = j[0] + probabilities[1];
        j[2] = j[1] + probabilities[2];
        j[3] = j[2] + probabilities[3];

        if (randomNumber < j[0])
        {
            return 0;
        }
        else if (randomNumber >= j[0] && randomNumber < j[1])
        {
            return 1;
        }
        else if (randomNumber >= j[1] && randomNumber < j[2])
        {
            return Random.Range(2, 4);
        }
        else
        {
            return 4;
        }
    }

    private float roomsPerSecond = 0;

    IEnumerator GenerationLoop()
    {
        while (dungeonComplete == false)
        {
            for (int i = 0; i < spawnNodes.Count; i++)
            {
                var node = spawnNodes[i];
                if (node == null) { break; }
                string direction = node.GetComponent<SpawnNode>().nodeDirection;
                switch (direction)
                {
                    case "u":
                        newRoom = Instantiate(upRooms[RandomRoom()], node.transform.position, Quaternion.Euler(0, 0, 0), transform.GetChild(0));
                        break;
                    case "r":
                        newRoom = Instantiate(rightRooms[RandomRoom()], node.transform.position, Quaternion.Euler(0, 0, 0), transform.GetChild(0));
                        break;
                    case "d":
                        newRoom = Instantiate(downRooms[RandomRoom()], node.transform.position, Quaternion.Euler(0, 0, 0), transform.GetChild(0));
                        break;
                    case "l":
                        newRoom = Instantiate(leftRooms[RandomRoom()], node.transform.position, Quaternion.Euler(0, 0, 0), transform.GetChild(0));
                        break;
                    default:
                        Debug.Log("Error in direction selection");
                        break;
                }
                i = -1;
                spawnNodes.Remove(node);
                Destroy(node);
                spawnNodes.AddRange(newRoom.GetComponent<RoomPrefab>().spawnNodes);
                rooms.Add(newRoom);
                roomsPerSecond++;
                yield return new WaitForSecondsRealtime(generationSpeed);
            }
            spawnNodes.RemoveAll(item => item == null);
            yield return null;
        }
    }


    private bool dungeonComplete = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetDungeon();
        }

        //check if dungeon is complete
        if (spawnNodes.Count == 0 && dungeonComplete == false)
        {

            if (rooms.Count < minRooms)
            {
                ResetDungeon();
            }
            else
            {
                gameStateManager.currentGameState = GameState.Playing;
                dungeonComplete = true;
                Debug.Log("Dungeon Complete" + " | " + " Rooms: " + rooms.Count + " | " + " Seed: " + seed + " | " + " Time: " + Mathf.Round(generationTimer * 100) / 100f + "s");
                generationTimer = 0;

                GetComponent<DungeonFinalizer>().InstantiateRooms();
            }
        }

        if (dungeonComplete == false)
        {
            generationTimer += Time.deltaTime;


            //reset roomspersecond every second
            roomsPerSecTimer += Time.deltaTime;
            if (roomsPerSecTimer >= 1)
            {
                Debug.Log("Rooms per second: " + roomsPerSecond);



                //estimated time remaining

                float estimatedTime = (maxRooms - rooms.Count) / roomsPerSecond;
                Debug.Log("Estimated time remaining: " + Mathf.Round(estimatedTime * 100) / 100f + "s");


                roomsPerSecond = 0;
                roomsPerSecTimer = 0;

            }
        }

    }
    float roomsPerSecTimer = 0;

    private int resetTimes = 0;
    void ResetDungeon()
    {
        /*
        if(dungeonComplete == false){
            resetTimes++;
        }
        if(resetTimes > 5){ //if dungeon isn't complete withing 5 resets, probabilties will be randomized
            resetTimes = 0;
            Debug.Log("Too many resets");
            RandomizeProbabilities();
        }
*/

        seed = Random.seed;
        calculaitonSeed = seed;

        StopAllCoroutines();
        spawnNodes.Clear();

        foreach (Transform child in transform.GetChild(0))
        {
            Destroy(child.gameObject);
        }
        rooms.Clear();
        gameStateManager.currentGameState = GameState.GeneratingDungeon;
        dungeonComplete = false;
        GenerateDungeon();
    }

    void RandomizeProbabilities()
    {
        for (int i = 0; i < probabilities.Length; i++)
        {
            probabilities[i] += Random.Range(-25, 25);
            if (probabilities[i] < 0)
            {
                probabilities[i] = 0;
            }
            if (probabilities[i] > 100)
            {
                probabilities[i] = 100;
            }
        }
    }
}
