using System;
using SQLite4Unity3d;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Android;

//
// This script is found on internet and not done by me, I simply edit it for my needings.
// 
public class NotificationManager : MonoBehaviour
{
    static List<string> Handled_Ids = new List<string>();


    string _Channel_Id = "notify_daily_reminder";
    string _Icon_Small = "notify_icon_small"; 
    string _Icon_Large = "notify_icon_large"; 
    string _Channel_Title = "Дневни напомняния";
    string _Channel_Description = "Напомняния дали има поръчки за следващия ден.";

    DataService ds;
    void Start()
    {
        Debug.Log("NotificationManager: Start");

        //connect DataService
        ds = new DataService(PlayerPrefs.GetString("DB"));

        //always remove any currently displayed notifications
        AndroidNotificationCenter.CancelAllDisplayedNotifications();


        //check if this was openened from a notification click
        var notification_intent_data = AndroidNotificationCenter.GetLastNotificationIntent();

        //this is just for debugging purposes
        if (notification_intent_data != null)
        {
            Debug.Log("notification_intent_data.Id: " + notification_intent_data.Id);
            Debug.Log("notification_intent_data.Channel: " + notification_intent_data.Channel);
            Debug.Log("notification_intent_data.Notification: " + notification_intent_data.Notification);
        }


        //if the notification intent is not null and we have not already seen this notification id, do something
        //using a static List to store already handled notification ids
        if (notification_intent_data != null && NotificationManager.Handled_Ids.Contains(notification_intent_data.Id.ToString()) == false)
        {
            NotificationManager.Handled_Ids.Add(notification_intent_data.Id.ToString());

            //this logic assumes only one type of notification is shown     
            if (ds.HaveOrdersForTomorrow())
                FindObjectOfType<NavManager>().SwapPanel("Orders");
            return;
        }
        else
        {
            Debug.Log("notification_intent_data is null or already handled");
        }

        this.Setup_Notifications();
    }


    internal void Setup_Notifications()
    {
        Debug.Log("NotificationsManager: Setup_Notifications");


        //initialize the channel
        this.Initialize();


        //schedule the next notification
        this.Schedule_Daily_Reminder();
    }


    void Initialize()
    {
        Debug.Log("NotificationManager: Initialize");

        //add our channel
        //a channel can be used by more than one notification
        //you do not have to check if the channel is already created, Android OS will take care of that logic
        var androidChannel = new AndroidNotificationChannel(this._Channel_Id, this._Channel_Title, this._Channel_Description, Importance.Default);
        AndroidNotificationCenter.RegisterNotificationChannel(androidChannel);
    }



    void Schedule_Daily_Reminder()
    {
        Debug.Log("NotificationManager: Schedule_Daily_Reminder");


        //since this is the only notification I have, I will cancel any currently pending notifications
        //if I create more types of notifications, additional logic will be needed
        AndroidNotificationCenter.CancelAllScheduledNotifications();


        //create new schedule
        string title = ds.HaveOrdersForTomorrow() ? "Напомняне !" : "Спи спокойно !";
        string body = ds.HaveOrdersForTomorrow() ? "Имаш поръчки за утре !" : "Нямаш поръчки за утре.";

        //show at the specified time 
        //you could also always set this a certain amount of hours ahead, since this code resets the schedule
        DateTime delivery_time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 0, 0);
        if (delivery_time < DateTime.Now)
        {
            //if in the past (ex: this code runs at 18:00 PM), push delivery date forward 1 day
            delivery_time = delivery_time.AddDays(1);
        }
        Debug.Log("Delivery Time: " + delivery_time.ToString());


        //you currently do not need the notification id
        //if you had multiple notifications, you could store this and use it to cancel a specific notification
        var scheduled_notification_id = AndroidNotificationCenter.SendNotification(
            new AndroidNotification()
            {
                Title = title,
                Text = body,
                FireTime = delivery_time,
                SmallIcon = this._Icon_Small,
                LargeIcon = this._Icon_Large
            },
            this._Channel_Id);
    }
}
