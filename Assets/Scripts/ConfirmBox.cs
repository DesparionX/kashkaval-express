using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmBox : MonoBehaviour
{
    private bool result;
    public enum Command { DeleteCustomer, DeleteOrder, DeliverOrder};
    public int command;
    public Text msg;

    public void Yes()
    {
        result = true;
        gameObject.SetActive(false);
        switch (command)
        {
            case (int)Command.DeleteCustomer:
                FindObjectOfType<ContactsManager>().Confirm(result);
                break;
            case (int)Command.DeleteOrder:
                FindObjectOfType<OrdersManager>().Confirm(result);
                break;
            case (int)Command.DeliverOrder:
                FindObjectOfType<OrdersManager>().ConfirmDeliver(result);
                break;
            default:
                break;
        }
        
        
    }
    public void No()
    {
        result = false;
        gameObject.SetActive(false);
    }
    public void SetCommand(Command cmd)
    {
        command = (int)cmd;
    }
}
