using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MiniGameManager : MonoBehaviour
{
    [Tooltip("Liste des joueurs, remplie automatiquement")] public List<PlayerController> players;
    public bool isGameDone;

    public void RegisterPlayer(PlayerController player)
    {
        players.Add(player);
    }

    public void FinishTimer(bool gameDone)
    {
        
    }
}