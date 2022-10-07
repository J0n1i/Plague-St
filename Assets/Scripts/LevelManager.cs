using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> roomPrefabs;
    [SerializeField] public List<Room> rooms;
    private DungeonGenerator dungeonGenerator;
    [SerializeField] private Transform enemiesParent, furnitureParent, potsParent, chestsParent;

    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] furniturePrefabs;
    [SerializeField] private GameObject[] potPrefabs;
    [SerializeField] private GameObject[] chestPrefabs;

    void Start()
    {
        dungeonGenerator = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DungeonGenerator>();
    }

    public void CreateList()
    {
        for (int i = 0; i < roomPrefabs.Count; i++)
        {
            var newRoom = new Room(i, Room.RoomType.EnemyRoom, 0, 0, roomPrefabs[i].transform, roomPrefabs[i]);
            rooms.Add(newRoom);
        }


        //select 20% of roooms to be treasure rooms
        for (int i = 0; i < rooms.Count / 5; i++)
        {
            int randomRoom = Random.Range(0, rooms.Count);
            rooms[randomRoom].roomType = Room.RoomType.TreasureRoom;
        }

        rooms[0].roomType = Room.RoomType.SpawnRoom;


        //get room furthest from spawn
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
        rooms[furthestRoom].roomType = Room.RoomType.BossRoom;
        StartCoroutine(InstantiateChests());
        StartCoroutine(InstantiateEnemies());
        StartCoroutine(InstantiateFurniture());
        StartCoroutine(InstantiatePots());
        StartCoroutine(InstantiateAstar());
        AstarPath.active.Scan();
    }
    //instantiate astar
    IEnumerator InstantiateAstar()
    {
        yield return new WaitForSeconds(2f);
       AstarPath.active.Scan();
    }

    IEnumerator InstantiateEnemies()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].roomType == Room.RoomType.EnemyRoom)
            {
                //Get enemy spawnpoints and put into list
                List<Transform> enemySpawnPoints = new List<Transform>();
                foreach (Transform child in rooms[i].roomPrefab.transform.Find("Enemies").transform)
                {
                    enemySpawnPoints.Add(child);
                }

                //instantiate 5 enemies for each spawnpoint

                for (int j = 0; j < enemySpawnPoints.Count; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        //add random offset to enemy spawnpoint (-2, 2)
                        Vector3 randomOffset = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2));
                        var newEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], enemySpawnPoints[j].position + randomOffset, Quaternion.identity, enemySpawnPoints[j]);
                    }
                }
            }
        }
    }


    IEnumerator InstantiateFurniture()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].roomType == Room.RoomType.EnemyRoom)
            {
                int randomFurnitureAmount = 1;
                for (int j = 0; j < randomFurnitureAmount; j++)
                {
                    int randomFurniture = Random.Range(0, furniturePrefabs.Length);
                    Instantiate(furniturePrefabs[randomFurniture], rooms[i].roomLocation.position, Quaternion.identity, furnitureParent);
                }

            }
        }
    }
    IEnumerator InstantiatePots()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < rooms.Count; i++)
        {
            int randomPotAmount = Random.Range(1, 3);
            for (int j = 0; j < randomPotAmount; j++)
            {
                int randomPot = Random.Range(0, potPrefabs.Length);
                float randomX = Random.Range(-15, 15);
                float randomY = Random.Range(-8, 6);
                randomX += 0.5f;
                randomY += 0.5f;
                Instantiate(potPrefabs[randomPot], rooms[i].roomLocation.position + new Vector3(randomX, randomY, 0), Quaternion.identity, potsParent);
            }
        }
    }
    IEnumerator InstantiateChests()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].roomType == Room.RoomType.TreasureRoom)
            {
                int randomChestAmount = Random.Range(1, 4);
                if(randomChestAmount == 1){
                    //instantiate chest in middle of room
                    Instantiate(chestPrefabs[0], rooms[i].roomLocation.position, Quaternion.identity, chestsParent);
                }else if(randomChestAmount == 2){
                    //instantiate two chests next to each other in the middle of the room
                    Instantiate(chestPrefabs[0], rooms[i].roomLocation.position + new Vector3(-1, 0, 0), Quaternion.identity, chestsParent);
                    Instantiate(chestPrefabs[0], rooms[i].roomLocation.position + new Vector3(1, 0, 0), Quaternion.identity, chestsParent);
                }else{
                    //instantiate three chests next to each other in the middle of the room
                    Instantiate(chestPrefabs[0], rooms[i].roomLocation.position + new Vector3(-2, 0, 0), Quaternion.identity, chestsParent);
                    Instantiate(chestPrefabs[0], rooms[i].roomLocation.position + new Vector3(2, 0, 0), Quaternion.identity, chestsParent);
                    Instantiate(chestPrefabs[0], rooms[i].roomLocation.position + new Vector3(0, 0, 0), Quaternion.identity, chestsParent);
                }
            }
        }
    }
}