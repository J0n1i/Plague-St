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
    }

    IEnumerator InstantiateEnemies()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].roomType == Room.RoomType.EnemyRoom)
            {
                //instantiate random amount(5,10) of enemies in random positions in enemyRooms
                int randomEnemyAmount = Random.Range(5, 10);
                for (int j = 0; j < randomEnemyAmount; j++)
                {
                    int randomEnemy = Random.Range(0, enemyPrefabs.Length);
                    int randomX = Random.Range(-15, 15);
                    int randomY = Random.Range(-8, 6);
                    Instantiate(enemyPrefabs[randomEnemy], rooms[i].roomLocation.position + new Vector3(randomX, randomY, 0), Quaternion.identity, rooms[i].roomPrefab.transform.Find("Enemies"));
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
            int randomPotAmount = Random.Range(5, 10);
            for (int j = 0; j < randomPotAmount; j++)
            {
                int randomPot = Random.Range(0, potPrefabs.Length);
                int randomX = Random.Range(-15, 15);
                int randomY = Random.Range(-8, 6);
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