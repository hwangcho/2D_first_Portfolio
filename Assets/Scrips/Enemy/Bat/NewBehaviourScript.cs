using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    Vector2 backPosition;
    Bat bat;
    Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        bat = GetComponentInParent<Bat>();
        backPosition = transform.position;
        ani = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = backPosition;
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Time.time-bat.targetDelay> 0.3f)
        {
            GetComponentInParent<Bat>().targeton = true;
            ani.SetBool("Attack", true);
            //GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else if( Time.time - bat.targetDelay < 0.3f)
        {
            GetComponentInParent<Bat>().targeton = false;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponentInParent<Bat>().targeton = false;
            ani.SetBool("Attack", false);

        }

    }
}
