using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class NavManager : MonoBehaviour
{
    // Collection of all panels;
    public GameObject[] panels;
    public GameObject confirmPanel;

    private GameObject currentPanel;

    // Navigate to home page when app start.
    public void GoToHome()
    {
        CloseAllPanels();
        SwapPanel("Home");
    }

    // Method name talks for itself.
    public void CloseAllPanels()
    {
        foreach(var panel in panels)
        {
            panel.SetActive(false);
        }
    }

    // 1st option to swap page - by panel tag
    public void SwapPanel(string tag)
    {
        currentPanel = panels.Where(p => p.CompareTag(tag)).FirstOrDefault();
        currentPanel.SetActive(true);
    }

    // 2nd option to swap page - by pressed button tag
    public void SwapPanel(Button bt)
    {
        CloseAllPanels();
        currentPanel = panels.Where(p => p.CompareTag(bt.tag)).FirstOrDefault();
        currentPanel.SetActive(true);
    }


    // Buttons section
    // Open new or fill a page with customer credentials.
    public void OpenAddEditCustomer(int id)
    {
        currentPanel.SetActive(false);
        var panel = panels.Where(p => p.name == "AddEditCustomerPanel").FirstOrDefault();
        panel.SetActive(true);

        FindObjectOfType<ContactsManager>().InitializeAddEditCustomerPanel(id);
    }

    // Open new or fill a page with order credentials.
    public void OpenAddEditOrder(int id)
    {
        currentPanel.SetActive(false);
        var panel = panels.Where(p => p.name == "AddEditOrderPanel").FirstOrDefault();
        panel.SetActive(true);

        FindObjectOfType<OrdersManager>().InitializeAddEditPanel(id);
    }

    // On click closes the current panel and opens the last active panel.
    public void Back(GameObject panel)
    {
        panel.SetActive(false);
        currentPanel.SetActive(true);
    }

    // Activates one of three confirm boxes:
    // 1 - Confirm box for deleting a customer.
    // 2 - Confirm box for deleting an order.
    // 3 - Confirm box for deliver an order.
    public void AskConfirm(GameObject trigger)
    {
        confirmPanel.SetActive(true);
        switch (trigger.tag)
        {
            case "DeleteCustomer":
                FindObjectOfType<ConfirmBox>().SetCommand(ConfirmBox.Command.DeleteCustomer);
                FindObjectOfType<ConfirmBox>().msg.text = "Сигурни ли сте, че искате да изтриете този клиент ?";
                break;
            case "DeleteOrder":
                FindObjectOfType<ConfirmBox>().SetCommand(ConfirmBox.Command.DeleteOrder);
                FindObjectOfType<ConfirmBox>().msg.text = "Сигурни ли сте, че искате да изтриете тази поръчка ?";
                break;
            case "DeliverOrder":
                FindObjectOfType<ConfirmBox>().SetCommand(ConfirmBox.Command.DeliverOrder);
                FindObjectOfType<ConfirmBox>().msg.text = "Сигурни ли сте, че сте доставили поръчката ?";
                break;
        }
        
    }
}
