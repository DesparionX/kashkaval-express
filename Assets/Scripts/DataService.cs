using SQLite4Unity3d;
using UnityEngine;
using UnityEngine.Networking;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Globalization;
using System.Linq;

public class DataService
{

	private SQLiteConnection _connection;
	private string cheeseKPrice = "cheeseKPrice";
	private string cheeseSPrice = "cheeseSPrice";
	private string milkPrice = "milkPrice";

	public DataService(string DatabaseName)
	{
#if UNITY_EDITOR
		var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            //var handler = loadDb.downloadHandler; 
			// then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);


#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
		_connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
		Debug.Log("Final PATH: " + dbPath);
	}
	public void CreateDB()
	{
		//_connection.DropTable<Customer>();
		//_connection.DropTable<Order>();

		_connection.CreateTable<Customer>();
        _connection.CreateTable<Order>();
        if (!PricesExist())
        {
			PlayerPrefs.SetFloat(cheeseKPrice, 0f);
            PlayerPrefs.SetFloat(cheeseSPrice, 0f);
			PlayerPrefs.SetFloat(milkPrice, 0f);
		}
		
	}
	private bool PricesExist()
    {
		return PlayerPrefs.HasKey(cheeseKPrice) &&
			PlayerPrefs.HasKey(cheeseSPrice) &&
			PlayerPrefs.HasKey(milkPrice);

	}

    // Customer operations.
	public bool AddCustomer(Customer customer)
    {
		var cust = _connection.Table<Customer>().Where(c => c.Id == customer.Id).FirstOrDefault();
		if(cust == null)
        {
            try
            {
                _connection.Insert(customer);
                return true;
            }
            catch (Exception err)
            {
                Debug.LogError(err.Message);
            }
        }
        return false;
    }
	public IEnumerable<Customer> GetCustomers(string filter)
    {
        var dbList = _connection.Table<Customer>().OrderBy(c => c.Name).ToList();
        return dbList.Where(c => c.Name.ToLower().StartsWith(filter, true, CultureInfo.InvariantCulture));
    }
	public Customer FindCustomer(int id)
    {
        try
        {
			return _connection.Table<Customer>().Where(c => c.Id == id).First();
		}
        catch (Exception err)
        {
			Debug.LogError(err.Message);
        }
		return null;
    }
    public bool UpdateCustomer(Customer cust)
    {
        if (cust != null)
        {
            try
            {
                _connection.Update(cust);
                return true;
            }
            catch(Exception err)
            {
                Debug.LogError(err.Message);
            }
        }
        return false;
    }
    public bool DeleteCustomer(Customer cust)
    {
        var customer = _connection.Table<Customer>().Where(c => c.Id == cust.Id).FirstOrDefault();
        if(customer != null)
        {
            try
            {
                _connection.Delete(customer);
                return true;
            }
            catch(Exception err)
            {
                Debug.LogError(err.Message);
            }
        }
        return false;
    }

    // Orders operations.
    public IEnumerable<Order> GetAllOrders()
    {
        // Retrieve all orders ordered by date.
        return _connection.Table<Order>().ToList().OrderBy(o => o.Date);
    }
    public IEnumerable<Order> GetOrdersForToday()
    {
        // Retrieve orders for today ordered by delivery order.
        return _connection.Table<Order>().Where(o => o.Date == DateTime.Today).ToList().OrderBy(o => o.DeliveryOrder);
    }

    public Order FindOrder(int id)
    {
        try
        {
            return _connection.Table<Order>().Where(o => o.Id == id).First();
        }
        catch (Exception err)
        {
            Debug.LogError(err.Message);
        }
        return null;
    }
    public bool AddOrder(Order order)
    {
        var ord = _connection.Table<Order>().Where(o => o.Id == order.Id).FirstOrDefault();
        if(ord == null)
        {
            try
            {
                _connection.Insert(order);
                return true;
            }
            catch (Exception err)
            {
                Debug.LogError(err.Message);
            }
        }
        return false;
    }
    public bool UpdateOrder(Order order)
    {
        if(order != null)
        {
            try
            {
                _connection.Update(order);
                return true;
            }
            catch(Exception err)
            {
                Debug.LogError(err.Message);
            }
        }
        return false;
    }
    public bool DeleteOrder(Order order)
    {
        var ord = _connection.Table<Order>().Where(o => o.Id == order.Id).FirstOrDefault();
        if(ord != null)
        {
            try
            {
                _connection.Delete(ord);
                return true;
            }
            catch(Exception err)
            {
                Debug.LogError(err.Message);
            }
        }
        return false;
    }
    public bool HaveOrdersForTomorrow()
    {
        var tomorrow = DateTime.Today.Date.AddDays(1);
        return _connection.Table<Order>().Where(o => o.Date == tomorrow).Any();
    }
}