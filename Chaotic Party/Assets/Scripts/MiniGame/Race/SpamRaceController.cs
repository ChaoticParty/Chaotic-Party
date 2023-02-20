using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public Transform spamValuePosition;
    
    protected new void Awake()
    {
        base.Awake();
        
        player.xJustPressed.AddListener(player.index == 0 ? Click : () => { ClickOnOtherPlayer(0); });
        if(spamManager.players.Count >= 2) player.yJustPressed.AddListener(player.index == 1 ? Click : () => { ClickOnOtherPlayer(1); });
        if(spamManager.players.Count >= 3) player.bJustPressed.AddListener(player.index == 2 ? Click : () => { ClickOnOtherPlayer(2); });
        if(spamManager.players.Count >= 4) player.aJustPressed.AddListener(player.index == 3 ? Click : () => { ClickOnOtherPlayer(3); });
        _spamRaceManager = spamManager as SpamRaceManager;
        float timeBetweenRegister = _spamRaceManager.timeBeforeClickRegister;
        if(_spamRaceManager.typeAjoutPoints is PointsType.BigPoints or PointsType.VfxBurst) 
            InvokeRepeating(nameof(SendClicks), timeBetweenRegister, timeBetweenRegister);
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
                if (_tmpPrefab)
                {
                    _tmpPrefab.text = _clickValue.ToString();
                    _tmpPrefab.color = Color.Lerp(_tmpPrefab.color, Color.red, Time.deltaTime * 50);
                    //_tmpPrefab.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
                    Transform tmpPrefabTransform = _tmpPrefab.transform;
                    tmpPrefabTransform.localScale += Vector3.one / 10;
                    tmpPrefabTransform.position += Vector3.up / 3;
                }
                else
                {
                    _tmpPrefab = Instantiate(_spamRaceManager.tmpPrefab, spamValuePosition.position, quaternion.identity, transform.GetChild(0).GetChild(0));
                }
                break;
        }
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
            1, launchSprite, OnEnd);
    }

    private IEnumerator Cooldown()
    {
        hasClicked = true;
        yield return new WaitForNextFrameUnit();
        hasClicked = false;
    }

    public Coroutine Race(Vector2 destination)
    {
        Debug.Log(player.index);
        Debug.Log(destination.x);
        _coroutine = StartCoroutine(RaceEnumerator(destination));
        return _coroutine;
    }

    private IEnumerator RaceEnumerator(Vector2 destination)
    {
        Transform raceCarTransform = raceCar.transform;
        while ((Vector2)raceCarTransform.position != destination)
        {
            raceCarTransform.position = Vector2.MoveTowards(raceCarTransform.position,
                destination, Time.deltaTime * 5);
            yield return new WaitForNextFrameUnit();
        }

        _coroutine = null;
    }
}
