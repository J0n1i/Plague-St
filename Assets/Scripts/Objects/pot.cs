using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pot : MonoBehaviour
{
    public GameObject CoinDrop;
    public GameObject HeartDrop;
    private Animator animator;
    public AudioClip breakSound;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }
    // Update is called once per frame
    public void Smash()
    {
        animator.SetBool("smash", true);
        StartCoroutine(breakCo());
        AudioPlayer.instance.PlaySound(breakSound, 1f);
    }

    IEnumerator breakCo()
    {
        yield return new WaitForSeconds(.3f);
        this.gameObject.SetActive(false);
        int dice = Random.Range(1, 101);
        if(dice < 71){
            Instantiate(CoinDrop, transform.position, Quaternion.identity);
            
        }else if (dice>70 && dice<80) {
            Instantiate(HeartDrop, transform.position, Quaternion.identity);
            
        }else {
            
        }
    }
}
