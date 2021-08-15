using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperAstar : MonoBehaviour
{
    GameObject Player;
    Animator ani;

    public int next; //좌,우,멈춤 설정 랜덤값
    public bool Target; //플에이어 범위찾기
    public float AttackDelay = 0;
    Vector3 dir;
    ReaperAttack reaperAttack;
    // Start is called before the first frame update
    void Awake()
    {
        next = Random.Range(-1, 2);
        AttackDelay = Time.time - 2;
        ani = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        reaperAttack = GetComponent<ReaperAttack>();
    }
    public enum LayerName
    {
        NoWeaponLayer = 0,
        OnWeaponLayer = 1,
        
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
        if (Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) < 1.5f && Player.transform.position.y - transform.position.y < 0.5f && Player.transform.position.y - transform.position.y > -0.5f)
        {
            
            StartCoroutine(TargetOon());
        }
        
    }
   
    void TargetOn()
    {
        dir = Player.transform.position - transform.position; //거리구하기 
                                                              //공격딜레이 동안 움직이기X && 거리 4이하 3이상 일때 추격
        if (Time.time - AttackDelay > 1.5f && Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) < 4
            && Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) > 0.9f && Player.transform.position.y - transform.position.y > -1.5f && Player.transform.position.y - transform.position.y < 1.9f)
        {

            transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 0);
            ani.SetBool("Walk", true);
            transform.Translate(new Vector3(dir.x, 0, 0).normalized *2* Time.deltaTime);
        }
        
        //플레이어와 거리 1.2밑이면 멈추고 공격실행
        else if (Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) <= 0.9f && Player.transform.position.y - transform.position.y > -1.5f && Player.transform.position.y - transform.position.y < 1.9f)
        {
            ani.SetBool("Walk", false);
            if (Time.time - AttackDelay > 1f)
            {
                transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 0);
            }
            if (Time.time - AttackDelay > 1.3f)
            {
                //Hit 트리거 초기화해줘야 애니 안꼬임
                ani.ResetTrigger("Hit");
                //transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 0);
                //reaperAttack.Attack1On();
                ani.SetTrigger("Attack");
                AttackDelay = Time.time;
            }

        }

        //범위밖으로 나오면 실행
        else if (Time.time - AttackDelay > 1f && (Vector2.Distance(new Vector2(Player.transform.position.x, 0), new Vector2(transform.position.x, 0)) >= 4 || Player.transform.position.y - transform.position.y < -1.5f || Player.transform.position.y - transform.position.y > 2.3f))
        {
            Target = false;
            ani.ResetTrigger("WeaponOn");

            ani.SetTrigger("WeaponOut");
            ani.SetBool("Walk", false);
            StartCoroutine(TargetOff());

        }

    }
    public IEnumerator TargetOff()
    {
        yield return new WaitForSeconds(0.7f);
        transform.GetChild(1).gameObject.SetActive(false);

        ActivateLayer(LayerName.NoWeaponLayer);


    }
    public IEnumerator TargetOon()
    {
        ani.SetTrigger("WeaponOn");
        ani.ResetTrigger("WeaponOut");
        
        yield return new WaitForSeconds(0.7f);
        transform.GetChild(1).gameObject.SetActive(true);

        Target = true;
        ActivateLayer(LayerName.OnWeaponLayer);

    }
    //public void HandleLayers()
    //{
    //    //움직일때
    //    if (IsMoving)
    //    {
    //        myAnimator.SetFloat("x", direction.x);
    //        myAnimator.SetFloat("y", direction.y);
    //    }
    //    else if (isAttacking)
    //    {


    //    }
    //    else  //idle
    //    {
    //        ActivateLayer(LayerName.IdleLayer);

    //    }
    //}

    public void ActivateLayer(LayerName layerName)
    {
        for (int i = 0; i < ani.layerCount; i++)
        {
            ani.SetLayerWeight(i, 0);
        }
        ani.SetLayerWeight((int)layerName, 1);
    }
}
