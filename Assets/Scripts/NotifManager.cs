using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RhythmGameStarter;
using Unity.Notifications.Android;

public class NotifManager : MonoBehaviour
{
    private static NotifManager m_Instance;
    public static NotifManager Instance
    {
        get
        {
            return m_Instance;
        }
    }
    public void Awake()
    {
        if (m_Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        m_Instance = this;
        DontDestroyOnLoad(gameObject);

        CreateChannel();

        if (!PlayerPrefs.HasKey("PushNoti"))
        {
            PlayerPrefs.SetInt("PushNoti", 1);
            SendNotiD1(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour + 3, DateTime.Now.Minute, DateTime.Now.Second));

            SendNotiD2_12h(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, 12, 00, 00));
            SendNotiD2_20h(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, 20, 00, 00));

            SendNotiD3(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 2, 12, 00, 00));
            SendNotiD3(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 2, 20, 00, 00));

            SendNotiD1(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 3, 12, 00, 00));

            SendNotiD3(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 4, 20, 00, 00));

            SendNotiD1(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 5, 12, 00, 00));

            SendNotiD3(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 6, 20, 00, 00));
        }
    }

    public void CreateChannel()
    {
        var c = new AndroidNotificationChannel()
        {
            Id = "Default Channel",
            Name = "Default Noti Channel",
            Importance = Importance.High,
            Description = "Generic notifications",
            CanBypassDnd = true,
            CanShowBadge = true,
            EnableLights = true,
            EnableVibration = true,
            LockScreenVisibility = LockScreenVisibility.Private,
        };
        AndroidNotificationCenter.RegisterNotificationChannel(c);
    }

    public void SendNotiD1(DateTime _date)
    {
        var notification = new AndroidNotification();
        notification.Title = "Music Battle";

        notification.Text = "Girlfriend is waiting for you!!!";
        notification.Text += System.Environment.NewLine;
        notification.Text = "-> Challenge to love from her. Sing now!";

        notification.SmallIcon = "icon";
        notification.LargeIcon = "icon_0";

        notification.FireTime = _date;

        AndroidNotificationCenter.SendNotification(notification, "Default Channel");
    }

    public void SendNotiD2_12h(DateTime _date)
    {
        var notification = new AndroidNotification();
        notification.Title = "Gangster Master";

        notification.Text = "Not a bad boy!";
        notification.Text += System.Environment.NewLine;
        notification.Text = "-> Ready for return the music battle?";

        notification.SmallIcon = "icon";
        notification.LargeIcon = "icon_0";

        notification.FireTime = _date;

        AndroidNotificationCenter.SendNotification(notification, "Default Channel");
    }

    public void SendNotiD2_20h(DateTime _date)
    {
        var notification = new AndroidNotification();
        notification.Title = "Gangster Master";

        notification.Text = "You are a SHY boy?";
        notification.Text += System.Environment.NewLine;
        notification.Text = "-> Defeat all by your voice!";

        notification.SmallIcon = "icon";
        notification.LargeIcon = "icon_0";

        notification.FireTime = _date;

        AndroidNotificationCenter.SendNotification(notification, "Default Channel");
    }

    public void SendNotiD3(DateTime _date)
    {
        var notification = new AndroidNotification();
        notification.Title = "Gangster Master";

        notification.Text = "Forever Love";
        notification.Text += System.Environment.NewLine;
        notification.Text = "-> Music Battle On Air!";

        notification.SmallIcon = "icon";
        notification.LargeIcon = "icon_0";

        notification.FireTime = _date;

        AndroidNotificationCenter.SendNotification(notification, "Default Channel");
    }
}
