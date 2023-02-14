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
    [Header("Timer")]
    [SerializeField] [Tooltip("Durée du minijeu")] private float timer;
    [SerializeField] [Tooltip("Manager du chrono")] private TimerManager timerManager;
    public bool isGameDone;
    protected Dictionary<PlayerController, int> _ranking;
    [SerializeField] private GameObject[] crowns;
    public bool isMinigamelaunched;

    public void RegisterPlayer(PlayerController player)
    {
        players.Add(player);
    }

    public abstract void FinishTimer();

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
        timerManager.SetTimer(timer);
        isMinigamelaunched = true;
    }

    public void LoadRecap()
    {
        SceneManager.LoadScene("RecapScore");
    }
}