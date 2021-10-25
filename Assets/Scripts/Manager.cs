using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhythmGameStarter;

[DefaultExecutionOrder(-100)]
public class Manager : MonoBehaviour
{
    private static Manager m_Instance;
    public static Manager Instance
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
            Helper.DebugLog("maangers != null");
            DestroyImmediate(gameObject);
            return;
        }
        // else
        // {
        Helper.DebugLog("maangers = null");
        m_Instance = this;
        DontDestroyOnLoad(gameObject);
        // }
    }
}