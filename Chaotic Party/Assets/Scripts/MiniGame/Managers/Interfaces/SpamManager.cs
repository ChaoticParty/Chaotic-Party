using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpamManager : MiniGameManager
{
    protected float nbClicks;
    protected float[] clicksArray;

    protected void Start()
    {
        clicksArray = new float[players.Count];
    }

    public abstract void Click(int playerIndex, float value, SpamButton spamButton = SpamButton.Any);
}
