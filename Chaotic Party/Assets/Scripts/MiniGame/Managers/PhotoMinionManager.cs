using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PhotoMinionManager : MiniGameManager
{
    public CanvasGroup uiCanvasGroup;
    public SoundManager soundManager;
    public PhotoMinionOverlord overlord;
    [Range(0, 500)] 
    public int pointsGained;
    [Range(-500, 0)] 
    public int pointsLost;

    public TimerType typeDeTimer = TimerType.Fixed;
    [Range(0, 60), ShowIf(nameof(typeDeTimer), TimerType.Random)]
    public float minTimerBetweenPictures;
    [Range(0, 60), ShowIf(nameof(typeDeTimer), TimerType.Random)]
    public float maxTimerBetweenPictures;
    [Range(0, 60), ShowIf(nameof(typeDeTimer), TimerType.Fixed)]
    public float timerBetweenPictures = 10;
    [Range(0, 10)] 
    public float launchAnimTime = 3;
    private bool _animLaunched;
    [Range(0, 100)]
    public float overlordRepositioningChances = 100;

    private float _remainingTimeBeforeNextPicture;
    public GameObject picImage;
    public float timeToWait;
    private List<int> _scores = new();
    public List<TextMeshProUGUI> scoreTexts;

    private WaitForSecondsRealtime _waitSeconds;

    public OnOverlordTrigger pictureData;

    public List<Image> polaroids;
    public Sprite pictureTaken;
    private int _picIndex;

    protected void Start()
    {
        ActivateUI(false);
        pictureData ??= FindObjectOfType<OnOverlordTrigger>();
        // soundManager ??= FindObjectOfType<SoundManager>();
        overlord.ChangePosition(false);
        overlord.PlaceCameraOnOverlord();
        //ColoriseObjectsAccordingToPlayers(ReferenceHolder.Instance.players.players, carsToColorise);
    }

    private void ActivateUI(bool activate)
    {
        uiCanvasGroup.alpha = activate ? 1 : 0;
        uiCanvasGroup.interactable = activate;
        uiCanvasGroup.blocksRaycasts = activate;
    }

    public override void StartMiniGame()
    {
        timer += 1f;
        base.StartMiniGame();
    }

    [ContextMenu("LoadMiniGame")]
    public override void LoadMiniGame()
    {
        overlord ??= FindObjectOfType<PhotoMinionOverlord>();
        soundManager.PlaySelfSound(gameObject.GetComponent<AudioSource>(), true);
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
        if (!_animLaunched && _remainingTimeBeforeNextPicture <= launchAnimTime)
        {
            _animLaunched = true;
            overlord.StartAnimationBeforePicture();
        }
        if (_remainingTimeBeforeNextPicture <= 0)
        {
            overlord.StopAnimationBeforePicture();
            ActivatePlayerInput(false);
            SetTimeBeforeNextPicture();
            TakePicture();
            _animLaunched = false;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) /*|| (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.SysReq)*/)
        {
            TakePicture();
        }
    }

    private void ActivatePlayerInput(bool activate)
    {
        foreach (PlayerController playerController in players)
        {
            playerController.enabled = activate;
            if(activate) playerController.AddAllListeners();
            //playerController.isStunned = !activate;
            //playerController.isInTheAir = !activate;
        }
    }

    private void SetTimeBeforeNextPicture()
    {
        switch (typeDeTimer)
        {
            case TimerType.Fixed:
                SetTimeBeforeNextPicture(timerBetweenPictures);
                break;
            case TimerType.Random:
                SetTimeBeforeNextPicture(minTimerBetweenPictures, maxTimerBetweenPictures);
                break;
            default:
                SetTimeBeforeNextPicture(timerBetweenPictures);
                break;
        }
    }

    private void SetTimeBeforeNextPicture(float time)
    {
        _remainingTimeBeforeNextPicture = time;
    }

    private void SetTimeBeforeNextPicture(float min, float max)
    {
        _remainingTimeBeforeNextPicture = Random.Range(min, max);
    }

    private void TakePicture()
    {
        soundManager.EventPlay("Photo");
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
        overlord.Focus();
        AddPlayerScores();
        
        // Après un certain temps
        yield return _waitSeconds;

        polaroids[_picIndex].sprite = pictureTaken;
        _picIndex++;
        // Désactive l'image blanche
        picImage.SetActive(false);
        
        // Après un certain temps
        yield return _waitSeconds;

        overlord.RemoveCameraFromOverlord();
        overlord.Unfocus();
        
        // Après un certain temps
        yield return _waitSeconds;
        if(overlordRepositioningChances >= Random.Range(0, 100)) overlord.ChangePosition();
        yield return _waitSeconds;
        yield return _waitSeconds;
        
        overlord.PlaceCameraOnOverlord();
        
        // Remet le temps normal et active les input players
        Time.timeScale = 1;
        ActivatePlayerInput(true);
    }

    private void AddPlayerScores()
    {
        for (int i = 0; i < players.Count; i++)
        {
            PlayerController player = players[i];
            if (pictureData.playersTouchingOverlord.Contains(player))
            {
                if (player.isHit || player.isStunned)
                {
                    // Si le joueur est près de l'overlord et il est stun/poussé
                    // Perds des points
                    _scores[i] += pointsLost;
                }
                else
                {
                    // Si le joueur est près de l'overlord et il n'est ni stun ni poussé
                    // Gagne des points
                    _scores[i] += pointsGained;
                    soundManager.PlaySelfSound(scoreTexts[i].GetComponent<AudioSource>());
                }
            }
            else
            {
                // Si le joueur n'est pas à côté de l'overlord
                // Perds des points
                _scores[i] += pointsLost;
            }
            
            //_scores[i] += (int)(Mathf.Clamp(20 - overlord.GetPlayerDistance(players[i].transform), 0, 20) * 10);
            UpdatePlayerScoreUI(i);
        }
        
        DisplayCrown();
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
    
    protected override void OnMinigameEnd()
    {
        ranking = GetRanking();
        AddPoints();
        SetCurrentRanking();
        soundManager.StopSelfSound(gameObject.GetComponent<AudioSource>());
        LoadRecap();
    }

    protected override int GetWinner()
    {
        int winnerIndex = 0;
        float winValue = _scores[0];
        for (int i = 0; i < _scores.Count; i++)
        {
            if (_scores[i] > winValue)
            {
                winValue = _scores[i];
                winnerIndex = i;
            }
        }

        return winnerIndex;
    }

    protected override Dictionary<PlayerController, int> GetRanking()
    {
        Dictionary<PlayerController, int> ranking = new();
        for (int i = 0; i < players.Count; i++)
        {
            int currentRanking = 0;
            for (int j = 0; j < players.Count; j++)
            {
                if (i != j)
                {
                    if (_scores[i] < _scores[j])
                    {
                        currentRanking++;
                    }
                }
            }
            ranking.Add(players[i], currentRanking);
        }

        return ranking;
    }
}

public enum TimerType
{
    Fixed,
    Random
}