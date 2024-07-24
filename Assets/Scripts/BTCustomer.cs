using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTCustomer : MonoBehaviour
{
    public void GetCustomerId()
    {
        var text = transform.Find("ID").GetComponent<Text>().text;
        if (int.TryParse(text, out int id))
            FindObjectOfType<NavManager>().OpenAddEditCustomer(id);
    }
    public void DropDownCustomerId()
    {
        var text = transform.Find("ID").GetComponent<Text>().text;
        if (int.TryParse(text, out int id))
        {
            FindObjectOfType<OrdersManager>().FillFields(id);
        }
        FindObjectOfType<NameSearch>().CloseDD();
    }
    public void GetOrderId()
    {
        var text = transform.Find("ID").GetComponent<Text>().text;
        if (int.TryParse(text, out int id))
        {
            var parent = transform.Find("Parent").GetComponent<Text>().text;
            if (parent.Equals("Order"))
                FindObjectOfType<NavManager>().OpenAddEditOrder(id);
            else if (parent.Equals("Delivery"))
            {
                FindObjectOfType<OrdersManager>().InitializeDeliveryPanel(id);
            }

        }
            
    }
}
