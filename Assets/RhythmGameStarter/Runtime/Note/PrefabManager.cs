using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : Singleton<PrefabManager>
{
    private Dictionary<string, GameObject> m_IngameObjectPrefabDict = new Dictionary<string, GameObject>();
    public GameObject[] m_IngameObjectPrefabs;

    private Dictionary<string, GameObject> m_PipePrefabDict = new Dictionary<string, GameObject>();
    public GameObject[] m_NoteLevel;

    private Dictionary<string, GameObject> m_EnemyDict = new Dictionary<string, GameObject>();
    public GameObject[] m_Enemy;

    private void Awake()
    {
        InitPrefab();
        // InitIngamePrefab();
    }

    public void InitPrefab()
    {
        for (int i = 0; i < m_NoteLevel.Length; i++)
        {
            GameObject iPrefab = m_NoteLevel[i];
            if (iPrefab == null) continue;
            string iName = iPrefab.name;
            try
            {
                m_PipePrefabDict.Add(iName, iPrefab);
            }
            catch (System.Exception)
            {
                continue;
            }
        }
        for (int i = 0; i < m_Enemy.Length; i++)
        {
            GameObject iPrefab = m_Enemy[i];
            if (iPrefab == null) continue;
            string iName = iPrefab.name;
            try
            {
                m_EnemyDict.Add(iName, iPrefab);
            }
            catch (System.Exception)
            {
                continue;
            }
        }
    }

    public void InitIngamePrefab()
    {
        // string pipe1 = ConfigKeys.m_Pipe1.ToString();
        // CreatePool(pipe1, GetPipePrefabByName(pipe1), 10);
        // string pipe0 = ConfigKeys.m_Pipe0.ToString();
        // CreatePool(pipe0, GetPipePrefabByName(pipe0), 2);
    }

    public void CreatePool(string name, GameObject prefab, int amount)
    {
        SimplePool.Preload(prefab, amount, name);
    }

    public GameObject SpawnPool(string name, Vector3 pos)
    {
        if (SimplePool.IsHasPool(name))
        {
            GameObject go = SimplePool.Spawn(name, pos, Quaternion.identity);
            return go;
        }
        else
        {
            GameObject prefab = GetPrefabByName(name);
            if (prefab != null)
            {
                SimplePool.Preload(prefab, 1, name);
                GameObject go = SpawnPool(name, pos);
                return go;
            }
        }
        return null;
    }

    public GameObject GetPrefabByName(string name)
    {
        GameObject rPrefab = null;
        if (m_IngameObjectPrefabDict.TryGetValue(name, out rPrefab))
        {
            return rPrefab;
        }
        return null;
    }

    public GameObject GetNotePrefabByName(string name)
    {
        if (m_PipePrefabDict.ContainsKey(name))
        {
            return m_PipePrefabDict[name];
        }
        return null;
    }

    public GameObject SpawnNoteLevelPool(string name, Vector3 pos)
    {
        if (SimplePool.IsHasPool(name))
        {
            GameObject go = SimplePool.Spawn(name, pos, Quaternion.identity);
            return go;
        }
        else
        {
            GameObject prefab = GetNotePrefabByName(name);
            if (prefab != null)
            {
                SimplePool.Preload(prefab, 1, name);
                GameObject go = SpawnPool(name, pos);
                return go;
            }
        }
        return null;
    }

    public GameObject GetEnemyPrefabByName(string name)
    {
        if (m_EnemyDict.ContainsKey(name))
        {
            return m_EnemyDict[name];
        }
        return null;
    }

    public GameObject SpawnEnemyPool(string name, Vector3 pos)
    {
        if (SimplePool.IsHasPool(name))
        {
            GameObject go = SimplePool.Spawn(name, pos, Quaternion.identity);
            return go;
        }
        else
        {
            GameObject prefab = GetEnemyPrefabByName(name);
            if (prefab != null)
            {
                SimplePool.Preload(prefab, 1, name);
                GameObject go = SpawnPool(name, pos);
                return go;
            }
        }
        return null;
    }

    public void DespawnPool(GameObject go)
    {
        SimplePool.Despawn(go);
    }
}
