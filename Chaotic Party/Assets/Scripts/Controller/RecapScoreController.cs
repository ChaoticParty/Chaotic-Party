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
        if(!manager) return;
        
        if(manager.HasNextMiniGame())
        {
            SceneManager.LoadScene("RulesUI");
        }
        else if(!manager.endMenu)
        {
            SceneManager.LoadScene("EndResults");
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
