using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RhythmGameStarter;

public class TestMove : MonoBehaviour
{
    private void OnEnable()
    {
        EventString.AddListener("inter_cd_time", TestEvent);
    }

    private void OnDisable()
    {
        EventString.RemoveListener("inter_cd_time", TestEvent);
    }

    private void OnDestroy()
    {
        EventString.RemoveListener("inter_cd_time", TestEvent);
    }

    public void TestEvent()
    {
        Helper.DebugLog("ALo alo alo");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            EventString.CallEvent("inter_cd_time");
        }
        // Helper.DebugLog("Next Add: " + ProfileManager.MyProfile.m_InterTime.GetTimeToNextAdd(1, ""));
    }
}

public class TestTest
{
    public BigNumber aaa;
}