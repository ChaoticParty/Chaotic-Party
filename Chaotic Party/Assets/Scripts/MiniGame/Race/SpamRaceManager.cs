using System;
using System.Collections;
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
    //public float timer;
    [NonSerialized] public float currentTimer;
    [SerializeField] private Image timerImage;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private CinemachineVirtualCamera raceCamera;
    [SerializeField] private Transform[] raceCars;
    [SerializeField] private CinemachineTargetGroup targetGroup;
    private List<Coroutine> _coroutines = new List<Coroutine>();
    public bool launchFromEditor;
    public float timeBeforeClickRegister;
    public PointsType typeAjoutPoints;
    public TextMeshProUGUI tmpPrefab;

    #region Events

    [Space, Header("Events")] 
    public UnityEvent<Vector2, string> playerGetsPointsEvent;
    public UnityEvent<Vector2, string> playerGetsOver500PointsEvent;
    public UnityEvent<Vector2, string> playerLosePointsEvent;
    public UnityEvent<Vector2> playerGets1000PointsEvents;

    #endregion

    protected override void Start()
    {
        base.Start();
        ActivateUI(false);
        currentTimer = timer;
    }

    private void ActivateUI(bool activate)
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = activate ? 1 : 0;
        canvasGroup.interactable = activate;
        canvasGroup.blocksRaycasts = activate;
        //transform.GetChild(0).gameObject.SetActive(activate);
    }

    [ContextMenu("LoadMiniGame")]
    public override void LoadMiniGame()
    {
        base.LoadMiniGame();
        ActivateUI(true);
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
    }

    public override void FinishTimer()
    {
        isMinigamelaunched = false;
        isGameDone = true;
        OnMinigameEnd();
    }

    [ContextMenu("StartMiniGame")]
    public override void StartMiniGame()
    {
        base.StartMiniGame();
    }

    private void Update()
    {
        if(!isMinigamelaunched || isGameDone) return;


        
        /*currentTimer -= Time.deltaTime;

        if (currentTimer <= 0)
        {
            isGameDone = true;
            winText.text = "Joueur " + (GetWinner() + 1) + " a gagné!";
            //winText.gameObject.SetActive(true);
            Debug.Log("onend");
            OnMinigameEnd();

            

            //raceCamera.Priority = 100;
        }
        else
        {
            timerImage.fillAmount = currentTimer / timer;
        }*/
    }

    public override void Click(int playerIndex, float value, SpamButton spamButton = SpamButton.Any)
    {
        if(!isMinigamelaunched || playerIndex >= players.Count) return;
        
        nbClicks++;
        clicksArray[playerIndex] += value;
        if(spamTexts.Length > playerIndex) UpdateClickUi(playerIndex, clicksArray[playerIndex]);
        DisplayCrown();
        
        if(value == 0 || typeAjoutPoints == PointsType.BigPoints) return;
        
        string valueToDisplay = value.ToString(CultureInfo.InvariantCulture);
        if (value >= 0) valueToDisplay = "+" + valueToDisplay;
        if(value >= 500) playerGetsOver500PointsEvent.Invoke(players[playerIndex].transform.position, valueToDisplay);
        else if(value >= 0) playerGetsPointsEvent.Invoke(players[playerIndex].transform.position, valueToDisplay);
        else playerLosePointsEvent.Invoke(players[playerIndex].transform.position, valueToDisplay);
        
        if (clicksArray[playerIndex] % 1000 == 0)
        {
            playerGets1000PointsEvents.Invoke(players[playerIndex].transform.position);
            CameraController.Shake();
        }
    }

    public void UpdateClickUi(int playerIndex, float value, bool valueToAdd = false)
    {
        if (valueToAdd) value += Convert.ToInt32(spamTexts[playerIndex].text.Replace("+", ""));
        spamTexts[playerIndex].text = value.ToString(CultureInfo.CurrentCulture);
    }

    public IEnumerator SendPointToTotal(TextMeshProUGUI pointsObject, int playerIndex, float value)
    {
        Transform pointsTransform = pointsObject.transform;
        Transform spamTextTransform = spamTexts[playerIndex].transform;
        while (Vector3.Distance(pointsTransform.position, spamTextTransform.position) > 0.1f)
        {
            pointsTransform.position = Vector3.Lerp(pointsTransform.position,
                spamTextTransform.position, Time.deltaTime * 10);
            pointsTransform.localScale =
                Vector3.Lerp(pointsTransform.localScale, Vector3.one / 2, Time.deltaTime * 10);
            yield return null;
        }
        spamTextTransform.localScale = Vector3.one * 2;
        yield return null;
        if(pointsObject.gameObject) Destroy(pointsObject.gameObject);
        Click(playerIndex, value);
        yield return null;
        spamTextTransform.localScale = Vector3.one;
    }

    protected override int GetWinner()
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

    protected override void OnMinigameEnd()
    {
        _ranking = GetRanking();
        AddPoints();
        SetCurrentRanking();
        GetComponent<PlayableDirector>().Play();
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
            if(!player.gameObject.activeSelf) continue;
            SpamRaceController playerScript = player.GetComponent<SpamRaceController>();
            Transform raceCar = raceCars[players.IndexOf(player)];
            _coroutines.Add(playerScript.Race(raceCar.position + Vector3.right * (5 - _ranking[player]) * 10));
        }
        StartCoroutine(CheckCoroutines());
    }

    public IEnumerator CheckCoroutines()
    {
        bool coroutinesDone = false;
        while (!coroutinesDone)
        {
            foreach (PlayerController player in players)
            {
                if (player.GetComponent<SpamRaceController>()._coroutine == null) coroutinesDone = true;
                yield return null;
            }
        }

        yield return new WaitForSeconds(1);
        
        LoadRecap();
    }
}

public enum PointsType
{
    Continuous,
    VfxBurst,
    BigPoints
}