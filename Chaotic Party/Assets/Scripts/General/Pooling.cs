using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pooling : MonoBehaviour
{
    public List<GameObject> poolList = new();
    public List<GameObject> poolObjectPrefabs;
    public static Pooling instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject GetPoolObject(Transform parent)
    {
        GameObject poolobject = GetPoolObject();
        poolobject.transform.SetParent(parent);
        poolobject.transform.localPosition = Vector3.zero;
        return poolobject;
    }

    public GameObject GetPoolObject()
    {
        GameObject poolObject = poolList.Count <= 0 ? Instantiate(poolObjectPrefabs[Random.Range(0, poolObjectPrefabs.Count)], transform) : poolList[0];
        poolObject.SetActive(true);
        
        if(poolList.Count > 0) poolList.RemoveAt(0);
        return poolObject;
    }

    public void ReturnToPool(GameObject poolobject)
    {
        poolobject.SetActive(false);
        poolList.Add(poolobject);
        poolobject.transform.SetParent(transform);
    }

    [Button]
    public void RandomizeList()
    {
        List<GameObject> newList = new();
        int size = poolList.Count;
        for (int i = 0; i < size; i++)
        {
            GameObject go = poolList[Random.Range(0, poolList.Count)];
            poolList.Remove(go);
            newList.Add(go);
        }

        poolList = new List<GameObject>(newList);
    }
}
