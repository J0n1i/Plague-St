using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum PlayerState{
    walk,
    attack,
    interact,
    stagger,
    idle
}

public class PlayerMovement : MonoBehaviour {

    public bool PlayerIsDead = false;
    public SpriteRenderer sprite;
    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    public FloatValue currentHealth;
    public SignalSender playerHealthSignal;
    public SignalSender playerAttackSignal;
    public SignalSender playerSpecialSignal;
    public SignalSender playerDamageSignal;
    private float rollSpeed;
    public float rollLength = .6f;
    public float rollCooldown = 5f;
    public float specialCooldown = 30f;
    private float originalSpeed;
    private float originalRollSpeed;
    private float activeMoveSpeed;
    private float timer;
    private bool isTimer;
    public bool isRolling;
    public bool isSpecial;
    public int coins;
    public Image cooldownImage;
    private Image specialCooldownImage;
    private DeathScreen deatscreen;

	// Use this for initialization
	void Start () {
        rollSpeed = speed * 2.5f;
        activeMoveSpeed = speed;
        originalSpeed = speed;
        originalRollSpeed = rollSpeed;

        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        deatscreen = GameObject.Find("DeathScreenCanvas").GetComponent<DeathScreen>();
        specialCooldownImage = GameObject.Find("specialCooldown").GetComponent<Image>();
        specialCooldownImage.enabled = true;
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
        else if(Input.GetButtonDown("special") && currentState != PlayerState.attack 
           && currentState != PlayerState.stagger && isSpecial == false)
        {
            StartCoroutine(SpecialCo());
            
        }

        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            
            UpdateAnimationAndMove();
        }
        if (isTimer == true){
            StartCoroutine(SpecialCooldownCo());
        }
        
	}
    

    private IEnumerator AttackCo()
    {
        playerAttackSignal.Raise();
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        currentState = PlayerState.walk;
    }

    private IEnumerator SpecialCo()
    {
        isSpecial=true;
        
        playerSpecialSignal.Raise();
        currentState = PlayerState.attack;
        animator.SetBool("special", true);
        yield return null;
        animator.SetBool("special", false);   
        yield return new WaitForSeconds(.5f);
        currentState = PlayerState.walk;
        isTimer=true;
        timer = specialCooldown;
        yield return new WaitForSeconds(specialCooldown);
        isSpecial=false;
    }
    private IEnumerator SpecialCooldownCo(){
        timer -= Time.deltaTime;
           specialCooldownImage.fillAmount = 1;
     while(specialCooldownImage.fillAmount > 0)
        {
            specialCooldownImage.fillAmount = timer / specialCooldown;
            yield return null;
        }
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
        //
        float speedUpgrade;
        float rollUpgrade;
        speedUpgrade = (originalSpeed * 1.1f)-originalSpeed;
        rollUpgrade = (originalRollSpeed * 1.1f)-originalRollSpeed;
        speed += speedUpgrade;
        rollSpeed += rollUpgrade;
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
            PlayerIsDead = true;
            deatscreen.ShowDeathScreen();

        }
    }

    private IEnumerator KnockCo(float knockTime)
    {
        if (myRigidbody != null)
        {
            playerDamageSignal.Raise();
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