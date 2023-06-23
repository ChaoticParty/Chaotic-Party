using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpamRaceBreakableRule : BreakableUI
{
    public List<SpamRaceRuleData> spamRaceRuleData;

    protected override void SetUp()
    {
        for (int i = 0; i < spamRaceRuleData.Count; i++)
        {
            if (i >= _miniGameManager.players.Count)
            {
                spamRaceRuleData[i].parent.gameObject.SetActive(false);
            }
            else
            {
                spamRaceRuleData[i].head.SetupSkin(_multiplayerManager.players[i]._playerSo.head, 
                    _multiplayerManager.players[i]._playerSo.color);
            }
        }
    }
}

[Serializable]
public struct SpamRaceRuleData
{
    public GameObject parent;
    public SkinSelector head;
    public Image button;
}