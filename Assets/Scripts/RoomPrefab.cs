using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPrefab : MonoBehaviour
{
    public List<GameObject> spawnNodes;
    void Start()
    {
        spawnNodes.Clear();
        for(int i = 0; i < transform.GetChild(2).childCount; i++){
            spawnNodes.Add(transform.GetChild(2).GetChild(i).gameObject);
        }
    }
}
