using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState{
    idle,
    walk,
    attack,
    stagger
}

public class Enemy : MonoBehaviour {
    public SignalSender killedSignal;
    public bool Elite;
    public SpriteRenderer sprite;
    public EnemyState currentState;
    public FloatValue maxHealth;
    public float health;
    public AudioClip dyingScream;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;
    public GameObject CoinDrop;
    public GameObject HeartDrop;
    public GameObject deathEffect;
    public GameObject hitEffect;
    public AudioClip damageSound;
    public Animator anim;
    public bool isDead;
    public SignalSender playerAttackSignal;
    void Start(){
        anim = GetComponent<Animator>();
    }

    private void Awake(){
        health = maxHealth.initialValue;
        
    }
    public void Boss1TakeDamage(){
        TakeDamage(1f);
    }
    private void TakeDamage(float damage)
    {
        health -= damage;
        playerAttackSignal.Raise();
        AudioPlayer.instance.PlaySound(damageSound, 1f);

        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.15f);
        if(health <= 0)
        {
            //Bossille t�� huuto vaan
            AudioPlayer.instance.PlaySound(dyingScream, 6f);

            int dice = Random.Range(1, 101);
        if(dice < 71 && Elite==false){
            Instantiate(CoinDrop, transform.position, Quaternion.identity);

            }
            else if (dice>70 && dice<80 && Elite==false) {
            Instantiate(HeartDrop, transform.position, Quaternion.identity);

        } else if(Elite==true){
            Instantiate(CoinDrop, transform.position, Quaternion.identity);
            Instantiate(CoinDrop, transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity);
            Instantiate(CoinDrop, transform.position + new Vector3(-0.5f, 0, 0), Quaternion.identity);
            Instantiate(HeartDrop, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);

        }
            GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonFinalizer>().enemies.Remove(gameObject);
            DeathEffect();
            

        }
    }
    public void DeathEffect(){
        if(deathEffect != null){
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            killedSignal.Raise();
            StartCoroutine(DeadCo());
            Destroy(effect, 0.33f);
        }
    }

    public void Knock(Rigidbody2D myRigidbody, float knockTime, float damage)
    {
        StartCoroutine(KnockCo(myRigidbody, knockTime));
        TakeDamage(damage);
        AudioPlayer.instance.PlaySound(damageSound, 1f);
    }

    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime)
    {
        if (myRigidbody != null)
        {
            GetComponent<Pathfinding.AIPath>().enabled = false;
             sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = EnemyState.idle;
            myRigidbody.velocity = Vector2.zero;
            GetComponent<Pathfinding.AIPath>().enabled = true;
        }
    }
    private IEnumerator DeadCo()
    {
        anim.SetBool("dead", true);
        isDead=true;
        GetComponent<Pathfinding.AIPath>().enabled = false;
        yield return new WaitForSeconds(0.5f);
    }
}