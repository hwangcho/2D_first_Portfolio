using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 AI?
public class MonsterAstar : MonoBehaviour
{
    GameObject Player;
    Animator ani;

    public int next; //좌,우,멈춤 설정 랜덤값
    public bool Target; //플에이어 범위찾기
    public float AttackDelay = 0;
    Vector3 dir;
    MonsterAttack monsterAttack;
    // Start is called before the first frame update
    void Awake()
    {
        next = Random.Range(-1, 2); 
        AttackDelay = Time.time -2;
        ani = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        monsterAttack = GetComponent<MonsterAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Target)
        {
            TargetOn();

        }
        else { 
            moving();

        }
        
    }

    void moving()
    {
        //플레이어가 범위안에 들어오면 실행
        if (Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) < 4 && Player.transform.position.y - transform.position.y < 0.5f && Player.transform.position.y - transform.position.y > -2f)
        {
            Target = true;
            next = 0;
            CancelInvoke("Think");
        }
        switch (next) //좌,우,중지 값
        {
            case 0://중지
                transform.Translate(Vector2.zero);
                if (!IsInvoking("Think"))//인보크중이 아니라면 실행
                {
                    Invoke("Think", 1);
                    ani.SetBool("Walk", false);
                }
                break;
            case -1://좌
                transform.Translate(Vector2.left * Time.deltaTime);
                Debug.DrawRay(transform.position, Vector3.left * 0.4f, Color.white);
                RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector3.left, 0.4f, LayerMask.GetMask("Wall"));
                if (hit2.collider != null)
                {
                    Debug.Log("몬스터벽닿");
                    //-1 곱해줘서 방향 전환
                    next *= -1;
                    transform.localScale = new Vector3(next, 1, 0);


                    CancelInvoke("Think");
                    Invoke("Think", 3f);
                }
                if (!IsInvoking("Think"))
                {
                   
                    ani.SetBool("Walk", true);
                    transform.localScale = new Vector3(next, 1, 0);//scale x축 next값 줘서 좌우 반전
                    Invoke("Think", 3);
                   
                }
                break;
            case 1://우
                transform.Translate(Vector2.right * Time.deltaTime);
                RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector3.right, 0.4f, LayerMask.GetMask("Wall"));
                Debug.DrawRay(transform.position, Vector3.right * 0.4f, Color.white);
                if (hit3.collider != null)
                {
                    Debug.Log("몬스터벽닿");
                    //-1 곱해줘서 방향 전환
                    next *= -1;
                    transform.localScale = new Vector3(next, 1, 0);

                    //기존 인보크 취소하고 다시 인보크줌
                    Debug.Log("몬스터 떨");
                    CancelInvoke("Think");
                    Invoke("Think", 3f);

                }
                if (!IsInvoking("Think"))
                {
                   
                    Invoke("Think", 3);
                    transform.localScale = new Vector3(next, 1, 0);
                    ani.SetBool("Walk", true);
                   
                    
                    
                 
                }
                break;
        }
        //몬스터 바닦체크후 안떨어지게함
        Debug.DrawRay(new Vector3(transform.position.x + 0.2f * next, transform.position.y - 1, transform.position.z), Vector3.down, Color.black);
        RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x + 0.2f * next, transform.position.y - 1, transform.position.z)
            , Vector3.down, 1, LayerMask.GetMask("Wall"));
        if (hit.collider == null)
        {
            //-1 곱해줘서 방향 전환
            next *= -1;
            transform.localScale = new Vector3(next , 1, 0);
            if(next == 0)
            {
                //이거안해주면 0일때 땅에 꺼짐
                transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 0);
            }
            //기존 인보크 취소하고 다시 인보크줌
            Debug.Log("몬스터 떨");
            CancelInvoke("Think");
            Invoke("Think", 3f);
        }
    }
    void Think()
    {
        next = Random.Range(-1, 2);
    }
    void TargetOn()
    {
         dir = Player.transform.position - transform.position; //거리구하기 
        //공격딜레이 동안 움직이기X && 거리 4이하 3이상 일때 추격
        if (Time.time - AttackDelay > 2.0f && Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) <4 && 
            Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) > 2.5f)
        {
            
            transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 0);

            ani.SetBool("Walk", true);
            transform.Translate(new Vector3(dir.x, 0, 0).normalized * Time.deltaTime);
        }
        else if (Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) <= 2.5 && Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) > 1.1f 
            && Player.transform.position.y - transform.position.y > -1.5f &&Player.transform.position.y - transform.position.y < 1.7f)
        {
            ani.SetBool("Walk", false);
            if (Time.time - AttackDelay > 1.5f)
            {
                transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 0);
            }
            if (Time.time - AttackDelay > 2.0f)
            {
                //Hit 트리거 초기화해줘야 애니 안꼬임
                ani.ResetTrigger("Hit");
                ani.SetTrigger("Attack2");
                monsterAttack.Attack2(dir);
                AttackDelay = Time.time;
            }
        }
        //플레이어와 거리 1.2밑이면 멈추고 공격실행
        else if (Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) <= 1.1f && Player.transform.position.y - transform.position.y > -1.5f&&Player.transform.position.y - transform.position.y < 1.7f)
        {
            ani.SetBool("Walk", false);
            if (Time.time - AttackDelay > 1.5f)
            {
                transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 0);
            }
                if (Time.time - AttackDelay > 1.5f)
            {
                //Hit 트리거 초기화해줘야 애니 안꼬임
                ani.ResetTrigger("Hit");
                //transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 0);
                ani.SetTrigger("Attack");
                monsterAttack.Attack1On();
                AttackDelay = Time.time;
            }
            
        }
        //범위밖으로 나오면 실행
        else if(Time.time - AttackDelay > 1f && (Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) >=4 || Player.transform.position.y - transform.position.y < -1.5f || Player.transform.position.y - transform.position.y > 1.7f))
        {
            Target = false;
            ani.SetBool("Walk", false);

        }

    }

   

}
