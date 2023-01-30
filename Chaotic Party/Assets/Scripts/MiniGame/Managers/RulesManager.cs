using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RulesManager : MiniGameManager
{
    public SpriteRenderer launcheButton;
    
    protected void Start()
    {
        StartCoroutine(LoadMiniGameScene());
    }

    private IEnumerator LoadMiniGameScene()
    {
        MiniGameData miniGameData = ReferenceHolder.Instance.miniGameData;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(miniGameData.chosenMiniGames[
            miniGameData.currentMiniGameIndex], LoadSceneMode.Additive);
        
        while (!asyncOperation.isDone)
        {
            //TODO: Update loading screen
            yield return null;
        }

        launcheButton.color = Color.white;
        miniGameData.currentMiniGameIndex++;
    }
    
    protected override int GetWinner()
    {
        return -1;
    }

    protected override void OnMinigameEnd()
    {
        
    }
}
