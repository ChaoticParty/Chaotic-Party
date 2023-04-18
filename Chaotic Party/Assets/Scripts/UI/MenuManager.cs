using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public MultiplayerManager multiplayerManager;
    public PlayersListSO playersListSO;
    public List<PlayerController> listUiPlayerControllers;
    public List<PlayerController> listInGamePlayerControllers;
    private ReferenceHolder _referenceHolder;
    private GameObject oldEventObject;
    private bool isClickCheckCoroutineActive = false;
    [Space]
    public string optionsScene;
    public List<sbyte> selectColor = new List<sbyte>();
    public sbyte readyCount = 0;
    public sbyte playSceneIndex = 1;
    public List<EcranPersonnage> listPersonnages = new List<EcranPersonnage>();
    public List<Animator> listMaskPersonnagesAnimator = new List<Animator>();
    private sbyte nbCurrentGamepads = 0;
    private sbyte nbGamepadsLastFrame = 0;
    [Space]
    public GameObject panelPrincipal;
    public GameObject panelParty;
    public GameObject panelMinigame;
    private GameObject oldPanel;
    [Space]
    public GameObject firstMenuPrincpal;
    public GameObject firstParty;
    public GameObject firstMinigame;
    public GameObject firstOptions;
    [Space]
    public Button partyBTN;
    public Button minigameBTN;
    public Button optionsBTN;
    public Button quitBTN;
    [Space]
    public Button minigameBackBTN;
    [Space]
    public Animator partyPlayerMinGO;
    public GameObject partyBandeauReadyGO;
    
    #region MonoBehaviour Méthodes

    private void Awake()
    {
        // Caching.ClearCache(); // Tester si ça resout le soucis de l'attribution des manettes
        _referenceHolder = GameObject.Find("ReferenceHolder").GetComponent<ReferenceHolder>();
        foreach (EcranPersonnage ecranPersonnage in listPersonnages)
        {
            ecranPersonnage.myPlayerController ??= ecranPersonnage.gameObject.GetComponent<PlayerController>();
        }
        playersListSO ??= ReferenceHolder.Instance.players;
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstMenuPrincpal);

        oldEventObject = firstMenuPrincpal;

        nbCurrentGamepads = multiplayerManager.GamepadCount();
        nbGamepadsLastFrame = multiplayerManager.GamepadCount();

        partyBTN.onClick.AddListener(PartyClick);
        minigameBTN.onClick.AddListener(MinigameClick);
        optionsBTN.onClick.AddListener(OptionsClick);
        quitBTN.onClick.AddListener(QuitClick);
        //Pas encore cree sur le prefab
        /*partyBackBTN.onClick.AddListener(() => { Back(panelParty); });
        minigameBackBTN.onClick.AddListener(() => { Back(panelMinigame); });*/
    }

    private void OnDisable()
    {
        partyBTN.onClick.RemoveAllListeners();
        minigameBTN.onClick.RemoveAllListeners();
        optionsBTN.onClick.RemoveAllListeners();
        quitBTN.onClick.RemoveAllListeners();
        //Pas encore cree sur le prefab
        /*partyBackBTN.onClick.RemoveAllListeners();
        minigameBackBTN.onClick.RemoveAllListeners();*/
    }

    private void LateUpdate()
    {
        nbCurrentGamepads = multiplayerManager.GamepadCount();
        if (!nbCurrentGamepads.Equals(nbGamepadsLastFrame))
        {
            multiplayerManager.InitMultiplayer();
            Debug.Log("InitFinis");
            foreach (EcranPersonnage ecranPersonnage in listPersonnages)
            {
                Debug.Log(ecranPersonnage.myPlayerController);
                if (ecranPersonnage.myPlayerController.gamepad != null)
                {
                    Debug.Log(ecranPersonnage.myPlayerController.index);
                    ecranPersonnage.transform.parent.gameObject.SetActive(ecranPersonnage.myPlayerController.gamepad.isConnected); //Gere l'activation du mask du player
                    ecranPersonnage.gameObject.SetActive(ecranPersonnage.myPlayerController.gamepad.isConnected);
                    ecranPersonnage.InitCusto();
                }
                else
                {
                    Debug.Log("null");
                }
            }
            // partyPlayerMinGO.gameObject.SetActive(nbCurrentGamepads < 2);
            partyPlayerMinGO.SetTrigger(nbCurrentGamepads < 2 ? "Descend" : "Monte");
        }
        nbGamepadsLastFrame = multiplayerManager.GamepadCount();
        
        if (EventSystem.current.alreadySelecting) return;
        if (!isClickCheckCoroutineActive) StartCoroutine(CheckMouseClick());
        if (EventSystem.current.currentSelectedGameObject != null)
            oldEventObject = EventSystem.current.currentSelectedGameObject;
    }
    
    private IEnumerator CheckMouseClick()
    {
        isClickCheckCoroutineActive = true;
        yield return new WaitForSeconds(0.1f);
        isClickCheckCoroutineActive = false;
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(oldEventObject);
        }
    }

    #endregion

    #region Class Méthodes

    #region LaunchGame
    
    public bool IsLaunchPossible()
    {
        sbyte playerCountTemp = 0;
        foreach (EcranPersonnage ecranPersonnage in listPersonnages)
        {
            if (ecranPersonnage.gameObject.activeSelf)
            {
                playerCountTemp++;
            }
        }
        return readyCount.Equals(playerCountTemp) /*&& playerCountTemp > 1*/;
    }

    public void LauchGame()
    {
        if (IsLaunchPossible())
        {
            foreach (EcranPersonnage ecranPerso in listPersonnages)
            {
                if (ecranPerso.gameObject.activeSelf)
                {
                    ecranPerso.FillSO();    
                }
            }
            SceneManager.LoadScene(playSceneIndex);
        }
    }
    #endregion

    private void PartyClick()
    {
        PanelChange(panelPrincipal, panelParty);
        if (nbCurrentGamepads < 2) partyPlayerMinGO.SetTrigger("Descend");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstParty);
    }

    private void MinigameClick()
    {
        PanelChange(panelPrincipal, panelMinigame);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstMinigame);
    }

    private void OptionsClick()
    {
        _referenceHolder.oldEventSystem = EventSystem.current.gameObject;
        EventSystem.current.gameObject.SetActive(false);
        SceneManager.LoadSceneAsync(optionsScene, LoadSceneMode.Additive);
    }

    private void QuitClick()
    {
        Application.Quit();
    }

    public void Back(GameObject actualPanel)
    {
        oldPanel.SetActive(true);
        actualPanel.SetActive(false);
    }

    private void PanelChange(GameObject oldPanel, GameObject newPanel)
    {
        this.oldPanel = oldPanel;
        newPanel.SetActive(true);
        oldPanel.SetActive(false);
    }
    
    #endregion
}
