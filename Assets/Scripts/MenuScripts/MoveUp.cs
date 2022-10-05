using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour
{


    private Rigidbody2D rb;
    private float moveSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = 5f;

    }

    public void Move()
    {
        for (int i = 0; i < 30; i++)
        {
            rb.velocity = Vector2.up * moveSpeed;
        }
    }


}
