using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpamTestManager : MiniGameManager, ISpamManager
{
    public int nbClicks { get; private set; }
    private int[] clicksArray;

    private void Start()
    {
        clicksArray = new int[players.Count];
        Debug.Log(clicksArray.Length);
        Debug.Log(players.Count);
    }

    public void Click(int playerIndex)
    {
        nbClicks++;
        Debug.Log(nbClicks);
        clicksArray[playerIndex]++;
        Debug.Log(clicksArray[playerIndex]);
    }
}
