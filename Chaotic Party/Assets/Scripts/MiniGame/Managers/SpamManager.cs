using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class SpamManager : MiniGameManager
{
    [SerializeField] protected TextMeshProUGUI[] spamTexts;
    protected float nbClicks;
    protected float[] clicksArray;
    private Dictionary<PlayerController, int> _ranking;
    [Header("Valeurs de spam"), Space(10)]
    [Range(0, 1000), Tooltip("Valeur qui sera ajoutée au conteur d'un joueur quand il spam")] public float spamValue;
    [Range(0, 1000), Tooltip("Valeur qui sera soustraite au conteur d'un joueur quand il spam")] public float versusSpamValue;

    protected void Start()
    {
        clicksArray = new float[players.Count];
    }
    
    protected override int GetWinner()
    {
        int winnerIndex = 0;
        float winValue = clicksArray[0];
        for (int i = 0; i < clicksArray.Length; i++)
        {
            if (clicksArray[i] > winValue)
            {
                winValue = clicksArray[i];
                winnerIndex = i;
            }
        }

        return winnerIndex;
    }

    public abstract void Click(int playerIndex, float value, SpamButton spamButton = SpamButton.Any);
}
