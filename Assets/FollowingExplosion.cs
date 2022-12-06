using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingExplosion : MonoBehaviour
{
    public float timer;
    public Animator anim;
     public float moveSpeed = 2f;
    private Rigidbody2D rb;
    public Transform target;
    private Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
        
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
    
            moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
        
       
        timer += Time.deltaTime;
        
        if (timer < 4f)
        {
            transform.localScale += new Vector3(0.45f, 0.45f, 0f) * Time.deltaTime;
            moveSpeed += 0.4f * Time.deltaTime;
        }
        if(timer >= 5.5f){
            StartCoroutine(Explosion());
        }    
    }
    public void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player") && !other.isTrigger){    
            StartCoroutine(Explosion());
        }
    }
    IEnumerator Explosion(){
        moveSpeed = 0.1f;
        yield return new WaitForSeconds(0.25f);
        anim.SetBool("Explode", true);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);

    }
}
