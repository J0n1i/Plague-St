using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<GameObject> spawnNodes;
    void Awake()
    {
        spawnNodes.Clear();
        for(int i = 0; i < transform.GetChild(2).childCount; i++){
            spawnNodes.Add(transform.GetChild(2).GetChild(i).gameObject);
        }
    }
}
