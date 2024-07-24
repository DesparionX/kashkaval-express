using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class CreateDBScript : MonoBehaviour
{

	void Start()
	{
		StartSync();
	}

	private void StartSync()
	{
        try
        {
			var ds = new DataService("KashkavalExpressDB.db");
			ds.CreateDB();
		}
		catch(Exception err)
        {
			Debug.LogError(err.ToString());
        }
	}
}
