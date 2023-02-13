using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class MiniGameManager : MonoBehaviour
{
    [Tooltip("Liste des joueurs, remplie automatiquement")] public List<PlayerController> players;
    [SerializeField] [Tooltip("Animator de compteur de debut du minijeu")] private Animator beginAnimator;
    public bool isGameDone;
    protected Dictionary<PlayerController, int> _ranking;
    [SerializeField] private GameObject[] crowns;
    protected bool isMinigamelaunched;

    public void RegisterPlayer(PlayerController player)
    {
        players.Add(player);
    }

    public void FinishTimer(bool gameDone)
    {
        
    }

    protected void BeginTimer()
    {
        beginAnimator.enabled = true;
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

    protected virtual void OnMinigameEnd()
    {
        isMinigamelaunched = false;
    }

    public virtual void StartMiniGame()
    {
        isMinigamelaunched = true;
    }

    public void LoadRecap()
    {
        SceneManager.LoadScene("RecapScore");
    }
}