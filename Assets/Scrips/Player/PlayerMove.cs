using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//플레이어 이동,공격,점프,대쉬 관리
public class PlayerMove : MonoBehaviour
{
    GameObject Boss;
    public Slider MpSlider;
    public float startingMP= 100;
    public float currentMP;

   
    bool AKey;
    bool DKey;
    bool wKey;

    bool Death = false;
    bool Dash = true;
    public bool Hit = false;
    public bool skillOn;

    float skillDelay;

    public float moveSpeed = 3f;
    public float jumpPower = 2f;
    
    public float AttackDelay = 0;
    public float AttackMpDelay = 0;

   public float DashDelay = 0;
    float DashMPDelay = 0;

    float speedPotionTime = 0;

    float jumpTimeCounter;
    float jumpTime = 0.25f;

    Animator ani;
    Rigidbody2D rigid;
    SpriteRenderer spriterend;
    public GameObject DashDust;
    SkillManager skillmanager;
    public AudioClip[] moveSound;



    void Awake()
    {
        ani = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriterend = GetComponent<SpriteRenderer>();
        skillmanager = GetComponent<SkillManager>();


        AttackDelay = Time.deltaTime -1;
        AttackMpDelay = Time.deltaTime - 1;
        DashDelay = Time.deltaTime -1;
        DashMPDelay = Time.deltaTime - 1;
        skillDelay = Time.deltaTime- 6f;
        currentMP = startingMP;
        MpSlider.maxValue = startingMP;
        MpSlider.value = currentMP;
        speedPotionTime = Time.time - 8;
        Boss = GameObject.FindGameObjectWithTag("Boss");
    }

    // Update is called once per frame
    void Update()
    {
        
            Moving();
            Jumping();
            KeyPress();
            MpSystem();




        Attacking();
            Dashing();
            Skill();
        if (Time.time - speedPotionTime > 7)
        {
            moveSpeed = 3f;
        }


    }

    //키관리
    void KeyPress()
    {
        //대쉬중에 키변경 불가
        if (Dash)
        {
            AKey = Input.GetKey(KeyCode.A);
            DKey = Input.GetKey(KeyCode.D);
        }
        wKey = Input.GetKeyDown(KeyCode.W);
    }
    //스테미너(mp)관리
    void MpSystem()
    {

        if (Time.time - AttackMpDelay >= 0.8f && Time.time-DashMPDelay >=0.8f)
        {
            currentMP += 1f;
        }

        if (currentMP < 0)
        {
            currentMP = 0;
            AttackMpDelay = Time.time + 0.8f;
            DashMPDelay = Time.time + 0.8f;
        }
        else if (currentMP > 100)
        {
            currentMP = 100;
        }
        
            MpSlider.value = currentMP;

        

        //어택딜레이,대쉬딜레이
    }

    //움직임 관리
    void Moving()
    {


        
        //공격맞을때랑 공격중에 방향전환,이동불가
        if (!ani.GetBool("HHit")&&!ani.GetBool("Combo"))
        {

            if (AKey)
            {
                if (!ani.GetBool("Jump") && !ani.GetBool("Fall"))
                    ani.SetBool("Run", true);
                else
                    ani.SetBool("Run", false);

                Debug.DrawRay(transform.position, Vector3.left * 1f, Color.white);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.left,1f, LayerMask.GetMask("Wall"));
              
                if (hit.collider != null)
                {
                    if (hit.distance < 0.4f)
                    {

                        transform.Translate(Vector2.left * moveSpeed * 0 * Time.deltaTime);
                       

                        //바라보는 방향 변경
                        transform.localScale = new Vector3(-1, 1, 0);
                        Debug.Log(hit.collider.name);
                    }
                    else
                    {
                        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
                        transform.localScale = new Vector3(-1, 1, 0);
                        



                    }


                }
                else if(hit.collider ==null)
                {
                   
                        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

                    //바라보는 방향 변경
                    transform.localScale = new Vector3(-1, 1, 0);
                    
                }


            }
            else if (DKey)
            {
                if (!ani.GetBool("Jump") && !ani.GetBool("Fall"))
                    ani.SetBool("Run", true);
                else
                    ani.SetBool("Run", false);

                Debug.DrawRay(transform.position, Vector3.right * 1f, Color.white);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right, 1f, LayerMask.GetMask("Wall"));
               

                if (hit.collider != null)
                {
                    if (hit.distance < 0.4f)
                    {

                        transform.Translate(Vector2.right * moveSpeed * 0 * Time.deltaTime);
                       
                        //바라보는 방향 변경
                        transform.localScale = new Vector3(1, 1, 0);
                        Debug.Log(hit.collider.name);
                    }
                    else
                    {
                        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
                        transform.localScale = new Vector3(1, 1, 0);
                    }


                }
                else if (hit.collider == null)
                {
                    transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
                    //바라보는 방향 변경
                    transform.localScale = new Vector3(1, 1, 0);

                }

            }
            else
            {
                ani.SetBool("Run", false);
            }

        }
    }

    //점프 관리
    void Jumping()
    {
        if (ani.GetBool("HHit"))
        {
            //공격맞앗을때 점프멈추게
            jumpTimeCounter = -1;

        }
        if (!ani.GetBool("HHit")&&Dash)
        {
            if (rigid.velocity.y < -6)
            {
                //가속도 -6이하로 안가게
                rigid.velocity = new Vector2(rigid.velocity.x,-6);
            }
            
                //점프키 눌럿을때
            if (wKey && !ani.GetBool("Jump") && !ani.GetBool("Fall"))
            {
                rigid.velocity = Vector2.zero;
                jumpTimeCounter = jumpTime;
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                
                ani.SetBool("Jump", true);
                ani.SetBool("Fall", false);
            }
            //점프키 누른상태 지속되면 점프 계속 상승 
            if(Input.GetKey(KeyCode.W) && ani.GetBool("Jump") && !ani.GetBool("Fall"))
            {

                if(jumpTimeCounter > 0)
                {
                    jumpTimeCounter -= Time.deltaTime;
                    rigid.velocity = Vector2.up * 5;
                }
                else
                {
                    jumpTimeCounter = -1;
                }
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                jumpTimeCounter = -1;
            }
            //떨어질때 가속도 y값 -1 이하일때 실행
            if (rigid.velocity.y < -1)
            {
                ani.SetBool("Jump", false);
                ani.SetBool("Fall", true);
                DashDelay = Time.time - 0.55f;//대쉬딜레이줘서 대쉬 오류 없앰

                //레이캐스트 땅에 닿았을때 점프 다시 할수있게 해줌
                Debug.DrawRay(rigid.position, Vector3.down * 1f, Color.white);
                RaycastHit2D hit = Physics2D.Raycast(rigid.position, Vector3.down, 1f, LayerMask.GetMask("Wall"));
                
                if (hit.collider != null)
                {
                    if (hit.distance <0.7f)
                    {
                        Debug.Log("땅밝ㅂ");
                        //리지드바디 속도 zero 안해주면 이상해짐
                        rigid.velocity = Vector2.zero;
              

                        //Debug.Log(hit.collider.name);
                        ani.SetBool("Jump", false);
                        ani.SetBool("Fall", false);

                    }
                }
            }
            if (rigid.velocity.y == 0)
            {
                ani.SetBool("Jump", false);
                ani.SetBool("Fall", false);

            }
        }
    }

    //공격관리
    void Attacking()
    {

        //맞는 애니 실행동안 공격불가하게
        if (!ani.GetBool("HHit") && currentMP >0&& !Inventory.invectoryActivated&&!Shop.shopActive)
        {
            //첫공격 any state 에서 실행
            if (Input.GetMouseButtonDown(0) && !ani.GetBool("Attack1"))
            {
                //공격할때 조금씩 앞으로 나가게 해줌
                if(transform.localScale == new Vector3(1, 1, 0))
                {
                    transform.Translate(new Vector3(1, 0, 0)*Time.deltaTime);
                }
                else
                {
                    transform.Translate(new Vector3(-1, 0, 0)*Time.deltaTime);

                }
                currentMP -= 7;
                AttackMpDelay = Time.time;
                //DashDelay = Time.time - 0.58f;

                AttackDelay = Time.time;
                ani.SetBool("Combo", true);
                ani.SetBool("Attack1", true);
                ani.SetBool("Attack2", false);
            }
            //공격 AttackDelay 증가할수록 콤보공격 하기 힘듬
            else if (Input.GetMouseButtonDown(0) && Time.time - AttackDelay > 0.1f && ani.GetBool("Attack1"))
            {

                if (transform.localScale == new Vector3(1, 1, 0))
                {
                    transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime);
                }
                else
                {
                    transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime);

                }
                currentMP -= 7;
                AttackMpDelay = Time.time;
                //DashDelay = Time.time - 0.58f;

                AttackDelay = Time.time;
                ani.SetBool("Combo", true);
                ani.SetBool("Attack1", false);
                ani.SetBool("Attack2", true);

            }
            else if (Input.GetMouseButtonDown(0) && Time.time - AttackDelay > 0.1f && ani.GetBool("Attack2"))
            {
                if (transform.localScale == new Vector3(1, 1, 0))
                {
                    transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime);
                }
                else
                {
                    transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime);

                }
                currentMP -= 7;
                AttackMpDelay = Time.time;
                //DashDelay = Time.time - 0.58f;

                AttackDelay = Time.time;
                ani.SetBool("Combo", true);
                ani.SetBool("Attack1", true);
                ani.SetBool("Attack2", false);

            }
        }
            if (Time.time - AttackDelay > 0.25f)
            {

                AttackDelay = Time.time;
                ani.SetBool("Combo", false);

                ani.SetBool("Attack1", false);
                ani.SetBool("Attack2", false);
            }
        
    }

    //대쉬 
    void Dashing()
    {
        //스페이스바 누르면 실행 
        //대쉬 딜레이줌
        if (Input.GetButton("Jump")&&Time.time-DashDelay>0.61f&&Dash && (AKey || DKey)
            &&!ani.GetBool("Jump") && !ani.GetBool("Fall") && !ani.GetBool("Attack1")
            && !ani.GetBool("Attack2")&&!ani.GetBool("HHit") && currentMP > 0&&rigid.velocity.y==0&& rigid.velocity.x == 0&&!Inventory.invectoryActivated && !Shop.shopActive)
        {

            if (AKey && DKey)
                return;
            if (AKey&&gameObject.transform.localScale == new Vector3(-1,1,0))
            {       
                rigid.AddForce(Vector2.left * 15, ForceMode2D.Impulse);
                gameObject.layer = 10;

             
                StartCoroutine(Dashoff());
                return;
            }
           if (DKey&&gameObject.transform.localScale == new Vector3(1, 1, 0))
            {
                rigid.AddForce(Vector2.right * 15, ForceMode2D.Impulse);
                gameObject.layer = 10;

                StartCoroutine(Dashoff());
                return;

            }



        }
    }

    //대쉬동안 투명 -> 원래색으로
    IEnumerator Dashoff()
    {
        GameObject dashdust = Instantiate(DashDust, transform.position+new Vector3(0,-0.5f,0), Quaternion.identity);
        Destroy(dashdust, 0.5f);
        spriterend.color = new Color(255, 255, 255, 0.1f);
        Dash = false;
        DashDelay = Time.time;
        skillmanager.dashFillAmount = 1;
        currentMP -= 9;
        DashMPDelay = Time.time;
        yield return new WaitForSeconds(0.05f);
        

        spriterend.color = new Color(255, 255, 255, 0.3f);
        yield return new WaitForSeconds(0.05f);

        Dash = true;
        rigid.velocity = new Vector2(0, rigid.velocity.y);

        spriterend.color = new Color(255, 255, 255, 0.5f);

        yield return new WaitForSeconds(0.1f);
        spriterend.color = new Color(255, 255, 255, 0.8f);
        gameObject.layer = 0;

        yield return new WaitForSeconds(0.1f);
        spriterend.color = new Color(255, 255, 255, 1);

    }
    void Skill()
    {
        if (Dash&&!ani.GetBool("Jump") && !ani.GetBool("Fall") && !ani.GetBool("Attack1")
            && !ani.GetBool("Attack2") && !ani.GetBool("HHit")&& !Inventory.invectoryActivated && !Shop.shopActive)
        {
            if (Input.GetMouseButtonDown(1) && !skillOn && Time.time - skillDelay > 6)
            {
                Time.timeScale = 0.5f;
                skillOn = true;
                skillDelay = Time.time;
                skillmanager.skillFillAmount = 1;
            }
          
        }
        if (skillOn && Time.time - skillDelay > 0.5f)
        {
            Time.timeScale = 1f;
            skillOn = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BossStage")
        {
            Debug.Log("보스방입장");
            Boss.GetComponent<BossAstar>().Target = true;
            Boss.transform.GetChild(1).gameObject.SetActive(true);
            Boss.transform.GetChild(2).gameObject.SetActive(true);

        }
        if(collision.tag == "StageOut")
        {
            transform.position = new Vector3(98.16f, -5.06f, 0);
            GetComponent<PlayerHealth>().currentHealth /= 2;
        }
    }

    public void IncreaseMP(int _count)
    {
        if (currentMP + _count < startingMP)
           currentMP += _count;
        else
            currentMP = startingMP;
    }
    public void IncreaseSPEED(int _count)
    {
        
        if (moveSpeed == 3)
        {
            speedPotionTime = Time.time;
            moveSpeed *= (_count*0.7f);
        }
        else
        {
            speedPotionTime = Time.time;
        }
    }
}
