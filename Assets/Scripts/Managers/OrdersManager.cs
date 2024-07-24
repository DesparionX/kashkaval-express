using SQLite4Unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class OrdersManager : MonoBehaviour
{
    // Panels
    [Header("Panels")]
    public GameObject homePanel;
    public GameObject homeParent;
    public GameObject ordersPanel;
    public GameObject ordersParent;
    public GameObject ddParent;
    public GameObject AddEditOrderPanel;
    public GameObject DeliveryOrderPanel;

    // Prefabs
    [Header("Prefabs")]
    public GameObject orderPrefab;
    public GameObject dateTemplate;
    public GameObject dropDownTempl;

    // Input fields
    [Header("Fields")]
    public Text txtId;
    public Text txtTitle;
    public Text txtDeliveryOrder;
    public Text date;
    public Text msg;
    public Text orderNoMSG;
    public Text orderTotalCost;
    public InputField custName;
    public InputField custAddress;
    public InputField custPhone;
    public InputField cheeseK;
    public InputField cheeseS;
    public InputField milk;
    public Toggle isEditor;

    // Buttons
    [Header("Buttons")]
    public GameObject save;
    public GameObject delete;
    public GameObject back;

    // Fields
    private List<Order> todayOrders;
    private List<Order> allOrders;
    private List<GameObject> oList;
    private List<GameObject> aList;

    private List<Customer> ddCustomers;
    private List<GameObject> ddList;

    private Order order;
    private DataService ds;
    private string lastText;

    void Start()
    {
        // Initializing database connection.
        ds = new DataService(PlayerPrefs.GetString("DB"));
        
        InitializeDropDown("");
    }
    
    // Today orders
    // Load today orders
    public void InitializeTodayOrders()
    {
        oList = new List<GameObject>();
        todayOrders = new List<Order>(ds.GetOrdersForToday());
        foreach(var order in todayOrders)
        {
            DisplayOrder(order);
        }
    }
    
    // Create GUI for today orders
    private void DisplayOrder(Order order)
    {
        var o = Instantiate(orderPrefab, homeParent.transform);
        oList.Add(o);
        var templ = o.transform.Find("Order");
        templ.transform.Find("ID").GetComponent<Text>().text = order.Id.ToString();
        templ.transform.Find("Parent").GetComponent<Text>().text = "Delivery";
        templ.transform.Find("CustomerName").GetComponent<Text>().text = order.Name;
        templ.transform.Find("CustomerAddress").GetComponent<Text>().text = order.Address;
        templ.transform.Find("CKQuantity").GetComponent<Text>().text = $"{order.CheeseK} бр";
        templ.transform.Find("CSQuantity").GetComponent<Text>().text = $"{order.CheeseS} бр";
        templ.transform.Find("MQuantity").GetComponent<Text>().text = $"{order.Milk} бр";
    }

    // Reload today orders
    private void RefreshTodayOrders()
    {
        foreach(var ord in oList)
        {
            Destroy(ord);
        }
        oList.Clear();
        todayOrders.Clear();
        InitializeTodayOrders();
    }

    // All orders
    // Load all orders
    public void InitializeOrders()
    {
        aList = new List<GameObject>();
        allOrders = new List<Order>(ds.GetAllOrders());
        DoTheMagic(allOrders);
    }

    // Reload all orders
    private void RefreshAllOrders()
    {
        foreach (var ord in aList)
        {
            Destroy(ord);
        }
        aList.Clear();
        allOrders.Clear();
        InitializeOrders();
    }

    // Order all orders by date and seperate them also by date
    private void DoTheMagic(List<Order> list)
    {
        List<string> dates = list.Select(x => x.Date.ToShortDateString()).Distinct().ToList();
        foreach(var date in dates)
        {
            CreateDateTemplate(date);
            var orders = list.Where(o => o.Date.ToShortDateString() == date).ToList();
            foreach(var order in orders)
            {
                CreateOrderTemplate(order);
            }
        }
    }

    // Create date separator GUI for the orders
    private void CreateDateTemplate(string date)
    {
        GameObject d = Instantiate(dateTemplate, ordersParent.transform);
        aList.Add(d);
        var templ = d.transform.Find("DateText");
        templ.transform.Find("Text").GetComponent<Text>().text = date;
    }

    // Create the orders GUI
    private void CreateOrderTemplate(Order order)
    {
        var o = Instantiate(orderPrefab, ordersParent.transform);
        aList.Add(o);
        var templ = o.transform.Find("Order");
        templ.transform.Find("ID").GetComponent<Text>().text = order.Id.ToString();
        templ.transform.Find("Parent").GetComponent<Text>().text = "Order";
        templ.transform.Find("CustomerName").GetComponent<Text>().text = order.Name;
        templ.transform.Find("CustomerAddress").GetComponent<Text>().text = order.Address;
        templ.transform.Find("CKQuantity").GetComponent<Text>().text = $"{order.CheeseK} бр";
        templ.transform.Find("CSQuantity").GetComponent<Text>().text = $"{order.CheeseS} бр";
        templ.transform.Find("MQuantity").GetComponent<Text>().text = $"{order.Milk} бр";
    }

    // Drop Down with customer names.
    // Initialize the drop down menu.
    private void GetTheCustomers(string filter)
    {
        ddCustomers = new List<Customer>(ds.GetCustomers(filter));
    }
    private void InitializeDropDown(string filter)
    {
        ddList = new List<GameObject>();
        GetTheCustomers(filter);
        if(ddCustomers.Count > 0)
        {
            foreach(var cust in ddCustomers)
            {
                AddCustomerToDD(cust);
            }
        }
    }
    // Adding the customers to it.
    private void AddCustomerToDD(Customer customer)
    {
        GameObject item;
        item = Instantiate(dropDownTempl, ddParent.transform);
        ddList.Add(item);
        var templ = item.transform.Find("Cust");
        templ.transform.Find("ID").GetComponent<Text>().text = customer.Id.ToString();
        templ.transform.Find("CustName").GetComponent<Text>().text = customer.Name;
    }
    // The search method.
    public void FillDropDown(string filter)
    {
        foreach(var cust in ddList)
        {
            Destroy(cust);
        }
        ddList.Clear();
        ddCustomers.Clear();
        InitializeDropDown(filter);
    }
    public void Suggestions(InputField input)
    {
        FillDropDown(input.text);
    }
    public void FillFields(int id)
    {
        var choosenCustomer = ds.FindCustomer(id);
        custName.text = choosenCustomer.Name;
        custAddress.text = choosenCustomer.Address;
        custPhone.text = choosenCustomer.Phone;
        txtDeliveryOrder.text = choosenCustomer.DeliveryOrder.ToString();
        if (isEditor.isOn)
        {
            order.Name = choosenCustomer.Name;
            order.Address = choosenCustomer.Address;
            order.Phone = choosenCustomer.Phone;
            order.DeliveryOrder = choosenCustomer.DeliveryOrder;
            UpdateOrder();
        }
    }

    // Determines wheter Add or Edit panel should be invoked
    public void InitializeAddEditPanel(int id)
    {
        if (id != 0)
        {
            order = ds.FindOrder(id);
            if (order != null)
                InitializeEditPanel(order);
        }
        else
            InitializeAddPanel();
    }

    // Initialize panel where user edit selected order
    private void InitializeEditPanel(Order ord)
    {
        isEditor.isOn = true;
        save.SetActive(false);
        delete.SetActive(true);
        InitializeMessagePanel();

        txtTitle.text = $"Поръчка № {ord.Id}";
        txtDeliveryOrder.text = ord.DeliveryOrder.ToString();
        custName.text = ord.Name;
        custAddress.text = ord.Address;
        custPhone.text = ord.Phone;
        date.text = ord.Date.ToShortDateString();
        cheeseK.text = $"{ord.CheeseK}";
        cheeseS.text = $"{ord.CheeseS}";
        milk.text = $"{ord.Milk}";

    }
   
    // Initialize panel where user creates new order
    private void InitializeAddPanel()
    {
        isEditor.isOn = false;
        delete.SetActive(false);
        save.SetActive(true);

        txtTitle.text = "Нова поръчка";
        txtDeliveryOrder.text = string.Empty;
        custName.text = string.Empty;
        custAddress.text = string.Empty;
        custPhone.text = string.Empty;
        date.text = DateTime.Today.ToShortDateString();
        cheeseK.text = string.Empty;
        cheeseS.text = string.Empty;
        milk.text = string.Empty;
    }

    // Add orders
    public void Add()
    {
        // Create new order and make some validations
        var ord = new Order();
        ord.Name = custName.text;
        ord.Address = custAddress.text;
        ord.Phone = custPhone.text;

        if (int.TryParse(txtDeliveryOrder.text, out int deliv))
            ord.DeliveryOrder = deliv;

        if (ValidateDate(date.text))
            ord.Date = DateTime.Parse(date.text);

        ord.CheeseK = ValidateQuantity(cheeseK.text);
        ord.CheeseS = ValidateQuantity(cheeseS.text);
        ord.Milk = ValidateQuantity(milk.text);
        if(ValidateName(ord.Name) && ValidatePhone(ord.Phone) && HasOrders(ord))
        {
            // If order passes all validations save it to db and refresh page
            if (ds.AddOrder(ord))
            {
                RefreshTodayOrders();
                RefreshAllOrders();
                FindObjectOfType<NavManager>().Back(AddEditOrderPanel);
            }
            else
                PrintMsg("Нещо се обърка !", false);
        }
    }

    // Update orders
    // Save input in case its left empty
    public void ClearInput(InputField input)
    {
        lastText = input.text;
    }

    // Update order credentials whenever given input text has been changed
    public void InputChanged(InputField input)
    {
        if (isEditor.isOn)
        {
            if (!string.IsNullOrEmpty(input.text))
            {
                // Identify which property should be updated by input tag
                switch (input.tag)
                {
                    case "InputName":
                        order.Name = input.text;
                        break;
                    case "InputAddress":
                        order.Address = input.text;
                        break;
                    case "InputPhone":
                        order.Phone = input.text;
                        break;
                    case "InputCheeseK":
                        order.CheeseK = ValidateQuantity(input.text);
                        break;
                    case "InputCheeseS":
                        order.CheeseS = ValidateQuantity(input.text);
                        break;
                    case "InputMilk":
                        order.Milk = ValidateQuantity(input.text);
                        break;
                    default:
                        PrintMsg("Нещо се обърка", false);
                        break;
                }
                // Update if the given string pass all validations
                if(ValidateName(order.Name) && ValidatePhone(order.Phone) && HasOrders(order))
                    UpdateOrder();
            }
            else
            {
                input.text = lastText;
            }
        }
    }

    // Update order credentials when user pick another date
    public void DateSelected(string date)
    {
        if (isEditor.isOn)
        {
            order.Date = DateTime.Parse(date);
            UpdateOrder();
        }
    }

    // Actual update of the order credentials
    public void UpdateOrder()
    {
        if (ds.UpdateOrder(order))
        {
            PrintMsg("Данните за поръчката са обновени.", true);
            RefreshAllOrders();
            RefreshTodayOrders();
        }
        else
            PrintMsg("Възникна грешка при обновяването !", false);
    }

    // Delete orders
    // Confirmation that user wants to delete the order
    public void Confirm(bool result)
    {
        if (result)
        {
            if (DeleteOrder())
            {
                FindObjectOfType<AudioManager>().Delete();
                FindObjectOfType<NavManager>().Back(AddEditOrderPanel);
            }
                
        }
    }

    // Confirm that order is delivered and remove it from the db
    public void ConfirmDeliver(bool result)
    {
        if (result)
        {
            if (DeleteOrder())
            {
                FindObjectOfType<AudioManager>().CashIn();
                DeliveryOrderPanel.SetActive(false);
            }
        }
    }

    // Delete order after confirmation
    private bool DeleteOrder()
    {
        if (ds.DeleteOrder(order))
        {
            RefreshAllOrders();
            RefreshTodayOrders();
            return true;
        }
        else
            PrintMsg("Възникна грешка при изтриването.", false);
            return false;
            
    }
    // Validations
    // Date validation
    private bool ValidateDate(string text)
    {
        if (DateTime.TryParse(text, out DateTime dt))
        {
            if (dt >= DateTime.Today)
                return true;
            else
                PrintMsg("Въвели сте задна дата !", false);
            return false;
        }
        else
            PrintMsg("Въвели сте невалидна дата !", false);
        return false;
    }

    // Parsing and validating quantity text inputs
    private float ValidateQuantity(string text)
    {
        if (float.TryParse(text, out float result))
            return result;
        return 0f;
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

    // Phone number validation
    // Disabled by request
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

    // Simple order validation
    private bool HasOrders(Order ord)
    {
        if ((ord.CheeseK + ord.CheeseS + ord.Milk) == 0f)
        {
            PrintMsg("Няма добавени продукти.", false);
            return false;
        }
        return true;
    }
    // Delivery 
    public void InitializeDeliveryPanel(int id)
    {
        order = ds.FindOrder(id);
        if(order != null)
        {
            orderNoMSG.text = $"Поръчка № {order.Id}";
            var total = CalcTotalCost(order.CheeseK, order.CheeseS, order.Milk);
            orderTotalCost.text = $"{total:0.00}лв.";
        }
        DeliveryOrderPanel.SetActive(true);
    }

    // Calculate total cost of the order
    private decimal CalcTotalCost(float cheeseK, float cheeseS, float milk)
    {
        var priceK = FindObjectOfType<PriceManager>().priceK;
        var priceS = FindObjectOfType<PriceManager>().priceS;
        var priceM = FindObjectOfType<PriceManager>().priceM;

        return (decimal)((cheeseK * priceK) + (cheeseS * priceS) + (milk * priceM));
    }
    // Call
    public void CallCustomer()
    {
        //Application.OpenURL("tel://"+ order.Phone);
        string phoneNum = "tel: "+ order.Phone;

        //For accessing static strings(ACTION_CALL) from android.content.Intent
        AndroidJavaClass intentStaticClass = new AndroidJavaClass("android.content.Intent");
        string actionCall = intentStaticClass.GetStatic<string>("ACTION_CALL");

        //Create Uri
        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", phoneNum);

        //Pass ACTION_CALL and Uri.parse to the intent
        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", actionCall, uriObject);

        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        try
        {
            if(FindObjectOfType<PermissionManager>().ReadyToCall())
            {
                //Start Activity
                unityActivity.Call("startActivity", intent);
            }
            else
            {
                FindObjectOfType<PermissionManager>().AskForPermission();
            }
            
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to Dial number: " + e.Message);
        }
    }

    // Log system.
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

