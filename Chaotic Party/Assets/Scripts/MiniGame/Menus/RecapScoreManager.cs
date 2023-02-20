using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecapScoreManager : MiniGameManager
{
    public GameObject nextMiniGameButton;
    private MiniGameData miniGameData;

    #region UpdateScore

    [Header("Update Score")] 
    public List<Score> scoreObjects;

    #endregion

    private void Awake()
    {
        miniGameData ??= ReferenceHolder.Instance.miniGameData;
        if (!HasNextMiniGame())
        {
            nextMiniGameButton.SetActive(false);
        }

        List<PlayerSO> playersData = ReferenceHolder.Instance.players.players;
        List<PlayerSO> rankToPlayerData = new(4);
        foreach (PlayerSO playerSo in playersData)
        {
            rankToPlayerData.Add(playerSo);
        }
        foreach (PlayerSO playerSo in playersData)
        {
            rankToPlayerData[playerSo.ranking] = playerSo;
        }
        for (int i = 0; i < scoreObjects.Count; i++)
        {
            Score scoreObj = scoreObjects[i];
            if (i >= players.Count)
            {
                scoreObj.obj.SetActive(false);
            }
            else
            {
                scoreObj.playerName.text = "Joueur" + (rankToPlayerData[i].id + 1);
                scoreObj.score.text = rankToPlayerData[i].points.ToString();
            }
        }
    }

    public bool HasNextMiniGame()
    {
        miniGameData ??= ReferenceHolder.Instance.miniGameData;
        return miniGameData.currentMiniGameIndex < miniGameData.chosenMiniGames.Count;
    }

    public override void FinishTimer()
    {
        
    }

    protected override int GetWinner()
    {
        return -1;
    }

    protected override void OnMinigameEnd() {}

    [Serializable]
    public struct Score
    {
        public GameObject obj;
        public TextMeshProUGUI playerName;
        public TextMeshProUGUI score;
        // TODO: Ajouter les sprites des larbins
    }
}
