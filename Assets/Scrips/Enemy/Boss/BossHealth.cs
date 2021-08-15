using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public Slider healthSlider;

    public int startingHealth = 100;
    //주인공의 현재 체력 
    public int currentHealth;

    SpriteRenderer sprite;
    GameObject Player;
    BossAstar BossMove;
    Animator ani;

    Vector3 startPos;
    float DamgeDelay;
    bool Death;
    void Start()
    {
        DamgeDelay = Time.time + 1;
        currentHealth = startingHealth;
        sprite = GetComponent<SpriteRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player");
        ani = GetComponent<Animator>();
        BossMove = GetComponent<BossAstar>();
        healthSlider.maxValue = startingHealth;
        healthSlider.value = currentHealth;
        startPos = transform.position;
    }
    void Update()
    {
        healthSlider.value = currentHealth;
        //healthSlider.GetComponent<RectTransform>().localScale = new Vector3(transform.localScale.x < 0 ? -1 : 1, 1, 1);
        if( currentHealth<=startingHealth *0.5 &&!BossMove.pageTwo)
        {
            ani.ResetTrigger("Hit");

            ani.SetTrigger("PageOn");
     
        }
    }
    IEnumerator pagetwoOn()
    {
        gameObject.layer = 14;
        BossMove.pageTwo = true;

        BossMove.TelpoDelay = Time.time;
        BossMove.AttackDelay = Time.time;

        yield return new WaitForSeconds(0.8f);
        sprite.color = new Color(255 / 255f, 150 / 255f, 255 / 255f, 255 / 255f);
        transform.position = startPos;
        BossMove.TelpoDelay = Time.time-0.5f;
        BossMove.AttackDelay = Time.time-1.3f;
        BossMove.RandomDelay = 0;
        yield return new WaitForSeconds(0.6f);

        gameObject.layer = 13;


    }
    // Update is called once per frame
    public void TakeDamage(int amout, Vector3 EnemyPosition, float pushBack)
    {
        currentHealth -= amout;

        //체력게이지에 변경된 체력값을 표시
        //만약 현재 체력이 0이하가되면 죽음
        if (currentHealth <= 0 && !Death)
        {
            //플레이어가 죽었을때 수행할 명령
            PlayerDie();
        }
        else
        {
            //피격 딜레이 줌
            if (Time.time - DamgeDelay > 0.1f)
            {
                //죽지않으면
                //컬러 알파값 낮춰서 맞은거처럼 해줌
                //sprite.color = new Color(255, 255, 255, 0.5f);
                StartCoroutine(Hittime());

                Vector3 diff = transform.position - EnemyPosition;
                diff = diff / diff.sqrMagnitude;
                GetComponent<Rigidbody2D>().AddForce((new Vector3(diff.x, diff.y, diff.z)) * pushBack, ForceMode2D.Impulse);
            }
        }
    }
    //피격후 다시 되돌릴것들
    //투명해지는거 없애고 피격 애니넣어서 사라진것들 많음 
    //리지드바디 속도 줄여야되서 필요!
    IEnumerator Hittime()
    {
        DamgeDelay = Time.time;
        yield return new WaitForSeconds(0.1f);
        //sprite.color = new Color(255, 255, 255, 0.7f);

        yield return new WaitForSeconds(0.1f);
        //playermove.Hit = false;
        //sprite.color = new Color(255, 255, 255, 1f);

        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    //죽음
    void PlayerDie()
    {

        BossMove.AllStop();
        GetComponent<BossAttack>().AllStop();
        ani.ResetTrigger("Hit");
        ani.SetBool("Die", true);
        //움직임 스크립트 없앰
        BossMove.enabled = false;
        Death = true;
        //박스콜라이더 없애고 안떨어지게 리지드바디 중력 0으로 바꿈
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        Destroy(gameObject, 1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어 칼에 맞으면 
        if (collision.tag == "PlayerAttack")
        {
            TakeDamage(10, Player.transform.position, 1);
            //이동중이 아니고 && 공격중아니면 맞는모션 실행
            if (!ani.GetBool("Walk") && Time.time - BossMove.AttackDelay > 1.2f-BossMove.RandomDelay&&Time.time-BossMove.TelpoDelay>1.5f)
                ani.SetTrigger("Hit");
        }
        if(collision.tag == "StageOut")
        {
            transform.position = startPos;
        }
    }
}
