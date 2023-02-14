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
    [SerializeField] [Tooltip("Dur�e du minijeu")] private float timer;
    [HideInInspector] public TimerManager timerManager;
    public bool isGameDone;
    protected Dictionary<PlayerController, int> _ranking;
    [SerializeField] protected GameObject[] crowns;
    [SerializeField] public bool isMinigamelaunched;

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

    public void AddPoints()
    {
        PlayersListSO playersList = ReferenceHolder.Instance.players;
        for (int i = 0; i < players.Count; i++)
        {
            playersList.players[i].points += 4 - _ranking[players[i]];
        }
    }

    public void SetCurrentRanking()
    {
        PlayersListSO playersList = ReferenceHolder.Instance.players;
        List<PlayerSO> playersData = new(playersList.players);
        for (int i = 0; i < playersData.Count; i++)
        {
            int currentRank = playersData.Count - 1;
            PlayerSO playerToCheck = playersData[0];
            foreach (PlayerSO playerData in playersData)
            {
                if(playerToCheck == playerData) continue;
                if (playerToCheck.points > playerData.points)
                {
                    currentRank--;
                }
            }

            playerToCheck.ranking = currentRank;
            playersData.RemoveAt(0);
        }
    }
}