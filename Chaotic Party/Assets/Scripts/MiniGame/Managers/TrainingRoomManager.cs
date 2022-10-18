using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class TrainingRoomManager : SpamManager
{
    [SerializeField] private TextMeshProUGUI[] spamTexts;

    protected new void Start()
    {
        base.Start();
        for (int i = 0; i < spamTexts.Length; i++)
        {
            if (i >= players.Count)
            {
                spamTexts[i].transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public override void Click(int playerIndex, float value, SpamButton spamButton = SpamButton.Any)
    {
        nbClicks++;
        clicksArray[playerIndex] += value;
        spamTexts[playerIndex].text = clicksArray[playerIndex].ToString(CultureInfo.CurrentCulture);
    }
}
