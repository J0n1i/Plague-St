using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState{
    walk,
    attack,
    interact,
    stagger,
    idle
}

public class PlayerMovement : MonoBehaviour {

    public SpriteRenderer sprite;
    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    public FloatValue currentHealth;
    public SignalSender playerHealthSignal;
    private float rollSpeed;
    public float rollLength = .6f;
    public float rollCooldown = 5f;
    private float activeMoveSpeed;
    public bool isRolling;
    public int coins;
    public Image cooldownImage;

	// Use this for initialization
	void Start () {
        rollSpeed = speed * 2.5f;
        activeMoveSpeed = speed;
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);

	}
	
	// Update is called once per frame
	void Update () {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        
       if(Input.GetButtonDown("attack") && currentState != PlayerState.attack 
           && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());
        }
      
        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            
            UpdateAnimationAndMove();
        }
	}

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        currentState = PlayerState.walk;
    }
  
    private IEnumerator RollCo()
    {
        isRolling=true;
        activeMoveSpeed = rollSpeed;
        yield return new WaitForSeconds(rollLength);
        activeMoveSpeed = speed;
        cooldownImage.enabled = true;
        StartCoroutine(RollCooldownCo());
        
       
        yield return new WaitForSeconds(rollCooldown);
        cooldownImage.enabled = false;
        isRolling=false;
    }
    private IEnumerator RollCooldownCo(){
        float alpha = 1f;
     while(alpha > 0)
        {
            alpha -= Time.deltaTime / rollCooldown;
            cooldownImage.color = new Color(1,1,1,alpha);
            yield return null;
        }
    }
    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        change.Normalize();
        
        myRigidbody.MovePosition(
            transform.position + change * activeMoveSpeed * Time.fixedDeltaTime
        );

        if(Input.GetButtonDown("roll") && currentState != PlayerState.attack 
           && currentState != PlayerState.stagger && isRolling == false)
        { 
            StartCoroutine(RollCo());
        }
        }
    public void RollCooldownPowerup(){
        if(rollCooldown == 0)
        {
            return;
        } else {
        rollCooldown =  rollCooldown-0.5f;
        print(rollCooldown);
        }
       
    }
    public void SpeedPowerup(){
        speed = speed * 1.1f;
        rollSpeed = rollSpeed * 1.1f;
        activeMoveSpeed = speed;
        print(speed);
        print(rollSpeed);
       
    }

    public void Knock(float knockTime, float damage)
    {
        currentHealth.RuntimeValue-=damage;
        playerHealthSignal.Raise();
        if(currentHealth.RuntimeValue > 0)
        {
        
        StartCoroutine(KnockCo(knockTime));
        }else{
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator KnockCo(float knockTime)
    {
        if (myRigidbody != null)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }
    
}