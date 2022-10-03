using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonFinalizer : MonoBehaviour
{
    [SerializeField] private GameObject roomPrefab;

    List<GameObject> rooms;

    private DungeonGenerator dungeonGenerator;
    private LevelManager levelManager;

    void Start()
    {
        dungeonGenerator = GetComponent<DungeonGenerator>();
        levelManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LevelManager>();
    }

    public void InstantiateRooms()
    {
        rooms = dungeonGenerator.rooms;
        foreach (GameObject room in rooms)
        {
            var newRoom = Instantiate(roomPrefab, room.transform.position, Quaternion.identity, transform.GetChild(1));
            levelManager.roomPrefabs.Add(newRoom);
        }



        levelManager.CreateList();





        StartCoroutine(DisableGenerationRooms());
    }

    IEnumerator DisableGenerationRooms()
    {
        yield return new WaitForEndOfFrame();
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
