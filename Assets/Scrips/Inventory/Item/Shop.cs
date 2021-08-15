using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static bool shopActive = false;
    [SerializeField]
    private Inventory theInventory;
    [SerializeField]
    GameObject shopBase;
    [SerializeField]
    PlayerInven playerInven;
    public ItemPickup[] item;

    OldMan oldman;
    void Start()
    {
        oldman = GetComponentInParent<OldMan>();
    }


    void Update()
    {
        TryOpenShop();
    }

    private void TryOpenShop()
    {
        if (Input.GetKeyDown(KeyCode.S)&&!oldman.walk)
        {
            shopActive = !shopActive;
       
            if (shopActive)
                OpenShop();
            else
                CloseShop();

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (shopActive)
            {
                shopActive= !shopActive;

                CloseShop();

            }
        }
    }

    private void OpenShop()
    {
        shopBase.SetActive(true);
    }

    public void CloseShop()
    {
        shopBase.SetActive(false);
      
    }
    public void RedPotion()
    {
        if (playerInven.coin >= 100)
        {
            theInventory.AcquireItem(item[0].item);
            playerInven.coin -= 100;
        }
        else
            return;
    }
    public void GreenPotion()
    {
        if (playerInven.coin >= 100)
        {
            theInventory.AcquireItem(item[1].item);
            playerInven.coin -= 100;
        }
        else
            return;
    }
    public void SpeedPotion()
    {
        if (playerInven.coin >= 100)
        {
            theInventory.AcquireItem(item[2].item);
            playerInven.coin -= 100;
        }
        else
            return;
    }
}
