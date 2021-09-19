using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTime", menuName = "EnemyTime/Song")]
public class EnemyTime : ScriptableObject
{
    public List<TimeInfo> m_TimeInfo;
}

[System.Serializable]
public struct TimeInfo
{
    public float m_Start;
    public float m_End;

    public bool IsInRange(float _songTime)
    {
        if (_songTime >= m_Start && _songTime <= m_End)
        {
            return true;
        }

        return false;
    }
}