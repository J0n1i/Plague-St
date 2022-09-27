using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    public int roomIndex;
    public enum RoomType {
        SpawnRoom,
        BossRoom,
        EnemyRoom,
        TreasureRoom
    };
    public RoomType roomType;

    public int enemyAmount;
    public int treasureAmount;
    public Transform roomLocation;



    public Room(int roomIndex, RoomType roomType, int enemyAmount, int treasureAmount, Transform roomLocation)
    {
        this.roomIndex = roomIndex;
        this.roomType = roomType;
        this.enemyAmount = enemyAmount;
        this.treasureAmount = treasureAmount;
        this.roomLocation = roomLocation;
    }
}
