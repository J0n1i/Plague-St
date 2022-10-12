using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    Transform player;
    void FixedUpdate(){
        if(player == null){
            player = GameObject.FindWithTag("Player").transform;
        }
        else{
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }

        
    }
}
