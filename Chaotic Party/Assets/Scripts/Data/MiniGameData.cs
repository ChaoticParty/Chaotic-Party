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
}
