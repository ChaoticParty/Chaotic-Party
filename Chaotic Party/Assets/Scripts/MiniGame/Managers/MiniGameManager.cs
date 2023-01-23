using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MiniGameManager : MonoBehaviour
{
    [Tooltip("Liste des joueurs, remplie automatiquement")] public List<PlayerController> players;
    public bool isGameDone;
    protected Dictionary<PlayerController, int> _ranking;
    [SerializeField] private GameObject[] crowns;

    public void RegisterPlayer(PlayerController player)
    {
        players.Add(player);
    }

    public void FinishTimer(bool gameDone)
    {
        
    }
    
    protected void DisplayCrown()
    {
        int winner = GetWinner();
        for (int i = 0; i < crowns.Length; i++)
        {
            crowns[i].SetActive(i == winner);
        }
    }

    protected abstract int GetWinner();

    protected abstract void OnMinigameEnd();
}