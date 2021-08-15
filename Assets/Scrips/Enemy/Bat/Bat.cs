using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    Vector3 backPosition;
    public GameObject a;
    GameObject player;
    Vector3 dir;
    Vector3 backdir;

    public float targetDelay;


    public bool targeton;
    // Start is called before the first frame update
    void Start()
    {
        targetDelay = Time.time - 1;
        player = GameObject.FindGameObjectWithTag("Player");
        backPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        dir = player.transform.position - transform.position;
        backdir = backPosition - transform.position;
        //a.transform.position = backPosition;

        if (targeton)
        {
            transform.Translate(dir.normalized * 2 * Time.deltaTime);
        }
        else if (!targeton)
        {
            transform.Translate(backdir.normalized * 2 * Time.deltaTime);

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !player.GetComponent<Animator>().GetBool("HHit") && this.gameObject.tag == "Bat")
        {
            player.GetComponent<PlayerHealth>().TakeDamage(4, transform.position, 4, 1);
            targetDelay = Time.time;
        }
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.gameObject.tag == "Player"&&!player.GetComponent<Animator>().GetBool("HHit")&&this.gameObject.tag == "Bat")
    //    {
    //        player.GetComponent<PlayerHealth>().TakeDamage(4, transform.position, 4, 1);
    //        targetDelay = Time.time;
    //    }
    //}
}
