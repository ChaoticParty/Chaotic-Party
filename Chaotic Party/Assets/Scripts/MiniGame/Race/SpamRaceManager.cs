using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Cinemachine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;

public class SpamRaceManager : SpamManager
{
    [FoldoutGroup("Scene Objects")]
    [FoldoutGroup("Scene Objects/Cars"), SceneObjectsOnly]
    [SerializeField] private GameObject[] cars;
    [FoldoutGroup("Scene Objects/Camera"), SceneObjectsOnly]
    [SerializeField] private CinemachineVirtualCamera raceCamera;
    [FoldoutGroup("Scene Objects/Cars"), SceneObjectsOnly]
    [SerializeField] private Transform[] raceCars;
    [FoldoutGroup("Scene Objects/Camera"), SceneObjectsOnly]
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [FoldoutGroup("Points Handler")]
    public float timeBeforeClickRegisters;
    [FoldoutGroup("Points Handler")]
    public PointsType typeAjoutPoints;
    [FoldoutGroup("Points Handler"), OnCollectionChanged(nameof(OnPointsChanged)), ListDrawerSettings(NumberOfItemsPerPage = 5)] 
    public List<Points> points;
    [ButtonGroup("Points Handler/Copy")] 
    private void CopyListToClipboard()
    {
#if UNITY_EDITOR
        Clipboard.Copy(points);
#endif
    }
    [ButtonGroup("Points Handler/Copy")] 
    private void PasteListToClipboard()
    {
#if UNITY_EDITOR
        Clipboard.TryPaste(out points);
#endif
    }
    private void OnPointsChanged(List<Points> value)
    {
        for (int i = 0; i < value.Count; i++)
        {
            Points p = value[i];
            p.points = (i + 1) * 100;
        }

        points = value;
    }
    [FoldoutGroup("Scene Objects/Colorisation"), SceneObjectsOnly]
    public List<SpriteRendererListWrapper> carsToColorise = new();
    [FoldoutGroup("Scene Objects/Colorisation"), SceneObjectsOnly]
    public List<SpriteRendererListWrapper> raceCarsToColorise = new();
    [FoldoutGroup("Scene Objects/Others"), AssetsOnly]
    public TextMeshProUGUI tmpPrefab;

    #region Events

    [FoldoutGroup("Events")] 
    public UnityEvent<Vector2, string> playerGetsPointsEvent;
    [FoldoutGroup("Events")] 
    public UnityEvent<Vector2, string> playerGetsOver500PointsEvent;
    [FoldoutGroup("Events")] 
    public UnityEvent<Vector2, string> playerLosePointsEvent;
    [FoldoutGroup("Events")] 
    public UnityEvent<Vector2> playerGets1000PointsEvents;

    #endregion

    protected override void Start()
    {
        base.Start();
        ActivateUI(false);
        //ColoriseObjectsAccordingToPlayers(ReferenceHolder.Instance.players.players, carsToColorise);
    }

    private void ActivateUI(bool activate)
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = activate ? 1 : 0;
        canvasGroup.interactable = activate;
        canvasGroup.blocksRaycasts = activate;
        //transform.GetChild(0).gameObject.SetActive(activate);
    }

    [ContextMenu("LoadMiniGame"), Button]
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
        foreach (PlayerController player in players)
        {
            player.GetComponent<SpamRaceController>().SendClicks();
        }
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

    public void SetClickText(Transform textTransform, TextMeshProUGUI text, int value, int index, out GameObject effect)
    {
        Points point = index < points.Count ? points[index] : points[^1];
        SetClickText(textTransform, text, value, point, out effect);
    }

    public void SetClickText(Transform textTransform, TextMeshProUGUI text, int value, Points point, out GameObject effect)
    {
        text.text = value.ToString();
        text.color = point.color;
        //_tmpPrefab.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        Transform tmpPrefabTransform = textTransform.transform;
        tmpPrefabTransform.localScale = point.scale;
        tmpPrefabTransform.position += Vector3.up / 3;
        tmpPrefabTransform.rotation = point.GetRotation();
        
        if (point.effect)
        {
            effect = Instantiate(point.effect, tmpPrefabTransform.position, Quaternion.identity, tmpPrefabTransform);
            return;
        }

        effect = null;
    }

    public override void Click(int playerIndex, float value, SpamButton spamButton = SpamButton.Any)
    {
        if(!isMinigamelaunched || playerIndex >= players.Count) return;
        
        nbClicks++;
        clicksArray[playerIndex] += value;
        if(spamTexts.Length > playerIndex) UpdateClickUi(playerIndex, clicksArray[playerIndex]);
        DisplayCrown();
        
        if(value == 0) return;

        if (typeAjoutPoints == PointsType.BigPoints)
        {
            
            return;
        }
        
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
        ranking = GetRanking();
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
        foreach ((PlayerController key, int value) in ranking)
        {
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
        
        ColoriseCinematicObjects(GetRankingToPlayerSo());
    }

    public void StartRace()
    {
        
        foreach (PlayerController player in players)
        {
            if(!player.gameObject.activeSelf) continue;
            SpamRaceController playerScript = player.GetComponent<SpamRaceController>();
            Transform raceCar = raceCars[players.IndexOf(player)];
            playerScript.Race(raceCar.position + Vector3.right * (5 - ranking[player]) * 30, ranking[player] == 0);
        }
        StartCoroutine(CheckCoroutines());
    }

    public IEnumerator CheckCoroutines()
    {
        bool coroutinesDone = false;
        /*while (!coroutinesDone)
        {
            foreach (PlayerController player in players)
            {
                if (player.GetComponent<SpamRaceController>()._coroutine == null) coroutinesDone = true;
                yield return null;
            }
        }*/

        yield return new WaitForSeconds(6);
        
        LoadRecap();
    }
}

public enum PointsType
{
    Continuous,
    VfxBurst,
    BigPoints
}