using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpamRaceController : SpamController
{
    private int[] playersIndex;
    private bool hasClicked;
    public GameObject car;
    public GameObject raceCar;
    public Sprite launchSprite;
    
    protected new void Awake()
    {
        base.Awake();
        
        Debug.Log("index:" + player.index);
        player.xJustPressed.AddListener(player.index == 0 ? Click : () => { ClickOnOtherPlayer(0); });
        player.yJustPressed.AddListener(player.index == 1 ? Click : () => { ClickOnOtherPlayer(1); });
        player.bJustPressed.AddListener(player.index == 2 ? Click : () => { ClickOnOtherPlayer(2); });
        player.aJustPressed.AddListener(player.index == 3 ? Click : () => { ClickOnOtherPlayer(3); });
    }

    public void DeactivatePlayer()
    {
        car.SetActive(false);
        raceCar.SetActive(false);
    }
    
    protected override void Click()
    {
        if (hasClicked) return;
        StartCoroutine(Cooldown());
        spamManager.Click(player.index, spamManager.spamValue);
    }

    private void ClickOnOtherPlayer(int otherPlayerIndex)
    {
        if (hasClicked) return;
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

    public void Race(Vector2 destination)
    {
        Debug.Log(player.index);
        Debug.Log(destination.x);
        StartCoroutine(RaceEnumerator(destination));
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
    }
}
