using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 공격범위 관리
public class AttackColldier : MonoBehaviour
{
    public GameObject AttackColldier1;
    public GameObject AttackColldier2;
    public AudioClip[] Atk;

    PlayerMove playermove;
    AudioSource audio;


    void Start()
    {
        audio = GetComponent<AudioSource>();

        playermove = GetComponent<PlayerMove>();
    }

    void Update()
    {
        
    }
    public void AttackSound()
    {
        audio.PlayOneShot(Atk[0]);

    }
    //공격시 공격콜라이더 생성햇다가 바로 없어지게 
    public void Attack1On()
    {
        AttackColldier1.GetComponent<PolygonCollider2D>().enabled = true;
        StartCoroutine(Atk1Off());
    }
    //공격시 공격콜라이더 생성햇다가 바로 없어지게 

    public void Attack2On()
    {
        AttackColldier2.GetComponent<PolygonCollider2D>().enabled = true;
        StartCoroutine(Atk2Off());
    }
    //이동속도 잠깐 멈추고 다시 원래속도로 되돌림
    IEnumerator Atk1Off()
    {
        //공격중에 이동,방향전환 자체 불가해서 없앤것
        yield return new WaitForSeconds(0.015f);
        AttackColldier1.GetComponent<PolygonCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.18f);
    }
    IEnumerator Atk2Off()
    {
        yield return new WaitForSeconds(0.015f);
        AttackColldier2.GetComponent<PolygonCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.18f);


    }

}
