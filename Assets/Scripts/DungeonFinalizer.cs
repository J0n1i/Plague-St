using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonFinalizer : MonoBehaviour
{
    [SerializeField] private GameObject[] roomPrefab;
    [SerializeField] private GameObject bossRoomPrefab, treasureRoomPrefab;

    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject[] enemyElitePrefab;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private GameObject treasurePrefab;
    [SerializeField] private GameObject tutorialPrefab;



    private List<GameObject> generationRooms, actualRooms;

    [SerializeField] private List<Room> rooms;

    private DungeonGenerator dungeonGenerator;

    private AstarPath pathfinder;

    public List<GameObject> enemies;

    static public Transform bossSpawnPoint;
    void Start()
    {
        dungeonGenerator = GetComponent<DungeonGenerator>();
        pathfinder = GameObject.Find("a*").GetComponent<AstarPath>();
    }

    public void InstantiateRooms()
    {
        rooms = new List<Room>();
        generationRooms = dungeonGenerator.rooms;

        for (int i = 0; i < generationRooms.Count; i++)
        {
            Room newRoom = new Room(i);
            newRoom.roomLocation = generationRooms[i].transform;
            rooms.Add(newRoom);
        }

        rooms[0].roomType = RoomType.SpawnRoom;
        AssignBossRoom();
        AssignEnemyRooms();
        AssignTreasureRooms();

        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].roomType == RoomType.SpawnRoom)
            {
                rooms[i].roomPrefab = roomPrefab[Random.Range(0, roomPrefab.Length)];
            }
            else if (rooms[i].roomType == RoomType.BossRoom)
            {
                rooms[i].roomPrefab = bossRoomPrefab;
            }
            else if (rooms[i].roomType == RoomType.EnemyRoom)
            {
                rooms[i].roomPrefab = roomPrefab[Random.Range(0, roomPrefab.Length)];
                rooms[i].enemyAmount = Random.Range(8, 15);
            }
            else if (rooms[i].roomType == RoomType.TreasureRoom)
            {
                rooms[i].roomPrefab = treasureRoomPrefab;
                rooms[i].treasureAmount = Random.Range(1, 4);
            }
        }

        for (int i = 0; i < rooms.Count; i++)
        {
            GameObject newRoomPrefab = Instantiate(rooms[i].roomPrefab, rooms[i].roomLocation.position, Quaternion.identity, transform.GetChild(1));
            rooms[i].roomPrefab = newRoomPrefab;
        }

        //SpawnEnemies();
        StartCoroutine(LateSpawnEnemies());
        SpawnChests();
        SpawnTutorial();
        
        StartCoroutine(DisableCreationRooms());
    }

    IEnumerator DisableCreationRooms()
    {
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.SetActive(false);

    }
    private void AssignBossRoom()
    {
        //get furthest room from spawnroom and assign it as bossroom
        int furthestRoom = 0;
        float furthestDistance = 0;
        for (int i = 0; i < rooms.Count; i++)
        {
            if (Vector3.Distance(rooms[0].roomLocation.position, rooms[i].roomLocation.position) > furthestDistance)
            {
                furthestDistance = Vector3.Distance(rooms[0].roomLocation.position, rooms[i].roomLocation.position);
                furthestRoom = i;
            }
        }
        rooms[furthestRoom].roomType = RoomType.BossRoom;
    }

    private void AssignEnemyRooms()
    {
        //get half of unassigned rooms and assign as enemy rooms
        for (int i = 0; i < (rooms.Count - 2) * 0.7f; i++)
        {
            int randomRoom = Random.Range(0, rooms.Count);
            if (rooms[randomRoom].roomType == RoomType.Unassigned)
            {
                rooms[randomRoom].roomType = RoomType.EnemyRoom;
            }
            else
            {
                i--;
            }
        }
    }

    private void AssignTreasureRooms()
    {
        //assign all unassigned rooms as treasure rooms
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].roomType == RoomType.Unassigned)
            {
                rooms[i].roomType = RoomType.TreasureRoom;
            }
        }
    }


    IEnumerator LateSpawnEnemies()
    {
        yield return new WaitForSeconds(0.1f);
        SpawnEnemies();
        SpawnBoss();
        pathfinder.Scan();
    }
    private void SpawnEnemies()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].roomType == RoomType.EnemyRoom)
            {
                List<Transform> spawnPoints = new List<Transform>();
                foreach (Transform child in rooms[i].roomPrefab.transform.Find("Enemies"))
                {
                    spawnPoints.Add(child);
                }

                for (int j = 0; j < rooms[i].enemyAmount; j++)
                {
                    Vector3 randomOffset = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);
                    Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                    //spawn atleast one enemy to every spawnpoint
                    if (j < spawnPoints.Count)
                    {
                        randomSpawnPoint = spawnPoints[j];
                    }
                    GameObject newEnemy = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], randomSpawnPoint.position + randomOffset, Quaternion.identity, rooms[i].roomPrefab.transform.Find("Enemies"));
                    enemies.Add(newEnemy);
                    //spawn elite enemy
                    int dice = Random.Range(1, 101);
                    if (j == 0 && dice <=25)
                    {
                        newEnemy = Instantiate(enemyElitePrefab[Random.Range(0, enemyElitePrefab.Length)], randomSpawnPoint.position + randomOffset, Quaternion.identity, rooms[i].roomPrefab.transform.Find("Enemies"));
                        enemies.Add(newEnemy);
                    }
                }
            }
        }
    }

    private void SpawnChests()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].roomType == RoomType.TreasureRoom)
            {
                        Instantiate(treasurePrefab, rooms[i].roomPrefab.transform.position + new Vector3(0, 1, 0), Quaternion.identity, rooms[i].roomPrefab.transform);
                        Instantiate(treasurePrefab, rooms[i].roomPrefab.transform.position + new Vector3(1, -1, 0), Quaternion.identity, rooms[i].roomPrefab.transform);
                        Instantiate(treasurePrefab, rooms[i].roomPrefab.transform.position + new Vector3(-1, -1, 0), Quaternion.identity, rooms[i].roomPrefab.transform);

                }
            }
        }
    private void SpawnTutorial()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].roomType == RoomType.SpawnRoom)
            {
                if(rooms[i].roomPrefab.transform.Find("Tutorial") != null)
                {
                    rooms[i].roomPrefab.transform.Find("Tutorial").gameObject.SetActive(true);
                }

            }
        }
    
    }
    private void SpawnBoss(){
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].roomType == RoomType.BossRoom)
            {
                
               GameObject newEnemy = Instantiate(bossPrefab, rooms[i].roomPrefab.transform.position, Quaternion.identity, rooms[i].roomPrefab.transform.Find("Enemies"));
                enemies.Add(newEnemy);
                //get spawnlocation of boss
                bossSpawnPoint = rooms[i].roomPrefab.transform.Find("Enemies").GetChild(0);
            }
        }
    }

}
