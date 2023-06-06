using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MiniGameData", menuName = "ScriptableObjects/MiniGameData")]
public class MiniGameData : ScriptableObject
{
    public List<string> miniGames;
    public List<string> chosenMiniGames;
    public int currentMiniGameIndex;
    public int numberOfMinigames;

    public void RandomiseMiniGames()
    {
        List<string> miniGamesTemp = new(miniGames);
        chosenMiniGames = new();
        for (int i = 0; i < miniGames.Count; i++)
        {
            if(i >= numberOfMinigames) break;
            
            int rnd = Random.Range(0, miniGamesTemp.Count);
            chosenMiniGames.Add(miniGamesTemp[rnd]);
            miniGamesTemp.RemoveAt(rnd);
        }
    }
}
