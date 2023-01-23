using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpamTrainingRoomController : SpamController
{
    private int[] playersIndex;
    private bool hasClicked;
    
    protected new void Awake()
    {
        base.Awake();
        
        player.xJustPressed.AddListener(player.index == 0 ? Click : () => { ClickOnOtherPlayer(0); });
        player.yJustPressed.AddListener(player.index == 1 ? Click : () => { ClickOnOtherPlayer(1); });
        player.bJustPressed.AddListener(player.index == 2 ? Click : () => { ClickOnOtherPlayer(2); });
        player.aJustPressed.AddListener(player.index == 3 ? Click : () => { ClickOnOtherPlayer(3); });
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
