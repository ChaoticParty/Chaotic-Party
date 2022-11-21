using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpamRaceController : SpamController
{
    private int[] playersIndex;
    private bool hasClicked;
    public GameObject car;
    public GameObject raceCar;
    
    protected new void Awake()
    {
        base.Awake();
        
        player.xJustPressed.AddListener(player.index == 0 ? Click : () => { ClickOnOtherPlayer(0); });
        player.yJustPressed.AddListener(player.index == 1 ? Click : () => { ClickOnOtherPlayer(1); });
        player.bJustPressed.AddListener(player.index == 2 ? Click : () => { ClickOnOtherPlayer(2); });
        player.aJustPressed.AddListener(player.index == 3 ? Click : () => { ClickOnOtherPlayer(3); });
        
        player.aJustPressed.AddListener(Click);
        playersIndex = new int[player.miniGameManager.players.Count - 1];
        for (int i = 0; i < playersIndex.Length; i++)
        {
            playersIndex[i] = i + 1;
            if (playersIndex[i] <= player.index) playersIndex[i]--;
        }
        if(playersIndex.Length > 0) player.xJustPressed.AddListener(() => { ClickOnOtherPlayer(playersIndex[0]); });
        if(playersIndex.Length > 1) player.yJustPressed.AddListener(() => { ClickOnOtherPlayer(playersIndex[1]); });
        if(playersIndex.Length > 2) player.bJustPressed.AddListener(() => { ClickOnOtherPlayer(playersIndex[2]); });
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
        spamManager.Click(otherPlayerIndex, -spamManager.versusSpamValue);
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