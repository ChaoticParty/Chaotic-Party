using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpamRaceManager : SpamManager
{
    public float timer;
    [NonSerialized] public float currentTimer;
    [SerializeField] private Image timerImage;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private GameObject[] crowns;

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

        currentTimer = timer;
    }

    private void Update()
    {
        if(Time.timeScale == 0) return;
        
        currentTimer -= Time.deltaTime;

        if (currentTimer <= 0)
        {
            Time.timeScale = 0;
            winText.text = "Joueur " + (GetWinner() + 1) + " a gagné!";
            winText.gameObject.SetActive(true);
        }
        else
        {
            timerImage.fillAmount = currentTimer / timer;
        }
    }

    public override void Click(int playerIndex, float value, SpamButton spamButton = SpamButton.Any)
    {
        nbClicks++;
        clicksArray[playerIndex] += value;
        if(spamTexts.Length > playerIndex) spamTexts[playerIndex].text = clicksArray[playerIndex].ToString(CultureInfo.CurrentCulture);
        DisplayCrown();
    }

    private void DisplayCrown()
    {
        int winner = GetWinner();
        for (int i = 0; i < crowns.Length; i++)
        {
            crowns[i].SetActive(i == winner);
        }
    }

    private int GetWinner()
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
}
