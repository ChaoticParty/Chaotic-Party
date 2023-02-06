using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecapScoreController : MiniGameController
{
    private new void Awake()
    {
        base.Awake();
        player.aJustPressed.AddListener(NextMiniGame);
        player.startPressed.AddListener(ReturnToMenu);
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
