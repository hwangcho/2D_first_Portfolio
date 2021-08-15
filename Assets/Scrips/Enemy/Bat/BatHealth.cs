using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BatHealth : MonoBehaviour
{
    [SerializeField]
    private GameObject[] item_prefab;
    int randomitem;
    public Slider healthSlider;

    public int startingHealth = 100;
    //주인공의 현재 체력 
    public int currentHealth;

    SpriteRenderer sprite;
    GameObject Player;
    
    Animator ani;

    float DamgeDelay;
    bool Death;
    void Start()
    {
        DamgeDelay = Time.time + 1;
        currentHealth = startingHealth;
        sprite = GetComponent<SpriteRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player");
        ani = GetComponent<Animator>();
       
        healthSlider.maxValue = startingHealth;
        healthSlider.value = currentHealth;
        randomitem = Random.Range(0, 10);
    }
    void Update()
    {
        healthSlider.value = currentHealth;
        //healthSlider.GetComponent<RectTransform>().localScale = new Vector3(transform.localScale.x < 0 ? -1 : 1, 1, 1);
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
                sprite.color = new Color(255, 255, 255, 0.5f);
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
        sprite.color = new Color(255, 255, 255, 0.7f);

        yield return new WaitForSeconds(0.1f);
        //playermove.Hit = false;
        sprite.color = new Color(255, 255, 255, 1f);

        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    //죽음
    void PlayerDie()
    {


    
        ani.SetBool("Die", true);
        //움직임 스크립트 없앰

        GetComponent<Bat>().enabled = false;
        GetComponent<Bat>().a.SetActive(false);
        Death = true;

        GetComponent<Rigidbody2D>().gravityScale = 1;
        //박스콜라이더 없애고 안떨어지게 리지드바디 중력 0으로 바꿈
        gameObject.layer = 10;
        GetComponent<BoxCollider2D>().isTrigger = false;
        Destroy(gameObject, 3);
        dropItem();
    }
    void dropItem()
    {
        switch (randomitem)
        {
            case 0:
                break;
            case 1:
            case 2:
                GameObject a = Instantiate(item_prefab[0], transform.position, Quaternion.identity);
                a.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 0.03f, ForceMode2D.Impulse);
                break;
            case 3:
            case 4:
                GameObject b = Instantiate(item_prefab[1], transform.position, Quaternion.identity);
                b.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 0.03f, ForceMode2D.Impulse);
                break;
            case 5:
            case 6:
                GameObject c = Instantiate(item_prefab[2], transform.position, Quaternion.identity);
                c.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 0.03f, ForceMode2D.Impulse);
                break;
            case 7:
            case 8:
                GameObject d = Instantiate(item_prefab[3], transform.position, Quaternion.identity);
                d.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 0.03f, ForceMode2D.Impulse);
                break;
            case 9:
                for (int i = 0; i < 4; i++)
                {
                    GameObject e = Instantiate(item_prefab[i], transform.position, Quaternion.identity);
                    e.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 0.03f, ForceMode2D.Impulse);

                }

                break;
            default:
                break;

        }
    }
        private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어 칼에 맞으면 
        if (collision.tag == "PlayerAttack")
        {
            TakeDamage(10, Player.transform.position, 4);
           
            
        }
    }
}
