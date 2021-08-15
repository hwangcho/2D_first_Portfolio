using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{

    public Image dashfill;
    public Image skillfill;

    public float dashFillAmount = 0;
    public float skillFillAmount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dashFillAmount -= Time.deltaTime/0.6f;
        dashfill.fillAmount = dashFillAmount;
        skillFillAmount -= Time.deltaTime/6;
        skillfill.fillAmount = skillFillAmount;
        //1에서 0까지 가야됨 0.6이 1임 0이 0이고 0.6~0까지 
    }
}
