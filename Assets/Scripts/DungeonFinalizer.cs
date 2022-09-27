using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonFinalizer : MonoBehaviour
{
    [SerializeField] private GameObject roomPrefab;

    List<GameObject> rooms;

    private DungeonGenerator dungeonGenerator;
    private LevelManager levelManager;

    void Start(){
        dungeonGenerator = GetComponent<DungeonGenerator>();
        levelManager = GetComponent<LevelManager>();
    }

    public void InstantiateRooms()
    {
        rooms = dungeonGenerator.rooms;
        foreach (GameObject room in rooms)
        {
            Instantiate(roomPrefab, room.transform.position, Quaternion.identity, transform.GetChild(1));
        }
        StartCoroutine(DisableGenerationRooms());
    }

    IEnumerator DisableGenerationRooms()
    {
        yield return new WaitForEndOfFrame();;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
