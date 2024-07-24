using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using SQLite4Unity3d;
using UnityEngine;
using UnityEngine.UI;

public class ContactsManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject customerPanel;
    public GameObject parent;
    public GameObject AddEditCustomerPanel;
    public GameObject confirmPanel;

    // Fields for customers blank
    [Header("Customer credentials")]
    public Text txtId;
    public Text txtTitle;
    public InputField inputName;
    public InputField inputAddress;
    public InputField inputPhone;
    public InputField inputDelivery;

    [Header("Etc..")]
    public string filter;
    public Text msg;
    public List<Customer> customers;
    public Toggle isEditor;

    //Buttons for customer blank
    [Header("Buttons")]
    public GameObject back;
    public GameObject save;
    public GameObject delete;

    // Database
    private DataService ds;

    // Ohter private fields
    private List<GameObject> thelist;
    private Customer customer;
    private string lastText;

    private void Start()
    {
        ds = new DataService(PlayerPrefs.GetString("DB"));
        InitializeCustomers("");
    }

    // Load the customers
    public void InitializeCustomers(string filter)
    {
        var list = ds.GetCustomers(filter);
        thelist = new List<GameObject>();
        customers = new List<Customer>(list);
        if (customers.Count > 0)
        {
            foreach (var cust in customers)
            {
                CreatePanel(cust);
            }
        }
        else
        {
            Debug.Log("Нема никой !");
        }
    }

    // Search for customer
    public void SearchCustomer(InputField search)
    {
        RedrawCustomers(search.text);
    }

    // Create customer GUI
    private void CreatePanel(Customer cust)
    {
        GameObject g;
        g = Instantiate(customerPanel, parent.transform);
        thelist.Add(g);
        var pan = g.transform.Find("Customer");
        pan.transform.Find("ID").GetComponent<Text>().text = cust.Id.ToString();
        pan.transform.Find("CustomerName").GetComponent<Text>().text = cust.Name;
        pan.transform.Find("CustomerAddress").GetComponent<Text>().text = cust.Address;
    }

    // Reload customers
    // Optional - use filter
    public void RedrawCustomers(string filter)
    {
        foreach (var cust in thelist)
        {

            Destroy(cust);
        }
        thelist.Clear();
        customers.Clear();
        InitializeCustomers(filter);
    }

    // Determines wheter Add or Edit panel should be invoked
    public void InitializeAddEditCustomerPanel(int id)
    {
        if (id != 0)
        {
            customer = ds.FindCustomer(id);
            if (customer != null)
                InitializeEditPanel(customer);
        }
        else
            InitializeAddPanel();

    }

    // Initialize panel where user creates new customer
    private void InitializeAddPanel()
    {
        isEditor.isOn = false;
        delete.SetActive(false);
        save.SetActive(true);
        
        txtTitle.text = "Нов клиент";
        inputName.text = "";
        inputAddress.text = "";
        inputPhone.text = "";
        inputDelivery.text = "";
    }

    // Initialize panel where user edits selected customer
    private void InitializeEditPanel(Customer cust)
    {
        isEditor.isOn = true;
        save.SetActive(false);
        delete.SetActive(true);
        InitializeMessagePanel();

        txtId.text = cust.Id.ToString();
        txtTitle.text = cust.Name;
        inputName.text = cust.Name;
        inputAddress.text = cust.Address;
        inputPhone.text = cust.Phone;
        inputDelivery.text = cust.DeliveryOrder.ToString();
    }

    // Add customer.
    public void Add()
    {
        var cust = new Customer();
        cust.Name = inputName.text;
        cust.Address = inputAddress.text;
        cust.Phone = inputPhone.text;

        if (int.TryParse(inputDelivery.text, out int deliv))
            cust.DeliveryOrder = deliv;
        if (ValidateName(cust.Name) && ValidatePhone(cust.Phone))
        {
            if (ds.AddCustomer(cust))
            {
                RedrawCustomers("");
                FindObjectOfType<NavManager>().Back(AddEditCustomerPanel);
            }
            else
            {
                PrintMsg("Възникна грешка при записването !", false);
            }
        }
    }

    // Name validation
    private bool ValidateName(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            return true;
        }
        PrintMsg("Въведете име на клиента !", false);
        return false;
    }

    // Simple phone validation
    // Disabled by client request
    private bool ValidatePhone(string phone)
    {
        //if (phone.Length < 10)
        //{
        //    if (string.IsNullOrEmpty(phone))
        //    {
        //        PrintMsg("Въведете телефонен номер на клиента !", false);
        //        return false;
        //    }
        //    PrintMsg("Въвели сте непълен номер !", false);
        //    return false;
        //}
        return true;
    }

    // Update customer on input changed.
    // Triggers when input field is clicked.
    public void ClearInput(InputField input)
    {
        lastText = input.text;
    }

    // Triggers when input text is changed.
    public void InputChanged(InputField input)
    {
        if (isEditor.isOn)
        {
            if (!string.IsNullOrEmpty(input.text))
            {
                switch (input.tag)
                {
                    case "InputName":
                        customer.Name = input.text;
                        break;
                    case "InputAddress":
                        customer.Address = input.text;
                        break;
                    case "InputPhone":
                        customer.Phone = input.text;
                        break;
                    case "InputDelivery":
                        if (int.TryParse(input.text, out int deliv))
                        {
                            customer.DeliveryOrder = deliv;
                        }
                        else
                            PrintMsg("Номера на доставка е невалиден !", false);
                        break;
                    default:
                        PrintMsg("Нещо се обърка", false);
                        break;
                }
                if (ValidateName(customer.Name) && ValidatePhone(customer.Phone))
                    UpdateCust();
            }
            else
            {
                input.text = lastText;
            }
        }
    }

    // Updates the customer in DB.
    private void UpdateCust()
    {
        if (ds.UpdateCustomer(customer))
        {
            PrintMsg("Данните за клиента са обновени.", true);
            RedrawCustomers("");
        }
        else
            PrintMsg("Възникна грешка при обновяването !", false);
    }

    // Confirmation to delete customer
    public void Confirm(bool result)
    {
        if (result)
        {
            DeleteCust();
            FindObjectOfType<AudioManager>().Delete();
        }
    }

    // Delete customer on confirm
    private void DeleteCust()
    {
        if (ds.DeleteCustomer(customer))
        {
            RedrawCustomers("");
            FindObjectOfType<NavManager>().Back(AddEditCustomerPanel);
        }
        else
        {
            PrintMsg("Възникна грешка при изтриването.", false);
        }
    }

    // Log system
    private void PrintMsg(string txt, bool success)
    {
        msg.color = success ? new Color32(55, 138, 70, 255) : new Color32(219, 0, 0, 255);
        msg.text = txt;
        Invoke("ClearMsg", 5);
    }
    private void ClearMsg()
    {
        msg.text = "";
    }
    private void InitializeMessagePanel()
    {
        MsgOff();
        Invoke("MsgOn", 3);
    }
    private void MsgOff()
    {
        msg.gameObject.SetActive(false);
    }
    private void MsgOn()
    {
        msg.gameObject.SetActive(true);
    }
}
