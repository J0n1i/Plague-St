using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public float AttractorSpeed;
    public bool playerInRange;
    private GameObject player;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInRange)
        {
            StartCoroutine(waitCoroutine());

        }

    }
    private IEnumerator waitCoroutine(){
        yield return new WaitForSeconds(0.5f);
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, AttractorSpeed * Time.deltaTime);
            if(transform.childCount < 1){
               Destroy(gameObject);
            }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerInRange = true;
            player = GameObject.FindWithTag("Player");
			target = player.transform;
        }
    }
}
