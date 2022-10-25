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
    public SignalSender roomSignal;

    public SpriteRenderer sprite;
    public EnemyState currentState;
    public FloatValue maxHealth;
    public float health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;
    public GameObject CoinDrop;
    public GameObject HeartDrop;
    public GameObject deathEffect;
    public AudioClip damageSound;
    private void Awake(){
        health = maxHealth.initialValue;
    }
    private void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
             if (roomSignal != null)
            {
                roomSignal.Raise();
            }
        int dice = Random.Range(1, 101);
        if(dice < 71){
            Instantiate(CoinDrop, transform.position, Quaternion.identity);
            AudioPlayer.instance.PlaySound(damageSound, 1f);

            }
            else if (dice>70 && dice<80) {
            Instantiate(HeartDrop, transform.position, Quaternion.identity);
            AudioPlayer.instance.PlaySound(damageSound, 1f);

        } else {
            
        }
            DeathEffect();
            this.gameObject.SetActive(false);
            AudioPlayer.instance.PlaySound(damageSound, 1f);

        }
    }
    public void DeathEffect(){
        if(deathEffect != null){
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
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
}