using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNode : MonoBehaviour
{
    public string nodeDirection;

    void Start()
    {
        RaycastHit2D[] hit;
        hit = Physics2D.BoxCastAll(transform.position, Vector2.one, 0f, Vector2.zero, 0f, LayerMask.GetMask("RoomSpawner"));

        foreach (var item in hit)
        {
            if (item.transform != null)
            {
                if (item.transform.gameObject.CompareTag("RoomSpawnCollider"))
                {
                    Debug.Log("A");
                    Destroy(gameObject);
                }

                if (item.transform.gameObject.CompareTag("SpawnNode"))
                {
                    if (item.transform.parent.parent != transform.parent.parent)
                    {
                        Debug.Log("B");
                        Destroy(item.transform.gameObject);
                    }
                }
            }
        }
    }

}
