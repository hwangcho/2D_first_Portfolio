using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperAttack : MonoBehaviour
{
    public GameObject Attack1;

    // Start is called before the first frame update
    

    
    GameObject player;
    PlayerHealth playerhealth;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerhealth = player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AllStop()
    {
        StopAllCoroutines();
    }
    public void Attack1On()
    {

        StartCoroutine(Atk1Off());
    }
    //공격 콜라이더 생성후 삭제
    public IEnumerator Atk1Off()
    {
        //yield return new WaitForSeconds(0.3f);
        Attack1.GetComponent<BoxCollider2D>().enabled = true;

        yield return new WaitForSeconds(0.15f);
        Attack1.GetComponent<BoxCollider2D>().enabled = false;
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player" && Attack1.gameObject.tag == "EnemyAttack")
        {
            Debug.Log("몬스터 공격에 맞음");


            playerhealth.TakeDamage(30, transform.position, 2f, 1f);
        }


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && gameObject.tag == "Enemy" && !player.GetComponent<Animator>().GetBool("HHit"))
        {
            playerhealth.TakeDamage(10, transform.position, 2f, 4f);
            Debug.Log("몬스터 몸통에 맞음");
            //player.GetComponent<Animator>().SetBool("Fall", false);

        }
    }
}
