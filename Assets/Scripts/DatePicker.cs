using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatePicker : MonoBehaviour
{
	private static DateTime selectedDate = DateTime.Now;

	class DateCallback : AndroidJavaProxy
	{
		public DateCallback() : base("android.app.DatePickerDialog$OnDateSetListener") { }
		void onDateSet(AndroidJavaObject view, int year, int monthOfYear, int dayOfMonth)
		{
			selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
			FindObjectOfType<DatePicker>().ChangeDate();
		}
	}
    public void ChangeDate()
    {
		this.transform.Find("Text").GetComponent<Text>().text = selectedDate.ToShortDateString();
		FindObjectOfType<OrdersManager>().DateSelected(selectedDate.ToShortDateString());
    }
    public void ShowDTP()
    {
		var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
		{
			new AndroidJavaObject("android.app.DatePickerDialog", activity, new DateCallback(), selectedDate.Year, selectedDate.Month - 1, selectedDate.Day).Call("show");
		}));
	}
}
