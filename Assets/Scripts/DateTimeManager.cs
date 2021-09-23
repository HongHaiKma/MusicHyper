using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

public class DateTimeManager : MonoBehaviour {
    private static DateTimeManager m_Instance;
    public static DateTimeManager Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new GameObject().AddComponent<DateTimeManager>();
            }
            return m_Instance;
        }
    }
    public DateTime m_GoogleDateTime = DateTime.Now;
    public static DateTime Now { get { return Instance.GetDateTimeNow(); } }
    public bool IsConnectedTimeToNetwork = false;
    private void Awake() {
        m_Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadTimeCurrent();
    }
    private void Update() {
        m_GoogleDateTime = m_GoogleDateTime.AddSeconds(Time.deltaTime);
    }
    public void LoadTimeCurrent() {
        StartCoroutine(GetWorldAsynDateTimeNow());
    }
    public IEnumerator GetWorldAsynDateTimeNow() {
        UnityWebRequest myHttpWebRequest = UnityWebRequest.Get("http://www.google.com");
        yield return myHttpWebRequest.SendWebRequest();
        if (myHttpWebRequest.isNetworkError) {
            Debug.Log("Get date time error. ");
            IsConnectedTimeToNetwork = false;
            m_GoogleDateTime = System.DateTime.Now; //In case something goes wrong. 
        } else {
            IsConnectedTimeToNetwork = true;
            string netTime = myHttpWebRequest.GetResponseHeader("date");
            m_GoogleDateTime = System.DateTime.ParseExact(netTime,
                        "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                        CultureInfo.InvariantCulture.DateTimeFormat,
                        DateTimeStyles.AdjustToUniversal);
            //TimeZone localZone = TimeZone.CurrentTimeZone;
            m_GoogleDateTime = m_GoogleDateTime.ToLocalTime();
            //Debug.Log("Date " + m_GoogleDateTime.ToLongDateString() + " Time " + m_GoogleDateTime.ToLongTimeString());
        }
    }
    public DateTime GetDateTimeNow() {
        return m_GoogleDateTime;
    }
}
