using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpamRaceController : SpamController
{
    private int[] playersIndex;
    private bool hasClicked;
    
    protected new void Awake()
    {
        base.Awake();
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
}
