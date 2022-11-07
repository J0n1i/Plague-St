using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class log : Enemy {

    public Rigidbody2D myRigidbody;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    
    public float timer;
    public bool isTimer;
    private float escapeRadius;
    public GameObject bullet;
    [SerializeField] public Material flashMaterial;
    [SerializeField] public float flashDuration;
    public SpriteRenderer spriteRenderer;
    public Material originalMaterial;
    public Coroutine flashRoutine;

	// Use this for initialization
	void Start () {
        timer = 4f;
        escapeRadius = 5f;
        currentState = EnemyState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        
        target = GameObject.FindWithTag("Player").transform;
        GetComponent<Pathfinding.AIPath>().enabled = false;
        float randomNum = Random.Range(0.1f, 0.3f);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 3f+randomNum;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;

        

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(isDead == false){
        CheckDistance();
        if(isTimer == true){
            timer -= Time.deltaTime;
            if(timer <= 0){
                isTimer = false;
                timer = 4f;

            }
        } 
        }else {
            //disable pathfinding
            GetComponent<Pathfinding.AIPath>().enabled = false;
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
                gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                anim.SetBool("aim", false);
                int LayerNotIgnoreRaycast = LayerMask.NameToLayer("Default");
        gameObject.layer = LayerNotIgnoreRaycast;
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
            if (currentState == EnemyState.idle && currentState != EnemyState.stagger && isTimer==false && Vector3.Distance(target.position,
                    transform.position) > escapeRadius|| currentState == EnemyState.walk
                && currentState != EnemyState.stagger && isTimer==false && Vector3.Distance(target.position,
                    transform.position) > escapeRadius)
            {
                
                StartCoroutine(AttackCo());
                
            }else {
                GetComponent<Pathfinding.AIPath>().enabled = false;
                Vector3 tempp = Vector3.MoveTowards(transform.position,
                                                         target.position,
                                                         moveSpeed * Time.deltaTime);
                changeAnim(tempp - transform.position);
                anim.SetBool("aim", true);
                gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
               int LayerIgnoreRaycast = LayerMask.NameToLayer("enemy");
                  gameObject.layer = LayerIgnoreRaycast;
            }
            if(Vector3.Distance(target.position, transform.position) <= escapeRadius){
                gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                anim.SetBool("aim", false);
                GetComponent<Pathfinding.AIPath>().enabled = false;
                int LayerIgnoreRaycast = LayerMask.NameToLayer("enemy");
                  gameObject.layer = LayerIgnoreRaycast;
                Vector3 dirToPlayer = transform.position - target.position;
                Vector3 newPos = transform.position + dirToPlayer;
                Vector3 tempp = Vector3.MoveTowards(transform.position,
                                                         newPos,
                                                         moveSpeed * Time.deltaTime);
                myRigidbody.MovePosition(tempp);
                changeAnim(tempp - transform.position);
                
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
    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashRoutine());
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
        anim.SetBool("attack", true);
        Flash();
        yield return new WaitForSeconds(0.2f);
        Flash();
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("attack", false);
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Instantiate(bullet, transform.position, rotation);
        yield return new WaitForSeconds(0.5f);
        float randomNum = Random.Range(0.1f, 0.3f);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 3f+randomNum;
        
    
    }
    public IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material = originalMaterial;
        flashRoutine = null;
    }
}