using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormAttack : MonoBehaviour
{

    public GameObject Attack2Ball; //파이어볼

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
    

    public void Attack2(Vector3 dir)
    {
        StartCoroutine(Attack2Delay(dir));

        //fireBall.transform.Translate(new Vector3(dir.x >0 ? 1:-1, 0, 0) * 20 * Time.deltaTime);

        //파이어볼
        //왼쪽방향일때 날아가는방향 , 오른쪽일때 날아가는 방향 주기
        //날아가는 속도
    }
    public IEnumerator Attack2Delay(Vector3 dir)
    {
        yield return new WaitForSeconds(0.9f);
        GameObject fireBall = Instantiate(Attack2Ball, Attack2Position.transform.position, Quaternion.identity);


        fireBall.GetComponent<FireBall>().dir = dir;

    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && gameObject.tag == "Enemy" && !player.GetComponent<Animator>().GetBool("HHit"))
        {
            playerhealth.TakeDamage(10, transform.position, 3f,4f);
            Debug.Log("몬스터 몸통에 맞음");
            //player.GetComponent<Animator>().SetBool("Fall", false);

        }
    }
}
