using SQLite4Unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public int DeliveryOrder { get; set; }
    public float CheeseK { get; set; }
    public float CheeseS { get; set; }
    public float Milk { get; set; }
    public DateTime Date { get; set; }
}
