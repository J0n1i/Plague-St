using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Unassigned,
    SpawnRoom,
    BossRoom,
    EnemyRoom,
    TreasureRoom
};
[System.Serializable]

public class Room
{
    public int roomIndex;

    public RoomType roomType;

    public int enemyAmount;
    public int treasureAmount;
    public Transform roomLocation;
    public GameObject roomPrefab;



    public Room(int roomIndex, RoomType roomType, int enemyAmount, int treasureAmount, Transform roomLocation, GameObject roomPrefab)
    {
        this.roomIndex = roomIndex;
        this.roomType = roomType;
        this.enemyAmount = enemyAmount;
        this.treasureAmount = treasureAmount;
        this.roomLocation = roomLocation;
        this.roomPrefab = roomPrefab;
    }

    public Room(int roomIndex)
    {
        this.roomIndex = roomIndex;
    }
}
