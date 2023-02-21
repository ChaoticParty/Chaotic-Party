using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MiniGameData", menuName = "ScriptableObjects/MiniGameData")]
public class MiniGameData : ResetOnMainMenu
{
    public List<string> miniGames;
    public List<string> chosenMiniGames;
    public int currentMiniGameIndex;
    
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("reset");
        //chosenMiniGames = new List<string>();
        currentMiniGameIndex = 0;
    }
}
