using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : log
{
    


    // Start is called before the first frame update
   void Start () {
    timer = 4f;
        currentState = EnemyState.idle;
        target = GameObject.FindWithTag("Player").transform;
        GetComponent<Pathfinding.AIPath>().enabled = false;
        float randomNum = Random.Range(0.3f, 0.5f);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 2f+randomNum;
         spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;

        //enable pathfinding
        
	}
    // Update is called once per frame
    void Update()
    {
        if(isTimer == true){
            timer -= Time.deltaTime;
            if(timer <= 0){
                isTimer = false;
                timer = Random.Range(2f, 4f);
            }
        }
     

    }
    public override void CheckDistance()
    {
        if (Vector3.Distance(target.position,
                            transform.position) <= chaseRadius
             && Vector3.Distance(target.position,
                               transform.position) > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk
                && currentState != EnemyState.stagger && currentState != EnemyState.attack)
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
                    transform.position) <= attackRadius)
        {
            if (currentState == EnemyState.walk
                && currentState != EnemyState.stagger && isTimer==false) 
            {
                
                StartCoroutine(AttackCo());
                
            }
        }
    else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            GetComponent<Pathfinding.AIPath>().enabled = false;
            ChangeState(EnemyState.idle);
        }

    }

    public IEnumerator AttackCo()
    {
        isTimer=true;
        float randomDuration = Random.Range(0.1f, 0.3f);
        flashDuration = randomDuration;
        Flash();
        yield return new WaitForSeconds(randomDuration);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 0f;
        int LayerIgnoreRaycast = LayerMask.NameToLayer("enemy");
        gameObject.layer = LayerIgnoreRaycast;
        currentState = EnemyState.attack;
        anim.SetBool("attack", true);
        yield return new WaitForSeconds(1f);
        currentState = EnemyState.walk;
        anim.SetBool("attack", false);
        int LayerNotIgnoreRaycast = LayerMask.NameToLayer("Default");
        gameObject.layer = LayerNotIgnoreRaycast;
        float randomNum = Random.Range(0.3f, 0.5f);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 2f+randomNum;
        
    }

}