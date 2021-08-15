using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThunder : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //transform.position = new Vector2(player.transform.position.x+0.1f,player.transform.position.y+1.5f);
        Destroy(gameObject, 1.4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ThunderColliderOn()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        StartCoroutine(ColliderOff());

    }
    IEnumerator ColliderOff()
    {
        yield return new WaitForSeconds(0.02f);
        GetComponent<BoxCollider2D>().enabled = false;

    }

}
