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
    public List<PlayerSO> playersSo = new List<PlayerSO>();

    private void Awake()
    {
        miniGameData ??= Resources.Load<MiniGameData>("ScriptableObjects/MiniGameData");
        players ??= Resources.Load<PlayersListSO>("ScriptableObjects/Players/Players");
        foreach (PlayerSO player in players.players)
        {
            playersSo.Add(player);
        }
        DontDestroyOnLoad(gameObject);
    }
}
