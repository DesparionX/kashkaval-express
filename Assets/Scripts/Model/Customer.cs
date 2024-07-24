using SQLite4Unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Customer
{
    [PrimaryKey, AutoIncrement, Unique]
    public int Id { get; set; }
    public int DeliveryOrder { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
}
