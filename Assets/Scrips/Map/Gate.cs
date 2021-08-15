using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;


public class Gate : MonoBehaviour
{
    public static float alpa;
    [SerializeField]
    GameObject nextGate;
    [SerializeField]
    GameObject cine;
    [SerializeField]
    PolygonCollider2D dungeaon1;

    public GameObject blackPanel;

    [SerializeField]
    RenderTexture miniMap;

    [SerializeField]
    GameObject miniMapRawImage;
    GameObject player;

    [SerializeField]
    GameObject sPanel;
    bool inside;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Gate.alpa = 1;
    }

    // Update is called once per frame
    void Update()
    {


        if (inside)
        {
            blackPanel.GetComponent<Image>().color = new Color(0, 0, 0, Gate.alpa);
            alpa -= 0.02f;
        }
       if(alpa <= 0)
        {
            inside = false;
            Gate.alpa = 1;
        }
       
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetKey(KeyCode.S))
        {
            if (Gate.alpa != 1)
            {
                return;
            }
            else 
            { 
                Debug.Log("던전입장");
                miniMapRawImage.GetComponent<RawImage>().texture = null;
            cine.GetComponent<CinemachineConfiner>().m_BoundingShape2D = null;
            player.transform.position = nextGate.transform.position;
            cine.GetComponent<CinemachineConfiner>().m_BoundingShape2D = dungeaon1;
                miniMapRawImage.GetComponent<RawImage>().texture = miniMap;
            inside = true;
            }
            
            
        }
        
    }
  
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            sPanel.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            sPanel.SetActive(false);
        }
    }


}
