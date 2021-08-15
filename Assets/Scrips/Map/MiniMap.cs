using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public static bool minimapActived = false;

    [SerializeField]
    private GameObject miniMapBase;
    [SerializeField]
    // Start is called before the first frame update
    void Start()
    {
        
    }


    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            minimapActived = !minimapActived;

            if (minimapActived)
                OpenMiniMap();
            else
                CloseMiniMap();

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (minimapActived)
            {
                minimapActived = !minimapActived;

                CloseMiniMap();

            }
        }
    }

    private void OpenMiniMap()
    {
        miniMapBase.SetActive(true);
    }

    private void CloseMiniMap()
    {
        miniMapBase.SetActive(false);
    }
}
