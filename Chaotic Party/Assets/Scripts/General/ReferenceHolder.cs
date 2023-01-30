using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReferenceHolder : MonoBehaviour
{
    public PlayersListSO players;
    private static ReferenceHolder instance;
    public static ReferenceHolder Instance
    {
        get
        {
            if (instance)
            {
                return instance;
            }
            else
            {
                return instance = new GameObject("SingletonHolder").AddComponent<ReferenceHolder>();
            }
        }
    }
    public GameObject oldEventSystem;

    private void Awake()
    {
        
        players ??= Resources.Load<PlayersListSO>("ScriptableObjects/Players/Players");
        DontDestroyOnLoad(gameObject);
    }
}
