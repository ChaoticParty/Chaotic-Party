using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PhotoMinionManager : MiniGameManager
{
    public CanvasGroup uiCanvasGroup;
    public PhotoMinionOverlord overlord;
    [Range(0, 60)]
    public float minTimerBetweenPictures;
    [Range(0, 60)]
    public float maxTimerBetweenPictures;
    [Range(0, 100)]
    public float overlordRepositioningChances = 100;

    private float _remainingTimeBeforeNextPicture;
    public GameObject picImage;
    public float timeToWait;
    private List<int> _scores = new();
    public List<TextMeshProUGUI> scoreTexts;

    private WaitForSecondsRealtime _waitSeconds;

    protected void Start()
    {
        ActivateUI(false);
        //ColoriseObjectsAccordingToPlayers(ReferenceHolder.Instance.players.players, carsToColorise);
    }

    private void ActivateUI(bool activate)
    {
        uiCanvasGroup.alpha = activate ? 1 : 0;
        uiCanvasGroup.interactable = activate;
        uiCanvasGroup.blocksRaycasts = activate;
    }

    [ContextMenu("LoadMiniGame")]
    public override void LoadMiniGame()
    {
        overlord ??= FindObjectOfType<PhotoMinionOverlord>();
        _waitSeconds = new WaitForSecondsRealtime(timeToWait / 3);
        foreach (PlayerController player in players)
        {
            _scores.Add(0);
            UpdatePlayerScoreUI(players.IndexOf(player));
        }

        for (int i = 0; i < 4 - players.Count; i++)
        {
            scoreTexts[3-i].transform.parent.parent.gameObject.SetActive(false);
        }
        base.LoadMiniGame();
        SetTimeBeforeNextPicture();
        ActivateUI(true);
    }

    private void Update()
    {
        if (!isMinigamelaunched) return;

        _remainingTimeBeforeNextPicture -= Time.deltaTime;
        if (_remainingTimeBeforeNextPicture <= 0)
        {
            ActivatePlayerInput(false);
            SetTimeBeforeNextPicture();
            TakePicture();
        }
    }

    private void ActivatePlayerInput(bool activate)
    {
        foreach (PlayerController playerController in players)
        {
            playerController.isStunned = !activate;
            playerController.isInTheAir = !activate;
        }
    }

    private void SetTimeBeforeNextPicture()
    {
        SetTimeBeforeNextPicture(minTimerBetweenPictures, maxTimerBetweenPictures);
    }

    private void SetTimeBeforeNextPicture(float min, float max)
    {
        _remainingTimeBeforeNextPicture = Random.Range(min, max);
    }

    private void TakePicture()
    {
        StartCoroutine(TakePictureCoroutine());
    }

    private IEnumerator TakePictureCoroutine()
    {
        // Stopper le temps pour donner l'effet de prise de photo
        Time.timeScale = 0;
        
        // Après un certain temps
        yield return _waitSeconds;
        
        // Active l'image blanche pour simuler la prise de photo, puis comptabilisation des scores et déplacement de l'overlord
        picImage.SetActive(true);
        AddPlayerScores();
        if(overlordRepositioningChances >= Random.Range(0, 100)) overlord.ChangePosition();
        
        // Après un certain temps
        yield return _waitSeconds;
        
        // Désactive l'image blanche
        picImage.SetActive(false);
        
        // Après un certain temps
        yield return _waitSeconds;
        
        // Remet le temps normal et active les input players
        Time.timeScale = 1;
        ActivatePlayerInput(true);
    }

    private void AddPlayerScores()
    {
        for (int i = 0; i < players.Count; i++)
        {
            _scores[i] += (int)(Mathf.Clamp(20 - overlord.GetPlayerDistance(players[i].transform), 0, 20) * 10);
            UpdatePlayerScoreUI(i);
        }
    }

    private void UpdatePlayerScoreUI(int index)
    {
        scoreTexts[index].text = _scores[index].ToString();
    }

    public override void FinishTimer()
    {
        ActivateUI(false);
        isMinigamelaunched = false;
        isGameDone = true;
        OnMinigameEnd();
    }

    protected override int GetWinner()
    {
        return 0;
    }

    protected override Dictionary<PlayerController, int> GetRanking()
    {
        return null;
    }
}
