using System;
using System.Collections.Generic;
using System.Globalization;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;

public class SpamRaceManager : SpamManager
{
    [SerializeField] private GameObject[] cars;
    public float timer;
    [NonSerialized] public float currentTimer;
    [SerializeField] private Image timerImage;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private GameObject[] crowns;
    [SerializeField] private CinemachineVirtualCamera raceCamera;
    [SerializeField] private Transform[] raceCars;
    [SerializeField] private CinemachineTargetGroup targetGroup;
    private Dictionary<PlayerController, int> _ranking;

    #region Events

    [Space, Header("Events")] 
    public UnityEvent<Vector2, string> playerGetsPointsEvent;
    public UnityEvent<Vector2, string> playerLosePointsEvent;
    public UnityEvent<Vector2> playerGets1000PointsEvents;

    #endregion

    protected new void Start()
    {
        base.Start();
        targetGroup.m_Targets = new CinemachineTargetGroup.Target[players.Count];
        for (int i = 0; i < spamTexts.Length; i++)
        {
            if (i >= players.Count)
            {
                cars[i].SetActive(false);
                raceCars[i].gameObject.SetActive(false);
            }
            else
            {
                targetGroup.m_Targets[i].target = raceCars[i];
                targetGroup.m_Targets[i].weight = 1;
            }
        }

        currentTimer = timer;
    }

    private void Update()
    {
        if(isGameDone) return;
        
        currentTimer -= Time.deltaTime;

        if (currentTimer <= 0)
        {
            isGameDone = true;
            winText.text = "Joueur " + (GetWinner() + 1) + " a gagné!";
            //winText.gameObject.SetActive(true);
            GetComponent<PlayableDirector>().Play();

            

            //raceCamera.Priority = 100;
        }
        else
        {
            timerImage.fillAmount = currentTimer / timer;
        }
    }

    public override void Click(int playerIndex, float value, SpamButton spamButton = SpamButton.Any)
    {
        if(playerIndex >= players.Count) return;
        
        nbClicks++;
        clicksArray[playerIndex] += value;
        if(spamTexts.Length > playerIndex) spamTexts[playerIndex].text = clicksArray[playerIndex].ToString(CultureInfo.CurrentCulture);
        DisplayCrown();
        
        string valueToDisplay = value.ToString(CultureInfo.InvariantCulture);
        if (value >= 0) valueToDisplay = "+" + valueToDisplay;
        if(value >= 0) playerGetsPointsEvent.Invoke(players[playerIndex].transform.position, valueToDisplay);
        else playerLosePointsEvent.Invoke(players[playerIndex].transform.position, valueToDisplay);
        
        if (clicksArray[playerIndex] % 1000 == 0)
        {
            playerGets1000PointsEvents.Invoke(players[playerIndex].transform.position);
            CameraController.Shake();
        }
    }

    private void DisplayCrown()
    {
        int winner = GetWinner();
        for (int i = 0; i < crowns.Length; i++)
        {
            crowns[i].SetActive(i == winner);
        }
    }

    private int GetWinner()
    {
        int winnerIndex = 0;
        float winValue = clicksArray[0];
        for (int i = 0; i < clicksArray.Length; i++)
        {
            if (clicksArray[i] > winValue)
            {
                winValue = clicksArray[i];
                winnerIndex = i;
            }
        }

        return winnerIndex;
    }

    private Dictionary<PlayerController, int> GetRanking()
    {
        Dictionary<PlayerController, int> ranking = new();
        for (int i = 0; i < players.Count; i++)
        {
            int currentRanking = 0;
            for (int j = 0; j < players.Count; j++)
            {
                if (i != j)
                {
                    if (clicksArray[i] < clicksArray[j])
                    {
                        currentRanking++;
                    }
                }
            }
            ranking.Add(players[i], currentRanking);
        }

        return ranking;
    }

    public void ReplaceCars()
    {
        _ranking = GetRanking();
        foreach ((PlayerController key, int value) in _ranking)
        {
            Debug.Log(key.index + " " + value);
            if (value == 0)
            {
                Transform winnerTransform = key.transform;
                raceCamera.Follow = winnerTransform;
                raceCamera.LookAt = winnerTransform;
            }
        }
        
        raceCamera.Priority = 100;
        foreach (PlayerController player in players)
        {
            Transform transform1;
            Transform raceCar = raceCars[players.IndexOf(player)];
            (transform1 = player.transform).SetParent(raceCar);
            transform1.localPosition = Vector3.zero;
        }
    }

    public void StartRace()
    {
        
        foreach (PlayerController player in players)
        {
            SpamRaceController playerScript = player.GetComponent<SpamRaceController>();
            Transform raceCar = raceCars[players.IndexOf(player)];
            playerScript.Race(raceCar.position + Vector3.right * (5 - _ranking[player]) * 10);
        }
    }
}
