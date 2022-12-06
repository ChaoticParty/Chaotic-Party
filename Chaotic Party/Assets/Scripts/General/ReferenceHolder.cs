using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceHolder : MonoBehaviour
{
    public PlayersListSO players;
    public static ReferenceHolder instance;

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        
        players ??= Resources.Load<PlayersListSO>("ScriptableObjects/Players/Players");
        DontDestroyOnLoad(gameObject);
    }
}
