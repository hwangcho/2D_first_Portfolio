using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInven : MonoBehaviour
{
    [SerializeField]
    private Inventory theInventory;  // 📜Inventory.cs
    [SerializeField]
    public int coin = 0;
    [SerializeField]
    Text goldText;
    void Start()
    {
        
    }


    void Update()
    {
        goldText.text = coin.ToString();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Item")
        {

            theInventory.AcquireItem(collision.transform.GetComponent<ItemPickup>().item);
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);
            coin += 100;
        }
    }
}
