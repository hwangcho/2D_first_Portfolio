using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//플레이어 체력,피격 관리
public class PlayerHealth : MonoBehaviour
{
    public Slider healthSlider;

    public GameObject WizardfireEffct;
    public GameObject WormfireEffct;

    //주인공의 시작 체력입니다. 기본 100으로 설정
    public int startingHealth = 100;
    //주인공의 현재 체력 
    public int currentHealth;

    PlayerMove playermove;
    Animator ani;

    bool Death;
    void Awake()
    {
        currentHealth = startingHealth;
        ani = GetComponent<Animator>();
        playermove = GetComponent<PlayerMove>();
        healthSlider.maxValue = startingHealth;
        healthSlider.value = currentHealth;

      
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = currentHealth;
 
    }
    //공격맞았을때 실행

    public void TakeDamage(int amout, Vector3 EnemyPosition,float pushBack,float pushUp)
    {
        //체력깎음
        currentHealth -= amout;
        ani.ResetTrigger("Attack1");
        ani.ResetTrigger("Attack2");
        playermove.AttackDelay = 1f; //모든 공격 멈추게 하기(애니메이션 안꼬이게

        //체력게이지에 변경된 체력값을 표시
        //만약 현재 체력이 0이하가되면 죽음
        if (currentHealth <= 0 && !Death)
        {
            //플레이어가 죽었을때 수행할 명령
            PlayerDie();
        }
        else
        {
            
            if (!ani.GetBool("HHit"))
            {
                //죽지않으면
                ani.SetBool("HHit", true);
                StartCoroutine(Hittime());
                
                Vector3 diff = transform.position - EnemyPosition;
                diff = (diff / diff.sqrMagnitude);
                if(diff.x > 0.01f && !ani.GetBool("Jump"))
                {
                    GetComponent<Rigidbody2D>().velocity = Vector3.zero;

                    GetComponent<Rigidbody2D>().AddForce((new Vector3(1 * pushBack, 0.8f* pushUp, 0)) , ForceMode2D.Impulse);//피격시 플레이어 밀리게

                }
                else if(diff.x < 0.01f && !ani.GetBool("Jump"))
                {
                    GetComponent<Rigidbody2D>().velocity = Vector3.zero;

                    GetComponent<Rigidbody2D>().AddForce((new Vector3(-1 * pushBack, 0.8f* pushUp, 0)), ForceMode2D.Impulse);//피격시 플레이어 밀리게

                }
                if (diff.x > 0.01f && ani.GetBool("Jump"))
                {
                    GetComponent<Rigidbody2D>().velocity = Vector3.zero;

                    GetComponent<Rigidbody2D>().AddForce((new Vector3(1 * pushBack, 0.3f * pushUp, 0)), ForceMode2D.Impulse);//피격시 플레이어 밀리게

                }
                else if (diff.x < 0.01f && ani.GetBool("Jump"))
                {
                    GetComponent<Rigidbody2D>().velocity = Vector3.zero;

                    GetComponent<Rigidbody2D>().AddForce((new Vector3(-1 * pushBack, 0.3f * pushUp, 0)), ForceMode2D.Impulse);//피격시 플레이어 밀리게

                }
          

            }
        }
    }
    //피격후 코루틴
    IEnumerator Hittime()
    {
       
        yield return new WaitForSeconds(0.2f);

        ani.SetBool("HHit", false);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
      

    }

    //죽엇을때
    void PlayerDie()
    {
        ani.SetBool("Fall", false);
        ani.SetBool("Attack1", false);
        ani.SetBool("Attack2", false);
        ani.SetBool("Combo", false);
        ani.SetBool("Jump", false);
        ani.SetBool("Run", false);
            ani.SetBool("HHit", false);
            gameObject.layer = 10;
        ani.ResetTrigger("Dash");
        ani.SetTrigger("Death");
            playermove.enabled = false;
            Death = true;
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.tag == "WizardFireBall")
        {
            Debug.Log("몬스터 파이어볼에 맞음");
            if (!ani.GetBool("HHit"))
            {
                GameObject fireefect = Instantiate(WizardfireEffct, transform.position, Quaternion.identity);
                Destroy(fireefect, 0.5f);
                TakeDamage(40, collision.transform.position, 2f,2);
            }
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "WormFireBall")
        {
            Debug.Log("몬스터 파이어볼에 맞음");
            if (!ani.GetBool("HHit"))
            {
                GameObject fireefect = Instantiate(WormfireEffct, transform.position, Quaternion.identity);
                Destroy(fireefect, 0.5f);
                TakeDamage(40, collision.transform.position, 2f, 2);
            }
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "BossThunder")
        {
            if (!ani.GetBool("HHit"))
            {
                Debug.Log("보스 스킬에 맞음");
                TakeDamage(40, Vector3.zero, 0, 0f);
            }
        }

        

    }

   public void onHp()
    {
        currentHealth = 150;
    }
   public void ReStart()
    {
        SceneManager.LoadScene("Game");
    }

    public void IncreaseHP(int _count)
    {
        if (currentHealth + _count < startingHealth)
            currentHealth += _count;
        else
            currentHealth = startingHealth;
    }
}
