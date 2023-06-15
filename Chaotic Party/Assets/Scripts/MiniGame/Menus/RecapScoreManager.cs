using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RecapScoreManager : MiniGameManager
{
    public bool endMenu;
    public GameObject nextMiniGameButton;
    public GameObject menuButton;
    private MiniGameData miniGameData;
    [SerializeField] private SoundManager _soundManager;

    #region UpdateScore

    [Header("Update Score")] 
    public List<Score> scoreObjects;

    #endregion

    private void Awake()
    {
        miniGameData ??= ReferenceHolder.Instance.miniGameData;
        if (!HasNextMiniGame())
        {
            //nextMiniGameButton.SetActive(false);
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
            Debug.Log("obj" + i);
            Score scoreObj = scoreObjects[i];
            if (i >= players.Count)
            {
                scoreObj.obj.SetActive(false);
            }
            else
            {
                ColorTools.ColorToName(rankToPlayerData[i].color, out string playerColor);
                scoreObj.playerName.text = rankToPlayerData[i].race.nomRace + " " + playerColor;//"Joueur" + (rankToPlayerData[i].id + 1);
                scoreObj.score.text = rankToPlayerData[i].points.ToString();
                Transform playerTransform = players[playersData[i].ranking].transform;
                playerTransform.SetParent(scoreObj.sceneObject);
                playerTransform.localScale = Vector3.one;
                playerTransform.localPosition = Vector3.zero;
            }
        }

        foreach (PlayerController player in players)
        {
            switch (player._playerSo.ranking)
            {
                case 0:
                    player.VictoryAnimation(Random.Range(0, 2));
                    break;
                case 2:
                    player.DefeatAnimation();
                break;
                case 3:
                    player.DefeatAnimation(1);
                    break;
            }
        }

        // foreach (PlayerController player in players)
        // {
        //     player.ChangeColor();
        // }
    }

    private void OnEnable()
    {
        _soundManager.PlaySelfSound(gameObject.GetComponent<AudioSource>(), true);
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

    protected override Dictionary<PlayerController, int> GetRanking()
    {
        return null;
    }

    protected override void OnMinigameEnd() {}

    [Serializable]
    public struct Score
    {
        public Transform sceneObject;
        public GameObject obj;
        public TextMeshProUGUI playerName;
        public TextMeshProUGUI score;
        // TODO: Ajouter les sprites des larbins
    }
}
