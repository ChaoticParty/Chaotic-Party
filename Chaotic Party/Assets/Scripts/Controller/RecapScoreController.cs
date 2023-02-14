using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecapScoreController : MiniGameController
{
    private new void Awake()
    {
        base.Awake();
        player.startPressed.AddListener(ReturnToMenu);
        RecapScoreManager manager = player.miniGameManager as RecapScoreManager;
        if(manager && manager.HasNextMiniGame()) player.aJustPressed.AddListener(NextMiniGame);
    }

    private void NextMiniGame()
    {
        SceneManager.LoadScene("RulesUI");
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}
