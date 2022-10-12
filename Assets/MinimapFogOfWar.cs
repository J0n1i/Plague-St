using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFogOfWar : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            spriteRenderer.enabled = true;
        }
    }
}
