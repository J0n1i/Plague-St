using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : log
{
    public bool isTimer;
    public bool isSpin;
    public bool isFire;
    public bool isAttacked;
    public bool logiSpawned;
    public bool enraged;
    public float timer;
    public float fireTimer;
    public float Attacktimer;
    public Slider healthBar;
    public float closeRadius;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject logi;
    Vector3 pos;
    Vector3 pos1;
    
    
   
    // Start is called before the first frame update
   void Start () {
        currentState = EnemyState.idle;
        target = GameObject.FindWithTag("Player").transform;
        GetComponent<Pathfinding.AIPath>().enabled = false;
        timer = 5f;
        //enable pathfinding
        fireTimer = 5f;
        Attacktimer = 2f;
	}
    // Update is called once per frame
    void Update()
    {
        healthBar.value = health;
        if(health <= 5)
        {
            if(logiSpawned == false && currentState != EnemyState.stagger && currentState != EnemyState.attack){
            StartCoroutine(SpawnLogi());
            }
        }
        if(isTimer == true){
            timer -= Time.deltaTime;
            if(timer <= 0){
                isTimer = false;
                timer = 5f;
                if(enraged == true){
                    timer= 4f;
                }
            }
        }
        if(isSpin == true){
            timer -= Time.deltaTime;
            if(timer <= 0){
                isSpin = false;
                timer = 5f;
                if(enraged == true){
                    timer= 4f;
                }
            }
        }
        if(isFire == true){
            fireTimer -= Time.deltaTime;
            if(fireTimer <= 0){
                isFire = false;
                fireTimer = 5f;
                if(enraged == true){
                    fireTimer= 3f;
                }
            }
        }
        if(isAttacked ==true){
            Attacktimer -= Time.deltaTime;
            if(Attacktimer <= 0){
                isAttacked = false;
                Attacktimer = 2f;
                if(enraged == true){
                    Attacktimer= 2f;
                }
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
            if (currentState == EnemyState.walk
                && currentState != EnemyState.stagger && isFire==false && isAttacked==false)
            {
                StartCoroutine(ShootCo());
                
                
            }
            else if (currentState == EnemyState.idle || currentState == EnemyState.walk
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
                    transform.position) <= attackRadius &&
                    Vector3.Distance(target.position,
                    transform.position) > closeRadius
                    )
        {
            if (currentState == EnemyState.walk
                && currentState != EnemyState.stagger && isTimer==false && isAttacked==false)
            {
                
                StartCoroutine(AttackCo());
                
            }
            else if (currentState == EnemyState.idle || currentState == EnemyState.walk
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
                    transform.position) <= attackRadius &&
                    Vector3.Distance(target.position,
                    transform.position) <= closeRadius)
        {   
            if (currentState == EnemyState.walk
                && currentState != EnemyState.stagger && isSpin == false && isAttacked==false)
            {
                StartCoroutine(SpinCo());
                
            }
            else if (currentState == EnemyState.idle || currentState == EnemyState.walk
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
                            transform.position) > chaseRadius)
        {
            GetComponent<Pathfinding.AIPath>().enabled = false;
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
        isAttacked=true;
        GetComponent<Pathfinding.AIPath>().maxSpeed = 0f;
        if(enraged == true){
            GetComponent<Pathfinding.AIPath>().maxSpeed = 1f;
        }
        currentState = EnemyState.attack;
        anim.SetBool("attack", true);
        yield return new WaitForSeconds(1f);
        currentState = EnemyState.walk;
        anim.SetBool("attack", false);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 3f;
        if(enraged == true){
            GetComponent<Pathfinding.AIPath>().maxSpeed = 3.5f;
        }
    }
    public IEnumerator SpinCo()
    {
        isSpin=true;
        isAttacked=true;
        
        GetComponent<Pathfinding.AIPath>().maxSpeed = 0f;
        if(enraged == true){
            GetComponent<Pathfinding.AIPath>().maxSpeed = 1f;
        }
        currentState = EnemyState.attack;
        anim.SetBool("spin", true);
        yield return new WaitForSeconds(1f);
        currentState = EnemyState.walk;
        anim.SetBool("spin", false);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 3f;
         if(enraged == true){
            GetComponent<Pathfinding.AIPath>().maxSpeed = 3.5f;
        }
        
    }
    public IEnumerator ShootCo()
    {
        isFire=true;
        isAttacked=true;
        GetComponent<Pathfinding.AIPath>().maxSpeed = 2f;
        yield return new WaitForSeconds(0.5f);
        Instantiate(bullet, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 3f;
        if(enraged==true){
            GetComponent<Pathfinding.AIPath>().maxSpeed = 3.5f;
        }

    }
   
    public IEnumerator SpawnLogi()
    {
        logiSpawned = true;
        anim.SetBool("enrage", true);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 0f;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        currentState = EnemyState.attack;
        pos = new Vector3(transform.position.x + Random.Range(-2,2), transform.position.y + Random.Range(-2,2), transform.position.z);
        pos1 = new Vector3(transform.position.x + Random.Range(-2,2), transform.position.y + Random.Range(-2,2), transform.position.z);
        yield return new WaitForSeconds(0.5f);
        Instantiate(logi, pos, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(logi, pos1, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        currentState = EnemyState.walk;
        anim.SetBool("enrage", false);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 3.5f;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        enraged=true;
    }
}