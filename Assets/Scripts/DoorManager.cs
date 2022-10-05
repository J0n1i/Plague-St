using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool doorOpen = false;
    void Start()
    {
        RaycastHit2D[] hit;
        hit = Physics2D.BoxCastAll(transform.position, Vector2.one, 0f, Vector2.zero, 0f, LayerMask.GetMask("RoomDoorTester"));


        if (hit.Length == 4)
        {
            doorOpen = true;
            transform.parent.parent.Find("Walls").GetChild(transform.GetSiblingIndex()).gameObject.SetActive(false);
        }

        

    }

    // Update is called once per frame
    void Update()
    {

    }
}


