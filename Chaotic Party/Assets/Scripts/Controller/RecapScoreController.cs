using UnityEngine;
using UnityEngine.SceneManagement;

public class RecapScoreController : MiniGameController
{
    private RecapScoreManager _manager;
    private ReferenceHolder _referenceHolder;
    private TransitionSetter _transitionSetter;
    
    private void Start()
    {
        AddListeners();
        _manager = player.miniGameManager as RecapScoreManager;
        _referenceHolder = ReferenceHolder.Instance;
        _transitionSetter = _referenceHolder.transitionSetter;
    }

    private void NextMiniGame()
    {
        RecapScoreManager manager = player.miniGameManager as RecapScoreManager;
        if(!manager) return;
        
        player.soundManager.EventPlay("NextGameClick");
        player.soundManager.StopSelfSound(_manager.GetComponent<AudioSource>());
        
        if(manager.HasNextMiniGame())
        {
            _transitionSetter.StartTransition(null, LoadRules, 
                SetRulesPosition, null, 
                _manager.nextMiniGameButton.transform.position);
        }
        else if(!manager.endMenu)
        {
            _transitionSetter.StartTransition(null, LoadEndResults, 
                SetEndMenuPosition, null, 
                _manager.nextMiniGameButton.transform.position);
        }
    }

    private void SetRulesPosition()
    {
        _transitionSetter.lastTransition.SetUIPosition(_referenceHolder.miniGameData.GetTransitionPosition("RulesUI"));
    }

    private void SetEndMenuPosition()
    {
        _transitionSetter.lastTransition.SetUIPosition(_referenceHolder.miniGameData.GetTransitionPosition("EndResults"));
    }

    private void LoadRules()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("RulesUI", LoadSceneMode.Additive);
        Scene currentScene = gameObject.scene;
        _transitionSetter.WaitTillSceneLoad(asyncOperation, currentScene);
    }

    private void LoadEndResults()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("EndResults", LoadSceneMode.Additive);
        Scene currentScene = gameObject.scene;
        _transitionSetter.WaitTillSceneLoad(asyncOperation, currentScene);
    }
    
    private void ReturnToMenu()
    {
        player.soundManager.EventPlay("MenuClick");
        player.soundManager.StopSelfSound(_manager.GetComponent<AudioSource>());
        _transitionSetter.StartTransition(null, LoadMainMenu, 
            ResetTransitionPosition, null, 
            _manager.menuButton.transform.position);
    }

    private void ResetTransitionPosition()
    {
        ReferenceHolder referenceHolder = ReferenceHolder.Instance;
        Debug.Log(referenceHolder);
        Debug.Log(_transitionSetter);
        Debug.Log(_transitionSetter.lastTransition);
        Debug.Log(Vector3.zero);
        _transitionSetter.lastTransition.SetUIPosition(Vector3.zero);
    }

    private void LoadMainMenu()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("MenuPrincipal", LoadSceneMode.Additive);
        Scene currentScene = gameObject.scene;
        _transitionSetter.WaitTillSceneLoad(asyncOperation, currentScene);
    }

    public override void AddListeners()
    {
        player.startPressed.AddListener(ReturnToMenu);
        player.xJustPressed.AddListener(NextMiniGame);
    }
}
