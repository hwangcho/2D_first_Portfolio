using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBackground : MonoBehaviour
{
    GameObject player;
    MeshRenderer mesh;
    Camera camera;
    float offset;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mesh = GetComponent<MeshRenderer>();
        camera = Camera.main;
       
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(player.transform.position, Vector3.left * 1f, Color.white);
        Debug.DrawRay(player.transform.position, Vector3.right * 1f, Color.white);

        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, Vector3.left, 1f, LayerMask.GetMask("Wall"));
        RaycastHit2D hit2 = Physics2D.Raycast(player.transform.position, Vector3.right, 1f, LayerMask.GetMask("Wall"));


        if (hit.collider != null)
        {
            if (hit.distance < 0.5f)
            {

                return;
            }


        }
        else if (hit2.collider != null)
            {
            if (hit2.distance < 0.5f)
            {
             
                return;
            }
            
        }
        else
        {
            if (Input.GetKey(KeyCode.A)&&!player.GetComponent<Animator>().GetBool("Combo"))
            {
                offset -= Time.deltaTime * 0.07f;

                mesh.material.mainTextureOffset = new Vector2(offset, 0);
            }
            else if (Input.GetKey(KeyCode.D) && !player.GetComponent<Animator>().GetBool("Combo"))
            {
                offset += Time.deltaTime * 0.07f;

                mesh.material.mainTextureOffset = new Vector2(offset, 0);
            }
            else
            {


                mesh.material.mainTextureOffset = new Vector2(offset, 0);

            }
        }



    }
}
