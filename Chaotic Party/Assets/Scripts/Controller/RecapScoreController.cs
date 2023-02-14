using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecapScoreController : MiniGameController
{
    private void Start()
    {
        player.startPressed.AddListener(ReturnToMenu);
        player.aJustPressed.AddListener(NextMiniGame);
    }

    private void NextMiniGame()
    {
        SceneManager.LoadScene("RulesUI");
    }

    private void ReturnToMenu()
    {
        RecapScoreManager manager = player.miniGameManager as RecapScoreManager;
        if(manager && manager.HasNextMiniGame())
        {
            SceneManager.LoadScene("MenuPrincipal");
        }
    }
}
