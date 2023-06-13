using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MiniGameData", menuName = "ScriptableObjects/MiniGameData")]
public class MiniGameData : SerializedScriptableObject
{
    public List<string> miniGames;
    public List<string> chosenMiniGames;
    public int currentMiniGameIndex;
    public int numberOfMinigames;
    public Dictionary<string, Vector3> TransitionPositionInScene = new();

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

    public Vector3 GetTransitionPosition(string scene)
    {
        return TransitionPositionInScene.ContainsKey(scene) ? TransitionPositionInScene[scene] : default;
    }

    public Vector3 GetTransitionPosition(int scene)
    {
        string sceneName = SceneManager.GetSceneByBuildIndex(scene).name;
        Debug.Log(sceneName);
        return TransitionPositionInScene.ContainsKey(sceneName) ? TransitionPositionInScene[sceneName] : default;
    }

    [Button]
    public void SetSceneName(int sceneIndex)
    {
        TransitionPositionInScene.Add(SceneManager.GetSceneByBuildIndex(sceneIndex).name, default);
    }
}
