using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy1 : log
{
    private bool isSpin;
    private bool isFire;
    private bool isAttacked;
    private bool isDashed;
    private bool logiSpawned;
    public bool enraged;
    private float fireTimer;
    private float Attacktimer;
    public Slider healthBar;
    public float closeRadius;
    private bool canShootTwice;
    public bool bossActive = false;
    public GameObject dashEffect;
    public GameObject bossAura;
    public GameObject miniexplosion;
    public GameObject FollowingExplosion;
private List<GameObject> enemies;
    [SerializeField] GameObject logi;
    Vector3 pos;
    Vector3 pos1;
    public float shootRadius;
    Vector3 posBoss;
    //box collider
    public BoxCollider2D boxCollider;
    //capsule collider
    public CapsuleCollider2D capsuleCollider;
    private int bossMobsDead;  
    private float DashTimer;  
    private bool spawningEnemies;
    public SignalSender playerEnterRoom;
    
   
    // Start is called before the first frame update
   void Start () {
     GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    //enemies list from dungeon finalizer
    enemies = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonFinalizer>().enemies;
    int LayerIgnoreRaycast = LayerMask.NameToLayer("enemy");
        gameObject.layer = LayerIgnoreRaycast;
        currentState = EnemyState.idle;
        target = GameObject.FindWithTag("Player").transform;
        GetComponent<Pathfinding.AIPath>().enabled = false;
        timer = 5f;
        //enable pathfinding
        fireTimer = 5f;
        Attacktimer = 2f;
        shootRadius = 6f;
        //get location of boss
        posBoss = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        bossMobsDead = 0;
        logiSpawned = false;
	}
    // Update is called once per frame
    void Update()
    {
        
        if(currentState!=EnemyState.stagger && currentState!=EnemyState.attack){
             Vector3 temp = Vector3.MoveTowards(transform.position,
                                                         target.position,
                                                         moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
        }
        DashTimer -= Time.deltaTime;
        healthBar.value = health;
        
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
                    fireTimer= 4.5f;
                }
            }
        }
        if(isAttacked ==true){
            Attacktimer -= Time.deltaTime;
            if(Attacktimer <= 0){
                isAttacked = false;
                Attacktimer = 3f;
                if(enraged == true){
                    Attacktimer= 2.5f;
                }
            }
        }
        if(isDashed ==true && enraged == true){
        DashTimer -= Time.deltaTime;
            if(DashTimer <= 0){
                isDashed = false;
                DashTimer = 7.5f;
            }
        }

    }
    public IEnumerator Dash(){
                isDashed = true;
                currentState = EnemyState.attack;
                GetComponent<Pathfinding.AIPath>().maxSpeed = 10f;
                dashEffect.SetActive(true);
                yield return new WaitForSeconds(0.25f);
                dashEffect.SetActive(false);
                GetComponent<Pathfinding.AIPath>().maxSpeed = 3.5f;
                currentState = EnemyState.walk;
                DashTimer = 7f;
    }
   
    public override void CheckDistance()
    {
        if (Vector3.Distance(target.position,transform.position) <= chaseRadius && Vector3.Distance(target.position,transform.position) > attackRadius)
        {
            if(logiSpawned == false){
            StartCoroutine(SpawnLogi());
        }
            //Bossimusiikki alkaa
            playerEnterRoom.Raise();
               
            //FindObjectOfType<LevelMusic>().BossMusic();
            if (currentState == EnemyState.walk
                && currentState != EnemyState.stagger && isFire==false && isAttacked==false && Vector3.Distance(target.position,
                               transform.position) > shootRadius && currentState != EnemyState.attack && spawningEnemies == false)
            {
                int random = Random.Range(1, 4);
                if(random == 1){
                    StartCoroutine(ShootCo());
                }
                else if(random == 2){
                    StartCoroutine(SpinCo());
                } else if (random == 3){
                    StartCoroutine(AttackCo());
                }
                
                
            }
            else if (currentState == EnemyState.idle || currentState == EnemyState.walk
                && currentState != EnemyState.stagger && currentState != EnemyState.attack)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position,
                                                         target.position,
                                                         moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                GetComponent<Pathfinding.AIPath>().enabled = false;
                ChangeState(EnemyState.walk);
                //if(enraged == true && isDashed == false){
                //    StartCoroutine(Dash());
                //}
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
            if(logiSpawned == false){
            StartCoroutine(SpawnLogi());
        }
            if (currentState == EnemyState.walk
                && currentState != EnemyState.stagger && isTimer==false && isAttacked==false && currentState != EnemyState.attack && spawningEnemies == false)
            {
                 int random = Random.Range(1, 4);
                if(random == 1){
                    StartCoroutine(ShootCo());
                }
                else if(random == 2){
                    StartCoroutine(SpinCo());
                } else if (random == 3){
                    StartCoroutine(AttackCo());
                }
                
                
            }
            else if (currentState == EnemyState.idle || currentState == EnemyState.walk
                && currentState != EnemyState.stagger && currentState != EnemyState.attack)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position,
                                                         target.position,
                                                         moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                GetComponent<Pathfinding.AIPath>().enabled = false;
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
                && currentState != EnemyState.stagger && isSpin == false && isAttacked==false && currentState != EnemyState.attack)
            {
                 int random = Random.Range(1, 4);
                if(random == 1){
                    StartCoroutine(ShootCo());
                }
                else if(random == 2){
                    StartCoroutine(SpinCo());
                } else if (random == 3){
                    StartCoroutine(AttackCo());
                }
                
                
            }
            else if (currentState == EnemyState.idle || currentState == EnemyState.walk
                && currentState != EnemyState.stagger && currentState != EnemyState.attack)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position,
                                                         target.position,
                                                         moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                GetComponent<Pathfinding.AIPath>().enabled = false;
                ChangeState(EnemyState.walk);
                

            }
        }
    }
    public void bossMobDead(){
        bossMobsDead++;
        Boss1TakeDamage();
        if(bossMobsDead % 2 == 0 && health > 0){
            StartCoroutine(SpawnLogi());
        
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
        Instantiate(FollowingExplosion, transform.position, Quaternion.identity);
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
        Vector3 explosiontarget = new Vector3(target.position.x+Random.Range(-0.5f,0.5f),target.position.y+Random.Range(-0.5f,0.5f),target.position.z);
         yield return new WaitForSeconds(0.01f);
        Instantiate(miniexplosion, explosiontarget , Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Vector3 explosiontarget1 = new Vector3(target.position.x+Random.Range(-0.5f,0.5f),target.position.y+Random.Range(-0.5f,0.5f),target.position.z);
        yield return new WaitForSeconds(0.01f);
        Instantiate(miniexplosion, explosiontarget1 , Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Vector3 explosiontarget2 = new Vector3(target.position.x+Random.Range(-0.5f,0.5f),target.position.y+Random.Range(-0.5f,0.5f),target.position.z);
        yield return new WaitForSeconds(0.01f);
        Instantiate(miniexplosion, explosiontarget2 , Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Vector3 explosiontarget3 = new Vector3(target.position.x+Random.Range(-0.5f,0.5f),target.position.y+Random.Range(-0.5f,0.5f),target.position.z);
        yield return new WaitForSeconds(0.01f);
        Instantiate(miniexplosion, explosiontarget3 , Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        
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
        currentState = EnemyState.attack;
        GetComponent<Pathfinding.AIPath>().maxSpeed = 0f;
        anim.SetBool("shoot", true);
        //AudioPlayer.instance.PlaySound(fireBall, 1f); 
        yield return new WaitForSeconds(0.5f);
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Instantiate(bullet, transform.position, rotation);
        anim.SetBool("shoot", false);        
        yield return new WaitForSeconds(0.2f);  
        Vector3 temp = Vector3.MoveTowards(transform.position,
                                                         target.position,
                                                         moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
        anim.SetBool("shoot", true);
        //AudioPlayer.instance.PlaySound(fireBall, 1f);
        yield return new WaitForSeconds(0.5f); 
        Vector3 dir1 = target.position - transform.position;
        float angle1 = Mathf.Atan2(dir1.y, dir1.x) * Mathf.Rad2Deg;
        Quaternion rotation1 = Quaternion.AngleAxis(angle1, Vector3.forward);
        Instantiate(bullet, transform.position, rotation1);
        anim.SetBool("shoot", false);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 3f;
        currentState = EnemyState.walk;
    }
   
    public IEnumerator SpawnLogi()
    {
        logiSpawned = true;        
        anim.SetBool("spin", true);
        spawningEnemies = true;
        
        currentState = EnemyState.attack;
        yield return new WaitForSeconds(0.25f);
        pos = new Vector3(transform.position.x + Random.Range(-2,2), transform.position.y + Random.Range(-2,2), transform.position.z);
        pos1 = new Vector3(transform.position.x + Random.Range(-2,2), transform.position.y + Random.Range(-2,2), transform.position.z);
        yield return new WaitForSeconds(1f);
        GameObject logi1 = Instantiate(logi, pos, Quaternion.identity);
        logi1.transform.parent = transform;
        enemies.Add(logi1);
        yield return new WaitForSeconds(3f);
        GameObject logi2 = Instantiate(logi, pos1, Quaternion.identity);
        logi2.transform.parent = transform;
        enemies.Add(logi2);
        yield return new WaitForSeconds(0.5f);
        currentState = EnemyState.walk;
        anim.SetBool("spin", false);
        spawningEnemies = false;
        
    }
}