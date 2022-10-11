using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : log
{
    // Start is called before the first frame update
   void Start () {
        currentState = EnemyState.idle;
        target = GameObject.FindWithTag("Player").transform;
        GetComponent<Pathfinding.AIPath>().enabled = false;
        //enable pathfinding

	}
    // Update is called once per frame
    void Update()
    {
        
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
                && currentState != EnemyState.stagger)
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
        GetComponent<Pathfinding.AIPath>().enabled = false;
        currentState = EnemyState.attack;
        anim.SetBool("attack", true);
        yield return new WaitForSeconds(1f);
        currentState = EnemyState.walk;
        anim.SetBool("attack", false);
        GetComponent<Pathfinding.AIPath>().enabled = true;
    }

}
