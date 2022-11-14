using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMob : log
{
    
    public bool Spawned;
    public float closeRadius;
    public SignalSender bossMobSignal;
    private bool raised;
    // Start is called before the first frame update
   void Start () {
    anim.SetBool("idle", true);
    timer = 4f;
        currentState = EnemyState.idle;
        target = GameObject.FindWithTag("Player").transform;
        GetComponent<Pathfinding.AIPath>().enabled = false;
        float randomNum = Random.Range(0.3f, 0.5f);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 3f+randomNum;
         spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        //turn off spriterenderer
        spriteRenderer.enabled = false;
    
        

        //enable pathfinding
        
	}
    // Update is called once per frame
    void Update()
    {
        if(Spawned == false){
            anim.SetBool("idle", true);
                StartCoroutine(SpawnCo());
            }
        if(health <= 0 && raised == false){
            bossMobSignal.Raise();
            raised = true;
            gameObject.SetActive(false);
        }
        if(isTimer == true){
            timer -= Time.deltaTime;
            if(timer <= 0){
                isTimer = false;
                timer = Random.Range(3.5f, 4.5f);
            }
        }
     

    }
    public override void CheckDistance()
    {
        if (Vector3.Distance(target.position,
                            transform.position) <= chaseRadius
             && Vector3.Distance(target.position,
                               transform.position) > attackRadius && Spawned!=false)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk
                && currentState != EnemyState.stagger && currentState != EnemyState.attack && Spawned!=false)
            {
                
            Vector3 temp = Vector3.MoveTowards(transform.position,
                                                         target.position,
                                                         moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                GetComponent<Pathfinding.AIPath>().enabled = true;
                ChangeState(EnemyState.walk);
            }
        }
        else if (Vector3.Distance(target.position,
                    transform.position) <= chaseRadius
                    && Vector3.Distance(target.position,
                    transform.position) <= attackRadius && Spawned!=false)
        {
             if (currentState == EnemyState.walk
                && currentState != EnemyState.stagger && isTimer==true && Spawned!=false) 
            {
                
                Vector3 temp = Vector3.MoveTowards(transform.position,
                                                         target.position,
                                                         moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);    
            }
            
            if (currentState == EnemyState.walk
                && currentState != EnemyState.stagger && isTimer==false && Spawned!=false) 
            {
                
                StartCoroutine(AttackCo());
                
            }
        }
        else if (Vector3.Distance(target.position,
                    transform.position) <= chaseRadius
                    && Vector3.Distance(target.position,
                    transform.position) <= attackRadius &&
                    Vector3.Distance(target.position,
                    transform.position) <= closeRadius && Spawned!=false)
        {
            Vector3 temp = Vector3.MoveTowards(transform.position,
                                                         target.position,
                                                         moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                GetComponent<Pathfinding.AIPath>().maxSpeed = 0.5f;
                ChangeState(EnemyState.walk);
           }

    

    }

    public IEnumerator SpawnCo()
    {
        spriteRenderer.enabled = true;
        GetComponent<Pathfinding.AIPath>().maxSpeed = 0f;
        Spawned=true;
        yield return new WaitForSeconds(1.3f);
        anim.SetBool("idle", false);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 3.2f;
                
    }
    public IEnumerator AttackCo()
    {
        isTimer=true;
        float randomDuration = Random.Range(0.1f, 0.2f);
        flashDuration = randomDuration;
        Flash();
        yield return new WaitForSeconds(randomDuration);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 0.6f;
        int LayerIgnoreRaycast = LayerMask.NameToLayer("enemy");
        gameObject.layer = LayerIgnoreRaycast;
        currentState = EnemyState.attack;
        anim.SetBool("attack", true);
        yield return new WaitForSeconds(0.5f);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 9f;
        yield return new WaitForSeconds(0.3f);
        currentState = EnemyState.walk;
        anim.SetBool("attack", false);
        int LayerNotIgnoreRaycast = LayerMask.NameToLayer("Default");
        gameObject.layer = LayerNotIgnoreRaycast;
        float randomNum = Random.Range(0.3f, 0.5f);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 3f+randomNum;
        
    }

}