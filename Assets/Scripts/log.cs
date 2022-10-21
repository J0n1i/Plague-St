using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class log : Enemy {

    private Rigidbody2D myRigidbody;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    public Animator anim;
    public float timer;
    public bool isTimer;
    

	// Use this for initialization
	void Start () {
        timer = 4f;
        currentState = EnemyState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        GetComponent<Pathfinding.AIPath>().enabled = false;
        float randomNum = Random.Range(0.1f, 0.3f);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 3f+randomNum;
        //enable pathfinding

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        CheckDistance();
        if(isTimer == true){
            timer -= Time.deltaTime;
            if(timer <= 0){
                isTimer = false;
                timer = 4f;

            }
        }
       
	}

   public virtual void CheckDistance()
    {
        if(Vector3.Distance(target.position, 
                            transform.position) <= chaseRadius
           && Vector3.Distance(target.position,
                               transform.position) > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk
                && currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position,
                                                         target.position,
                                                         moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                //myRigidbody.MovePosition(temp);
                GetComponent<Pathfinding.AIPath>().enabled = true;
                ChangeState(EnemyState.walk);
                
                anim.SetBool("wakeUp", true);
            }
        }else if (Vector3.Distance(target.position,
                    transform.position) <= chaseRadius
                    && Vector3.Distance(target.position,
                    transform.position) <= attackRadius)
        {
            if (currentState == EnemyState.idle && currentState != EnemyState.stagger && isTimer==false|| currentState == EnemyState.walk
                && currentState != EnemyState.stagger && isTimer==false)
            {
                
                StartCoroutine(AttackCo());
                
            }
        }else if (Vector3.Distance(target.position,
                            transform.position) > chaseRadius)
        {
            GetComponent<Pathfinding.AIPath>().enabled = false;
            anim.SetBool("wakeUp", false);
        }
    }

    public void SetAnimFloat(Vector2 setVector){
        anim.SetFloat("moveX", setVector.x);
        anim.SetFloat("moveY", setVector.y);
    }

    public void changeAnim(Vector2 direction){
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if(direction.x > 0){
                SetAnimFloat(Vector2.right);
            }else if (direction.x < 0)
            {
                SetAnimFloat(Vector2.left);
            }
        }else if(Mathf.Abs(direction.x) < Mathf.Abs(direction.y)){
            if(direction.y > 0)
            {
                SetAnimFloat(Vector2.up);
            }
            else if (direction.y < 0)
            {
                SetAnimFloat(Vector2.down);
            }
        }
    }

    public void ChangeState(EnemyState newState){
        if(currentState != newState)
        {
            currentState = newState;
        }
    }
     public IEnumerator AttackCo()
    {
        
        isTimer=true;
        GetComponent<Pathfinding.AIPath>().maxSpeed = 0f;
        int LayerIgnoreRaycast = LayerMask.NameToLayer("enemy");
        gameObject.layer = LayerIgnoreRaycast;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        currentState = EnemyState.attack;
        anim.SetBool("attack", true);
        yield return new WaitForSeconds(1f);
        currentState = EnemyState.walk;
        anim.SetBool("attack", false);
        int LayerNotIgnoreRaycast = LayerMask.NameToLayer("Default");
        gameObject.layer = LayerNotIgnoreRaycast;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        float randomNum = Random.Range(0.1f, 0.3f);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 3f+randomNum;
       
    
    }
}