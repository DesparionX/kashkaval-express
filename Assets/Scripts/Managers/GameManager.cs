using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
     void Start()
    {
        if (StartSync())
        {
            FindObjectOfType<PermissionManager>().AskForPermission();
            FindObjectOfType<AudioManager>().StartEngine();
            FindObjectOfType<PriceManager>().InitializePrices();
            FindObjectOfType<OrdersManager>().InitializeTodayOrders();
            FindObjectOfType<OrdersManager>().InitializeOrders();
            FindObjectOfType<NavManager>().GoToHome();
        }
    }

    // Create database or connect to existing one
    private bool StartSync()
    {
        try
        {
            var ds = new DataService("KashkavalExpressDB.db");
            PlayerPrefs.SetString("DB", "KashkavalExpressDB.db");
            ds.CreateDB();
            return true;
        }
        catch (Exception err)
        {
            Debug.LogError(" ERROR " + err.ToString());
        }
        return false;
    }
    
}
