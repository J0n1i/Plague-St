using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : log
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
    //enemies list from dungeon finalizer
    enemies = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonFinalizer>().enemies;

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
	}
    // Update is called once per frame
    void Update()
    {
        DashTimer -= Time.deltaTime;
        healthBar.value = health;
        if(health <= 7)
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
            //Bossimusiikki alkaa
            playerEnterRoom.Raise();
            //FindObjectOfType<LevelMusic>().BossMusic();
            if (currentState == EnemyState.walk
                && currentState != EnemyState.stagger && isFire==false && isAttacked==false && Vector3.Distance(target.position,
                               transform.position) > shootRadius && currentState != EnemyState.attack && spawningEnemies == false)
            {
                canShootTwice = true;
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
                if(enraged == true && isDashed == false){
                    StartCoroutine(Dash());
                }
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
                && currentState != EnemyState.stagger && isTimer==false && isAttacked==false && currentState != EnemyState.attack && spawningEnemies == false)
            {
                int random = Random.Range(0, 2);
                if(random == 0){
                    StartCoroutine(ShootCo());
                }
                else if(random == 1){
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
                && currentState != EnemyState.stagger && isSpin == false && isAttacked==false && currentState != EnemyState.attack)
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
    }
    public void bossMobDead(){
        bossMobsDead++;
        if(bossMobsDead == 2){
            StartCoroutine(WakeUp());
        
        }
    }
    public IEnumerator WakeUp(){
        yield return new WaitForSeconds(0.5f);
         int LayerNotIgnoreRaycast = LayerMask.NameToLayer("Default");
        gameObject.layer = LayerNotIgnoreRaycast;
        boxCollider.enabled = true;
        capsuleCollider.enabled = true;
        currentState = EnemyState.walk;
        anim.SetBool("enrage", false);
        spawningEnemies = false;
        GetComponent<Pathfinding.AIPath>().maxSpeed = 3.5f;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        bossAura.SetActive(true);
        enraged=true;

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
        currentState = EnemyState.attack;
        GetComponent<Pathfinding.AIPath>().maxSpeed = 0f;
        anim.SetBool("shoot", true); 
        yield return new WaitForSeconds(0.5f);
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Instantiate(bullet, transform.position, rotation);
        anim.SetBool("shoot", false);        
        int random = Random.Range(1, 3);
        if(random == 1 && canShootTwice == true || enraged==true && canShootTwice == true){
        yield return new WaitForSeconds(0.2f); 
        anim.SetBool("shoot", true);
        yield return new WaitForSeconds(0.5f); 
            Vector3 dir1 = target.position - transform.position;
        float angle1 = Mathf.Atan2(dir1.y, dir1.x) * Mathf.Rad2Deg;
        Quaternion rotation1 = Quaternion.AngleAxis(angle1, Vector3.forward);
        Instantiate(bullet, transform.position, rotation1);
        
        anim.SetBool("shoot", false);
        }
        else{
        }

        yield return new WaitForSeconds(0.5f);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 3f;
        currentState = EnemyState.walk;
        if(enraged==true){
            GetComponent<Pathfinding.AIPath>().maxSpeed = 3.5f;
        }
        canShootTwice = false;
    }
   
    public IEnumerator SpawnLogi()
    {
        logiSpawned = true;        
        anim.SetBool("enrage", true);
        spawningEnemies = true;
        int LayerIgnoreRaycast = LayerMask.NameToLayer("enemy");
        gameObject.layer = LayerIgnoreRaycast;
        boxCollider.enabled = false;
        capsuleCollider.enabled = false;
        currentState = EnemyState.attack;
        yield return new WaitForSeconds(0.25f);
        transform.position = posBoss;
        GetComponent<Pathfinding.AIPath>().maxSpeed = 0f;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
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

        
    }
}