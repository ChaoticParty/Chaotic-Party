using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SpamRaceController : SpamController
{
    private int[] playersIndex;
    private bool hasClicked;
    public GameObject car;
    public GameObject raceCar;
    public Sprite launchSprite;
    public Coroutine _coroutine;
    private int _clickValue;
    private SpamRaceManager _spamRaceManager;
    private TextMeshProUGUI _tmpPrefab;
    private GameObject _effect;
    public Transform spamValuePosition;
    public Transform spamButton;
    public Transform playerPositionVoiture;
    
    protected new void Awake()
    {
        base.Awake();
        
        AddListeners();
    }

    public override void AddListeners()
    {
        player.xJustPressed.AddListener(player.index == 0 ? Click : () => { ClickOnOtherPlayer(0); });
        if(spamManager.players.Count >= 2) player.yJustPressed.AddListener(player.index == 1 ? Click : () => { ClickOnOtherPlayer(1); });
        if(spamManager.players.Count >= 3) player.bJustPressed.AddListener(player.index == 2 ? Click : () => { ClickOnOtherPlayer(2); });
        if(spamManager.players.Count >= 4) player.aJustPressed.AddListener(player.index == 3 ? Click : () => { ClickOnOtherPlayer(3); });
        _spamRaceManager = spamManager as SpamRaceManager;
        float timeBetweenRegister = _spamRaceManager.timeBeforeClickRegisters;
        if(_spamRaceManager.typeAjoutPoints is PointsType.BigPoints or PointsType.VfxBurst)
            _spamRaceManager.onLoadMiniGame.AddListener(() => InvokeRepeating(nameof(SendClicks), timeBetweenRegister, timeBetweenRegister));
    }

    public void DeactivatePlayer()
    {
        car.SetActive(false);
        raceCar.SetActive(false);
    }
    
    protected override void Click()
    {
        if(!_spamRaceManager.isMinigamelaunched) return;
        
        if (hasClicked) return;
        player.MetCheval();
        StartCoroutine(Cooldown());
        switch (_spamRaceManager.typeAjoutPoints)
        {
            case PointsType.VfxBurst:
                _clickValue += (int)spamManager.spamValue;
                //_spamRaceManager.UpdateClickUi(player.index, spamManager.spamValue, true);
                break;
            case PointsType.Continuous:
                spamManager.Click(player.index, spamManager.spamValue);
                break;
            case PointsType.BigPoints:
                _clickValue += (int)spamManager.spamValue;
                
                if(_effect) Destroy(_effect);
                
                if (_tmpPrefab)
                {
                    _spamRaceManager.SetClickText(_tmpPrefab.transform, _tmpPrefab, _clickValue, 
                        Mathf.RoundToInt(_clickValue / _spamRaceManager.spamValue) - 1, out _effect);
                }
                else
                {
                    _tmpPrefab = Instantiate(_spamRaceManager.tmpPrefab, spamValuePosition.position, 
                        _spamRaceManager.points[0].GetRotation(), 
                        transform.GetChild(0).GetChild(0));
                    _tmpPrefab.transform.localScale = _spamRaceManager.points[0].scale;
                    _spamRaceManager.SetClickText(_tmpPrefab.transform, _tmpPrefab, _clickValue, 
                        Mathf.RoundToInt(_clickValue / _spamRaceManager.spamValue) - 1, out _effect);
                }
                break;
        }
        spamButton.localScale = Vector3.one * 0.7f;
        GameObject poolObject = Pooling.instance.GetPoolObject(playerPositionVoiture);
        ChevalRace chevalRace = poolObject.GetComponent<ChevalRace>();
        chevalRace.StartAnimation();
    }

    public void SendClicks()
    {
        if(!_spamRaceManager.isMinigamelaunched) return;
        
        
        if(_spamRaceManager.typeAjoutPoints == PointsType.VfxBurst)
        {
            spamManager.Click(player.index, _clickValue);
            _clickValue = 0;
        }
        else
        {
            if(!_tmpPrefab) return;
            StartCoroutine(_spamRaceManager.SendPointToTotal(_tmpPrefab, player.index, _clickValue));
            _clickValue = 0;
            _tmpPrefab = null;
        }
    }

    private void ClickOnOtherPlayer(int otherPlayerIndex)
    {
        if (hasClicked || !player.miniGameManager.isMinigamelaunched) return;
        StartCoroutine(Cooldown());
        ThrowObjectCurve throwObjectScript = new GameObject().AddComponent<ThrowObjectCurve>();
        Vector2 pos = transform.position;
        Vector2 endPos = spamManager.players[otherPlayerIndex].transform.position;//new(-3f, -1f);
        void OnEnd() => spamManager.Click(otherPlayerIndex, -spamManager.versusSpamValue);
        throwObjectScript.Setup(pos, endPos/*spamManager.players[otherPlayerIndex].transform.position*/, 0.5f, 
            1, launchSprite, new Vector3(0.3f, 0.3f, 0.3f), OnEnd);
    }

    private IEnumerator Cooldown()
    {
        hasClicked = true;
        yield return new WaitForNextFrameUnit();
        hasClicked = false;
        spamButton.localScale = Vector3.one;
    }

    public Coroutine Race(Vector2 destination, bool winner)
    {
        _coroutine = StartCoroutine(RaceCoroutine(destination, winner));
        player.Conduit();
        return _coroutine;
    }

    private IEnumerator RaceCoroutine(Vector2 destination, bool winner)
    {
        Transform raceCarTransform = raceCar.transform;
        while ((Vector2)raceCarTransform.position != destination)
        {
            raceCarTransform.position = Vector2.MoveTowards(raceCarTransform.position,
                destination, Time.deltaTime * 40);
            yield return new WaitForNextFrameUnit();
        }

        if(winner) player.VictoryAnimation();
        else player.ConduitDefaite();
        _coroutine = null;
    }
}
