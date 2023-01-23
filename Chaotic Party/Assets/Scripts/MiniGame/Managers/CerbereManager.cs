using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerbereManager : SpamManager
{
    [SerializeField] private int endValue = 0;
    public override void Click(int playerIndex, float value, SpamButton spamButton = SpamButton.Any)
    {
        //playerIndex de 0 à 3, player.index en gros
        clicksArray[playerIndex] += value;
        DisplayCrown();
        
        if (IsSomeoneArrived()) OnMinigameEnd();
    }

    private bool IsSomeoneArrived()
    {
        foreach (float score in clicksArray)
        {
            if (score >= endValue)
            {
                return true;
            }
        }

        return false;
    }

    protected override void OnMinigameEnd()
    {
        throw new System.NotImplementedException();
    }
}
