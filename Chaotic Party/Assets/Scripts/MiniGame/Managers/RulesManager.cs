using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RulesManager : MiniGameManager
{
    public float loadingTime;
    private float _currentLoadingTime;
    public Image loadingImage;
    public SpriteRenderer launchButton;
    private MiniGameManager _loadedMiniGameManager;

    [Space] 
    [Header("Listes des composants des règles")] 
    [SerializeField] private GameObject rules;
    [SerializeField] private List<string> keyDictRules = new List<string>();
    [SerializeField] private List<GameObject> valueDictRules = new List<GameObject>();
    private Dictionary<string, GameObject> dictRules = new Dictionary<string, GameObject>();
    [SerializeField] private GameObject rulesCassable;
    [SerializeField] private List<string> keyDictRulesCassable = new List<string>();
    [SerializeField] private List<GameObject> valueDictRulesCassable = new List<GameObject>();
    private Dictionary<string, GameObject> dictRulesCassable = new Dictionary<string, GameObject>();

    protected void Start()
    {
        _currentLoadingTime = 0;
        isMinigamelaunched = true;
        //StartMiniGame();
        StartCoroutine(LoadMiniGameScene());
        StartCoroutine(OngoingLoading());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator LoadMiniGameScene()
    {
        MiniGameData miniGameData = ReferenceHolder.Instance.miniGameData;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(miniGameData.chosenMiniGames[
            miniGameData.currentMiniGameIndex], LoadSceneMode.Additive);
        
        DisplayRules(miniGameData);
        
        while (!asyncOperation.isDone)
        {
            //TODO: Update loading screen
            yield return null;
        }
        
        SearchForLoadedMinigameManager();

        launchButton.color = Color.white;
        miniGameData.currentMiniGameIndex++;
    }

    private IEnumerator OngoingLoading()
    {
        while (_currentLoadingTime < loadingTime)
        {
            yield return null;
            _currentLoadingTime += Time.deltaTime;
            loadingImage.fillAmount = _currentLoadingTime / loadingTime;
        }
        
        StartLoadedMinigame();
    }

    private void DisplayRules(MiniGameData miniGameData)
    {
        for (int i = 0; i < keyDictRules.Count; i++)
        {
            dictRules.Add(keyDictRules[i], valueDictRules[i]);
        }
        for (int i = 0; i < keyDictRulesCassable.Count; i++)
        {
            dictRulesCassable.Add(keyDictRulesCassable[i], valueDictRulesCassable[i]);
        }

        Instantiate(dictRules[miniGameData.chosenMiniGames[
            miniGameData.currentMiniGameIndex]], rules.transform);
        Instantiate(dictRulesCassable[miniGameData.chosenMiniGames[
            miniGameData.currentMiniGameIndex]], rulesCassable.transform);
        Debug.Log("fin display rules");
    }

    private void SearchForLoadedMinigameManager()
    {
        MiniGameManager[] minigameManagers = FindObjectsOfType<MiniGameManager>();

        foreach (MiniGameManager minigameManager in minigameManagers)
        {
            if (minigameManager != this) _loadedMiniGameManager = minigameManager;
        }
    }

    public void StartLoadedMinigame()
    {
        _loadedMiniGameManager.LoadMiniGame();
        foreach (PlayerController player in _loadedMiniGameManager.players)
        {
            player.gamepad.A.Enable();
            player.gamepad.X.Enable();
            player.gamepad.leftStick.Enable();
        }
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }

    public override void FinishTimer() { }

    protected override int GetWinner()
    {
        return -1;
    }

    protected override Dictionary<PlayerController, int> GetRanking()
    {
        return null;
    }

    protected override void OnMinigameEnd() { }

    public override void StartMiniGame()
    {
        base.StartMiniGame();
    }
}
