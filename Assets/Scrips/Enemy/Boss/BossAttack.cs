using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public GameObject Attack1;
    public GameObject Attack2Thunder; //파이어볼

    public GameObject Attack2Position;

  
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
    //일반공격
    public void Attack1On()
    {

        StartCoroutine(Atk1Off());
    }
    //공격 콜라이더 생성후 삭제
    public IEnumerator Atk1Off()
    {
        //yield return new WaitForSeconds(0.4f);
        Attack1.GetComponent<PolygonCollider2D>().enabled = true;

        yield return new WaitForSeconds(0.15f);
        Attack1.GetComponent<PolygonCollider2D>().enabled = false;
    }


    //텔레포트
    public void Skill2On()
    {
        Vector3 dir;
        dir = player.transform.position - transform.position; 
       
        if (player.transform.position.x - transform.position.x > 0.01)
        {
            transform.position = new Vector2(player.transform.position.x+2, transform.position.y);
            transform.localScale = new Vector3(dir.x > 0 ? -1 : 1, 1, 0);

        }
        else if(player.transform.position.x - transform.position.x < 0.01)
        {
            transform.position = new Vector2(player.transform.position.x-2, transform.position.y);
            transform.localScale = new Vector3(dir.x > 0 ? -1 : 1, 1, 0);


        }
    }
    public void Layerchange14()
    {
        gameObject.layer = 14;
    }
    public void Layerchange13()
    {
        gameObject.layer = 13;
    }
    //1스킬
    public void Attack2()
    {

        Instantiate(Attack2Thunder, new Vector3(player.transform.position.x + 0.1f, player.transform.position.y + 1.5f), Quaternion.identity);
        //fireBall.transform.Translate(new Vector3(dir.x >0 ? 1:-1, 0, 0) * 20 * Time.deltaTime);

        //파이어볼
        //왼쪽방향일때 날아가는방향 , 오른쪽일때 날아가는 방향 주기
        //날아가는 속도
    }
  
    public IEnumerator PageTwoSkill()
    {
        
        Vector3 a = player.transform.position;
        Instantiate(Attack2Thunder, new Vector3(a.x + 0.1f, a.y + 1.5f), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(Attack2Thunder, new Vector3(a.x - 1.4f, a.y + 1.5f), Quaternion.identity);
        Instantiate(Attack2Thunder, new Vector3(a.x + 1.4f, a.y + 1.5f), Quaternion.identity);

        yield return new WaitForSeconds(0.5f);
        Instantiate(Attack2Thunder, new Vector3(a.x - 3.1f, a.y + 1.5f), Quaternion.identity);
        Instantiate(Attack2Thunder, new Vector3(a.x + 3.1f, a.y + 1.5f), Quaternion.identity);

    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player" && Attack1.gameObject.tag == "EnemyAttack")
        {
            Debug.Log("몬스터 공격에 맞음");


            playerhealth.TakeDamage(15, transform.position, 6f, 2f);
        }


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && gameObject.tag == "Boss" && !player.GetComponent<Animator>().GetBool("HHit"))
        {
            playerhealth.TakeDamage(20, transform.position, 3f, 4f);
            Debug.Log("몬스터 몸통에 맞음");
            //player.GetComponent<Animator>().SetBool("Fall", false);

        }
    }
}
