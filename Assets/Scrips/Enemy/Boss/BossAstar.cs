using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAstar : MonoBehaviour
{
    GameObject Player;
    Animator ani;
    SpriteRenderer sprite;

    
    public bool pageTwo;

    public int next; //좌,우,멈춤 설정 랜덤값
    public bool Target; //플에이어 범위찾기
    public float AttackDelay = 0;
    public float RandomDelay;
    public int randomSkill;
    public int randomAttack;
    public float TelpoDelay;


    Vector3 dir;
    BossAttack bossAttack;
    // Start is called before the first frame update
    void Awake()
    {
    sprite = GetComponent<SpriteRenderer>();

        next = Random.Range(-1, 2);
        AttackDelay = Time.time - 2;
        TelpoDelay = Time.time - 2;
        ani = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        bossAttack = GetComponent<BossAttack>();
    }

    // Update is called once per frame
    void Update()
    {

        //보스방 입장시 실행 or boss 소환
        if (Target && !pageTwo)
        {
            TargetOn();

        }
        if (Target && pageTwo)
        {
            PageTwoOn();
        }



    }
    public void AllStop()
    {
        StopAllCoroutines();
    }

    //void moving()
    //{
    //    //플레이어가 범위안에 들어오면 실행
    //    if (Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) < 8)
    //    {
    //        Target = true;

    //    }
    //}
    void TargetOn()
    {
        //계속 추척해
        //공격 and 스킬 사용 and 순간이동 사용때만 추적x 좌우반전도 x
        //일정량 떨어져있으면 스킬(순간이동 or 공격스킬)사용 
        //근접시 공격 or 공격스킬 
        //공격시 공격딜레이 랜덤으로 주기!
        //텔레포트 하게되면 플레이어 뒤로 이동 
        //플레이어가 보스보다 왼쪽에있으면 플레이어 왼쪽으로
        
        dir = Player.transform.position - transform.position; //거리구하기 
        if(Time.time - AttackDelay > 1.5-RandomDelay &&Time.time-TelpoDelay >2 &&Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) > 3f)
        {
            transform.Translate(new Vector3(dir.x, 0, 0).normalized*2 * Time.deltaTime);
            transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 0);

            ani.SetBool("Walk", true);
        }
        else
        {
            ani.SetBool("Walk", false);

        }

        //공격딜레이 동안 움직이기X && 거리 4이하 3이상 일때 추격
        if ( Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) >5)
        {

            if (Time.time - AttackDelay > 1.5f)
            {
                transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 0);
            }
            if (Time.time - AttackDelay > 2.4f)
            {
                randomAttack = Time.time - TelpoDelay > 2.2f ? Random.Range(1, 5) : Random.Range(1, 4);
                ani.SetBool("Walk", false);
                RandomDelay = Random.Range(-0.7f, 0.2f);
                ani.ResetTrigger("Hit");
                AttackDelay = Time.time + RandomDelay;

                switch (randomAttack)
                {
                    case 1:
                    case 2:
                    case 3:
                        ani.SetTrigger("Skill");
                        
                        break;
                    case 4:
                        ani.SetTrigger("Skill2");
                        TelpoDelay = Time.time;
                        break;
                }
                //Hit 트리거 초기화해줘야 애니 안꼬임
                
            }
        }
        
        //플레이어와 거리 1.2밑이면 멈추고 공격실행
        else if (Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) <= 5f)
        {
            if (Time.time - AttackDelay > 1.7f)
            {
                transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 0);
            }
            if (Time.time - AttackDelay > 2f)
            {
                randomAttack = Time.time - TelpoDelay > 2.2f ? Random.Range(1, 6) : Random.Range(1, 5);
                ani.SetBool("Walk", false);
                RandomDelay = Random.Range(-0.7f, 0.2f);
                ani.ResetTrigger("Hit");
                AttackDelay = Time.time + RandomDelay;

                switch (randomAttack)
                {
                    case 1:
                    case 2:
                    case 3:

                        ani.SetTrigger("Attack");
                        //bossAttack.Attack1On();
                        break;

                    case 4:
                        ani.SetTrigger("Skill");
                        break;

                    case 5:
                        ani.SetTrigger("Skill2");
                        TelpoDelay = Time.time;

                        break;
                }





                
                
            }

        }
        //범위밖으로 나오면 실행
        //else if (Time.time - AttackDelay > 1f && Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) >= 8)
        //{
        //    Target = false;
        //    ani.SetBool("Walk", false);

        //}

    }
    void PageTwoOn()
    {
        dir = Player.transform.position - transform.position; //거리구하기 
        if (Time.time - AttackDelay > 1.7f - RandomDelay && Time.time - TelpoDelay > 1.5f && Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) > 4f)
        {
            transform.Translate(new Vector3(dir.x, 0, 0).normalized * 2 * Time.deltaTime);
            transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 0);

            ani.SetBool("Walk", true);
        }
        else
        {
            ani.SetBool("Walk", false);

        }

        //공격딜레이 동안 움직이기X && 거리 4이하 3이상 일때 추격
        if (Time.time - AttackDelay > 2.0f && Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) > 5)
        {

            if (Time.time - AttackDelay > 1.5f)
            {
                transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 0);
            }
            if (Time.time - AttackDelay > 2.4f)
            {
                randomAttack = Time.time - TelpoDelay > 1.7f ? Random.Range(1, 5) : Random.Range(1, 4);
                ani.SetBool("Walk", false);
                RandomDelay = Random.Range(-0.7f, 0.2f);
                ani.ResetTrigger("Hit");
                AttackDelay = Time.time + RandomDelay;

                switch (randomAttack)
                {
                    case 1:
                    case 2:
                        ani.SetTrigger("2PageSkill");
                        AttackDelay += 0.2f;

                        break;
                    case 3:

                    case 4:
                        TelpoDelay = Time.time;
                        AttackDelay -= 0.7f;
                        ani.SetTrigger("2PageSkill2");

                        break;
                }
                //Hit 트리거 초기화해줘야 애니 안꼬임

            }
        }

        //플레이어와 거리 1.2밑이면 멈추고 공격실행
        if (Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) <= 5f&& Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) >= 2f)
        {
            if (Time.time - AttackDelay > 1.5f - RandomDelay)
            {
                transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 0);
            }
            if (Time.time - AttackDelay > 2f)
            {
                randomAttack = Time.time - TelpoDelay > 1.7f ? Random.Range(1, 7) : Random.Range(1, 6);
                ani.SetBool("Walk", false);
                RandomDelay = Random.Range(-0.7f, 0.2f);
                ani.ResetTrigger("Hit");
                AttackDelay = Time.time + RandomDelay;

                switch (randomAttack)
                {
                    case 1:
                    case 2:
                    case 3:
                        ani.SetTrigger("Attack");
                        break;

                    

                        //기본공격
                        
               
                    case 4:
                    case 5:

                        ani.SetTrigger("2PageSkill");
                        AttackDelay += 0.2f;

                        break;
                    case 6:

                        TelpoDelay = Time.time;
                        AttackDelay -= 0.7f;
                        ani.SetTrigger("2PageSkill2");

                        break;
                }


            }
         }
        if (Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) < 2f)
        {
            if (Time.time - AttackDelay > 1.3f - RandomDelay)
            {
                transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 0);
            }
            if (Time.time - AttackDelay > 2.3f)
            {
                randomAttack = Time.time - TelpoDelay > 1.5f?Random.Range(1, 7) : Random.Range(1, 6);
                ani.SetBool("Walk", false);
                RandomDelay = Random.Range(-0.7f, 0.2f);
                ani.ResetTrigger("Hit");
                AttackDelay = Time.time + RandomDelay;

                switch (randomAttack)
                {
                    case 1:
                        ani.SetTrigger("Attack");
                    
                        break;
                    case 2:
                    case 3:
                    case 4:




                        StartCoroutine(Attakc1Twice());
                        break;



                        //기본공격
                  
                 
                    case 5:
                        ani.SetTrigger("2PageSkill");
                        AttackDelay += 0.2f;

                        break;
                    case 6:

                        TelpoDelay = Time.time;
                        AttackDelay -= 0.7f;
                        ani.SetTrigger("2PageSkill2");

                        break;
                }


            }
        }
    }
  

    IEnumerator Attakc1Twice()
    {
        //transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 0);
        AttackDelay = Time.time;
        ani.SetTrigger("Attack");

        yield return new WaitForSeconds(0.95f);
        AttackDelay = Time.time + RandomDelay;
        transform.localScale = new Vector3(transform.localScale.x == 1? -1 : 1, 1, 0);
        ani.SetTrigger("Attack");
        //bossAttack.Attack1On();




    }
}
