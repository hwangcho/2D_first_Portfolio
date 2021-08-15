using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public Vector3 dir;
    GameObject player;
    float firespeed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(gameObject.tag == "WizardFireBall")
        {
            Invoke("Destroying", 1.5f);

        }
        else  if(gameObject.tag == "WormFireBall")
            {
                Invoke("Destroying", 2.8f);

            }
    }

    // Update is called once per frame
    void Update()
    {
        if(dir.x > 0)
        {
            transform.position += new Vector3(firespeed, 0, 0) * Time.deltaTime;

        }
        else
        {
            transform.position += new Vector3(-firespeed, 0, 0) * Time.deltaTime;

        }
        transform.localScale = new Vector3(dir.x > 0 ? 0.8f : -0.8f, 0.8f, 0);

    }
    void Destroying()
    {
        Destroy(gameObject);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((gameObject.tag=="WizardFireBall"||gameObject.tag == "WormFireBall")&&collision.gameObject.tag == "PlayerAttack" && player.GetComponent<PlayerMove>().skillOn)
        {
            
            if (this.gameObject.tag == "WizardFireBall")
            {
                gameObject.tag = "PlayerFire";
            }
            if(this.gameObject.tag == "WormFireBall")
            {
                gameObject.tag = "PlayerFire2";
            }
            CancelInvoke("Destroying");
            dir *= -1;
            Invoke("Destroying", 1f);
            firespeed = 5f;
            Debug.Log("패링성공");
        }
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}

//5월5일한것
//몬스터 파이어볼쏘기 
