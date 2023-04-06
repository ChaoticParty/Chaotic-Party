using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecapScoreController : MiniGameController
{
    private void Start()
    {
        AddListeners();
    }

    private void NextMiniGame()
    {
        RecapScoreManager manager = player.miniGameManager as RecapScoreManager;
        if(manager && manager.HasNextMiniGame())
        {
            SceneManager.LoadScene("RulesUI");
        }
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public override void AddListeners()
    {
        player.startPressed.AddListener(ReturnToMenu);
        player.xJustPressed.AddListener(NextMiniGame);
    }
}
