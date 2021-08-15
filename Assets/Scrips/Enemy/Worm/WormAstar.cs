using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormAstar : MonoBehaviour
{
    GameObject Player;
    Animator ani;

    public int next; //좌,우,멈춤 설정 랜덤값
    public bool Target; //플에이어 범위찾기
    public float AttackDelay = 0;
    Vector3 dir;
    WormAttack wormAttack;
    // Start is called before the first frame update
    void Awake()
    {
        next = Random.Range(-1, 2);
        AttackDelay = Time.time - 2;
        ani = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        wormAttack = GetComponent<WormAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Target)
        {
            TargetOn();

        }
        else
        {
            moving();

        }

    }

    void moving()
    {
        //플레이어가 범위안에 들어오면 실행
        if (Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) < 8 && Player.transform.position.y - transform.position.y > -1.5f && Player.transform.position.y - transform.position.y < 1.7f)
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
                transform.Translate(Vector2.left*1.3f * Time.deltaTime);
                Debug.DrawRay(transform.position, Vector3.left * 0.8f, Color.white);
                RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector3.left, 0.8f, LayerMask.GetMask("Wall"));
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
                transform.Translate(Vector2.right*1.3f * Time.deltaTime);
                RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector3.right, 0.8f, LayerMask.GetMask("Wall"));
                Debug.DrawRay(transform.position, Vector3.right * 0.8f, Color.white);
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
        Debug.DrawRay(new Vector3(transform.position.x + 0.5f * next, transform.position.y , transform.position.z), Vector3.down, Color.black);
        RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x + 0.2f * next, transform.position.y , transform.position.z)
            , Vector3.down, 1, LayerMask.GetMask("Wall"));
        if (hit.collider == null)
        {
            //-1 곱해줘서 방향 전환
            next *= -1;
            transform.localScale = new Vector3(next, 1, 0);
            if (next == 0)
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
        //거리 8이하이면 공격
        if (Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) < 8 &&Player.transform.position.y - transform.position.y > -1.5f && Player.transform.position.y - transform.position.y < 2.3f)
        {

            ani.SetBool("Walk", false);
            if (Time.time - AttackDelay > 1.5f)
            {
                transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 0);
            }
            if (Time.time - AttackDelay > 2.4f)
            {
                //Hit 트리거 초기화해줘야 애니 안꼬임
                ani.ResetTrigger("Hit");
                ani.SetTrigger("Attack");
                wormAttack.Attack2(dir);
                AttackDelay = Time.time;
            }
        }
       
        //범위밖으로 나오면 실행
        else if(Time.time - AttackDelay > 1.5f && (Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) >= 8 || Player.transform.position.y - transform.position.y < -1.5f || Player.transform.position.y - transform.position.y > 2.3f))
        {
            Target = false;
            ani.SetBool("Walk", false);

        }

    }


}
