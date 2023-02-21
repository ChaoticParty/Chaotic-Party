using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public abstract class MiniGameManager : MonoBehaviour
{
    [Tooltip("Liste des joueurs, remplie automatiquement")] public List<PlayerController> players;
    [SerializeField] [Tooltip("Animator de compteur de debut du minijeu")] private Animator beginAnimator;
    [Header("Timer")]
    [SerializeField] [Tooltip("Dur�e du minijeu")] protected float timer;
    [HideInInspector] public TimerManager timerManager;
    public bool isGameDone;
    protected Dictionary<PlayerController, int> ranking;
    [SerializeField] protected GameObject[] crowns;
    [SerializeField] public bool isMinigamelaunched;
    private static readonly int Begin = Animator.StringToHash("Begin");

    public virtual void LoadMiniGame()
    {
        BeginTimer();
        //timerManager ??= FindSceneTimerManager();
    }

    private TimerManager FindSceneTimerManager()
    {
        TimerManager[] timerManagers = FindObjectsOfType<TimerManager>();
        foreach (TimerManager manager in timerManagers)
        {
            if (manager.gameObject.scene == gameObject.scene) return manager;
        }

        return null;
    }

    public void RegisterPlayer(PlayerController player)
    {
        players.Add(player);
    }

    public abstract void FinishTimer();

    protected void BeginTimer()
    {
        beginAnimator.SetTrigger(Begin);
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
    protected abstract Dictionary<PlayerController, int> GetRanking();

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
            playersList.players[i].points += 4 - ranking[players[i]];
        }
    }

    public void SetCurrentRanking()
    {
        PlayersListSO playersList = ReferenceHolder.Instance.players;

        List<PlayerSO> playersData = playersList.players.OrderBy(so => so.points).ToList();
        foreach (PlayerSO playerSo in playersData)
        {
            playerSo.ranking = 3 - playersData.IndexOf(playerSo);
        }
    }

    protected void ColoriseObjectsAccordingToPlayers(List<PlayerSO> playerSos, List<SpriteRendererListWrapper> objectsToColorise)
    {
        for (int i = 0; i < playerSos.Count; i++)
        {
            PlayerSO playerSo = playerSos[i];
            foreach (SpriteRenderer spriteRenderer in objectsToColorise[i].list)
            {
                spriteRenderer.color = Color.Lerp(playerSo.color, Color.gray, 0.5f);
            }
        }
    }

    protected void ColoriseObjectsAccordingToPlayers(List<PlayerSO> playerSos, List<ImageListWrapper> objectsToColorise)
    {
        for (int i = 0; i < playerSos.Count; i++)
        {
            PlayerSO playerSo = playerSos[i];
            foreach (Image image in objectsToColorise[i].list)
            {
                image.color = playerSo.color;
            }
        }
    }

    protected List<PlayerController> RankingToList()
    {
        List<PlayerController> playerControllers = new List<PlayerController>(4);

        // Set taille liste
        foreach (KeyValuePair<PlayerController,int> _ in ranking)
        {
            playerControllers.Add(null);
        }
        foreach ((PlayerController playerController, int i) in ranking)
        {
            playerControllers[i] = playerController;
        }

        return playerControllers;
    }

    protected List<PlayerSO> PlayerControllersToPlayerSos(List<PlayerController> playerControllers)
    {
        List<PlayerSO> orderedPlayerSos = new List<PlayerSO>();
        
        List<PlayerSO> playerSos = ReferenceHolder.Instance.players.players;
        
        for (int i = 0; i < playerControllers.Count; i++)
        {
            orderedPlayerSos.Add(playerSos[players.IndexOf(playerControllers[i])]);
        }

        return orderedPlayerSos;
    }

    protected List<PlayerSO> GetRankingToPlayerSo()
    {
        return PlayerControllersToPlayerSos(RankingToList());
    }
}

[Serializable]
public class SpriteRendererListWrapper
{
    public List<SpriteRenderer> list;
}

[Serializable]
public class ImageListWrapper
{
    public List<Image> list;
}