using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using TMPro;

public enum PlayerState
{
    walk,
    attack,
    interact,
    stagger,
    idle
}

public class PlayerMovement : MonoBehaviour
{
    private bool bossKilled = false;
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
 
    public SignalSender playerDamageSignal;
    private float rollSpeed;
    public float rollLength = .6f;
    public float rollCooldown = 5f;

    private float originalSpeed;
    private float originalRollSpeed;
    private float originalRollCooldown;
    public GameObject dashEffect;
    private float activeMoveSpeed;
    private float timer;
    private bool isTimer;
    public bool isRolling;

    public int coins;
    public int spd;
    public int cd;
    public int dmg;

    public Image cooldownImage;
    public Image cooldownImage2;
    public Image cooldownImage3;

    private DeathScreen deatscreen;
    public Inventory playerInventory;
    public AudioClip attackSound;
    public AudioClip playerDies;
    public Joystick joystick;
    public Transform closestEnemy;
    private List<GameObject> enemies;
    public AudioSource footstepSound;
    public GameObject powerupEffect;
    public Collider2D triggerCollider;
    public bool inputEnabled = true;
    private int enemiesKilled;
    private float timePlayed;
    private bool bossRoomEntered;

    async void Start2()
    {
        try
        {
            await UnityServices.InitializeAsync();
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
        }
        catch (ConsentCheckException e)
        {
            // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
        }
    }
    // Use this for initialization
    void Start()
    {
        Initialize();
        Start2();
        enemiesKilled = 0;
        closestEnemy = null;
        rollSpeed = speed * 2.5f;
        activeMoveSpeed = speed;
        originalSpeed = speed;
        originalRollSpeed = rollSpeed;
        originalRollCooldown = rollCooldown;
        timePlayed = 0;
        bossRoomEntered = false;
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }

    public void Initialize()
    {
        gameObject.transform.position = Vector2.zero;
        Debug.Log("Initialized");
        joystick = GameObject.Find("Fixed Joystick").GetComponent<Joystick>();
        deatscreen = GameObject.Find("DeathScreenCanvas").GetComponent<DeathScreen>();
        enemies = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonFinalizer>().enemies;
    }

    void Update()
    {
        if(joystick == null)
        {
            Initialize();
        }

        timePlayed += Time.deltaTime;

        if (inputEnabled == true)
        {
            if (Input.GetButtonDown("attack") && currentState != PlayerState.attack
                && currentState != PlayerState.stagger)
            {
                StartCoroutine(AttackCo());
            }
            
            else if (Input.GetButtonDown("roll") && currentState != PlayerState.attack
               && currentState != PlayerState.stagger && isRolling == false)
            {
                StartCoroutine(RollCo());
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        closestEnemy = getClosestEnemy(enemies);


        change = Vector3.zero;
        //change.x = Input.GetAxisRaw("Horizontal");
        //change.y = Input.GetAxisRaw("Vertical");
        //n채채 ylemm채t pois kommentista nii toimii n채ppis ja alemmat kommenteiks
        change.x = joystick.Horizontal;
        change.y = joystick.Vertical;

        if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {

            UpdateAnimationAndMove();
        }
      




    }


    public Transform getClosestEnemy(List<GameObject> enemies)
    {

        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject enemy in enemies)
        {
            Vector3 diff = enemy.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closestEnemy = enemy.transform;
                distance = curDistance;
            }
        }
        return closestEnemy;
    }


    public void pressedAttack()
    {
        if (currentState != PlayerState.attack
      && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());
            RandomSoundPlayer.RandomSndP.PlayWeaponSound();
        }
    }
    public void pressedDash()
    {
        if (currentState != PlayerState.attack
      && currentState != PlayerState.stagger && isRolling == false)
        {
            StartCoroutine(RollCo());
        }
    }
    public void EnemiesKilled()
    {
        enemiesKilled++;
    }

    private IEnumerator AttackCo()
    {

        float xDistance = closestEnemy.transform.position.x - transform.position.x;
        float yDistance = closestEnemy.transform.position.y - transform.position.y;
        if (closestEnemy != null && Vector3.Distance(closestEnemy.position, transform.position) < 3f)
        {
            if (Mathf.Abs(xDistance) > Mathf.Abs(yDistance))
            {
                if (xDistance > 0)
                {
                    animator.SetFloat("moveX", 1);
                    animator.SetFloat("moveY", 0);
                }
                else
                {
                    animator.SetFloat("moveX", -1);
                    animator.SetFloat("moveY", 0);
                }
            }
            else
            {
                if (yDistance > 0)
                {
                    animator.SetFloat("moveX", 0);
                    animator.SetFloat("moveY", 1);
                }
                else
                {
                    animator.SetFloat("moveX", 0);
                    animator.SetFloat("moveY", -1);
                }
            }
        }


        playerAttackSignal.Raise();
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        currentState = PlayerState.walk;

    }

    
    private IEnumerator RollCo()
    {
        isRolling = true;
        dashEffect.SetActive(true);
        activeMoveSpeed = rollSpeed;
        yield return new WaitForSeconds(rollLength);
        dashEffect.SetActive(false);
        activeMoveSpeed = speed;
        cooldownImage.enabled = true;
        cooldownImage2.enabled = true;
        cooldownImage3.enabled = true;
        StartCoroutine(RollCooldownCo());


        yield return new WaitForSeconds(rollCooldown);
        cooldownImage.enabled = false;
        cooldownImage2.enabled = false;
        cooldownImage3.enabled = false;
        isRolling = false;
    }

    private IEnumerator RollCooldownCo()
    {
        /*float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / rollCooldown;
            cooldownImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }*/
        //fill cooldownimage2 while rollcooldown active
        float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / rollCooldown;
            cooldownImage2.fillAmount = alpha;
            yield return null;
        }
    }
    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero && inputEnabled == true)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
            footstepSound.enabled = true;
        }
        else
        {
            animator.SetBool("moving", false);
            footstepSound.enabled = false;
        }
    }

    void MoveCharacter()
    {
        change.Normalize();

        myRigidbody.MovePosition(
            transform.position + change * activeMoveSpeed * Time.fixedDeltaTime
        );


    }
    public void RollCooldownPowerup()
    {

        float rollCooldownUpgrade;
        rollCooldownUpgrade = (originalRollCooldown * 1.2f) - originalRollCooldown;
        if (rollCooldown - rollCooldownUpgrade <= 0)
        {
            return;
        }
        rollCooldown -= rollCooldownUpgrade;
        print(rollCooldown);


    }
    public void SpeedPowerup()
    {
        //
        float speedUpgrade;
        float rollUpgrade;
        speedUpgrade = (originalSpeed * 1.2f) - originalSpeed;
        rollUpgrade = (originalRollSpeed * 1.2f) - originalRollSpeed;
        speed += speedUpgrade;
        rollSpeed += rollUpgrade;
        activeMoveSpeed = speed;
        print(speed);
        print(rollSpeed);

    }
    public void PowerupEffect()
    {
        StartCoroutine(PowerupEffectCo());
    }
    public IEnumerator PowerupEffectCo()
    {
        powerupEffect.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        powerupEffect.SetActive(false);


    }

    public void Knock(float knockTime, float damage)
    {
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {

            StartCoroutine(KnockCo(knockTime));
            RandomSoundPlayer.RandomSndP.PlayPlayerSound();
        }
        else
        {
            SendAnalytics();
            PlayerIsDead = true;
            AudioPlayer.instance.PlaySound(playerDies, 1f);
            deatscreen.ShowDeathScreen();
            FindObjectOfType<LevelMusic>().DeathMusic();
            this.gameObject.SetActive(false);
        }

    }
    public void SendAnalytics()
    {

        Dictionary<string, object> parameter = new Dictionary<string, object>()
        {
            { "Enemies_Killed", enemiesKilled },
            { "Time_Playeddd", (int)timePlayed},
            { "Boss_Killed", bossKilled},
            {"BossRoom_Entered", bossRoomEntered}

      };
        Events.CustomData("EnemiesKilled", parameter);
    }
    public void BossKilled()
    {
        bossKilled = true;
        SendAnalytics();
    }
    public void BossRoomEntered()
    {
        bossRoomEntered = true;
    }

    private IEnumerator KnockCo(float knockTime)
    {
        if (myRigidbody != null)
        {
            playerDamageSignal.Raise();
            triggerCollider.enabled = false;
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(knockTime);
            triggerCollider.enabled = true;
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }

}