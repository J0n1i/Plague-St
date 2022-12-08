using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitLevel2 : MonoBehaviour
{
    void Awake()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().Initialize();
    }
    // Start is called before the first frame update
    void Start()
    {
        //GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
