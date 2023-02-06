using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReferenceHolder : MonoBehaviour
{
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
    public PlayersListSO players;
    public MiniGameData miniGameData;
    public GameObject oldEventSystem;

    private void Awake()
    {
        miniGameData ??= Resources.Load<MiniGameData>("ScriptableObjects/MiniGameData");
        miniGameData.currentMiniGameIndex = 0;
        players ??= Resources.Load<PlayersListSO>("ScriptableObjects/Players/Players");
        DontDestroyOnLoad(gameObject);
    }
}
