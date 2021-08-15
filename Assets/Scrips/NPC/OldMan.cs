using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldMan : MonoBehaviour
{
    [SerializeField]
    GameObject talkPanel;
    [SerializeField]
    GameObject sPanel;

    SpriteRenderer sprite;
    Animator ani;
    public bool walk;
    bool right;
    //움직이면서 몇초마다 말했다가 안했다가함
    //플레이어랑 만나면 대화x s버튼창 오픈하고 움직임도 멈춤
    // Start is called before the first frame update
    void Awake()
    {
         walk = true;
         right = true;
        sprite = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (walk)
        {
            if (right)
            {
                transform.Translate(new Vector3(1f, 0, 0) * Time.deltaTime);
                sprite.flipX = false;
            }
            else
            {
                transform.Translate(new Vector3(-1f, 0, 0) * Time.deltaTime);
                sprite.flipX = true;
            }
        }
        if(transform.position.x > 4.5f)
        {
            right = false;
        }else if (transform.position.x < 0){

            right = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            sPanel.SetActive(true);
            talkPanel.SetActive(false);
            walk = false;
            ani.SetBool("Idle", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            sPanel.SetActive(false);
            talkPanel.SetActive(true);
            walk = true;
            ani.SetBool("Idle", false);
            Shop.shopActive = false;
            GetComponentInChildren<Shop>().CloseShop();
        }
    }

}
