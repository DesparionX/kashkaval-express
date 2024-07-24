using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryBox : MonoBehaviour
{
    public void CloseDelivery()
    {
        gameObject.SetActive(false);
    }
}
