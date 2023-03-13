using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpamRaceBreakableRule : BreakableUI
{
    public List<SpamRaceRuleData> spamRaceRuleData;

    protected override void SetUp()
    {
        Debug.Log("setup");
        Debug.Log(_miniGameManager);
        Debug.Log(_miniGameManager.players.Count);
        for (int i = 0; i < spamRaceRuleData.Count; i++)
        {
            if (i >= _miniGameManager.players.Count)
            {
                Debug.Log("désactivation");
                spamRaceRuleData[i].parent.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("set sprites");
                //TODO: Changer le sprite avec le bon système de sprite après inté
                spamRaceRuleData[i].head.sprite = _miniGameManager.players[i].head.sprite;
                spamRaceRuleData[i].head.color = _miniGameManager.players[i].head.color;
            }
        }
    }
}

[Serializable]
public struct SpamRaceRuleData
{
    public GameObject parent;
    public Image head;
    public Image button;
}