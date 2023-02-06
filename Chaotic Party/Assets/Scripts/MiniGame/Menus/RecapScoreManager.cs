using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecapScoreManager : MiniGameManager
{
    public GameObject nextMiniGameButton;

    private void Awake()
    {
        MiniGameData miniGameData = ReferenceHolder.Instance.miniGameData;
        if (miniGameData.currentMiniGameIndex >= miniGameData.chosenMiniGames.Count)
        {
            nextMiniGameButton.SetActive(false);
        }
    }

    protected override int GetWinner()
    {
        return -1;
    }

    protected override void OnMinigameEnd() {}
}
