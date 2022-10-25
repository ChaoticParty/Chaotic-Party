using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public string optionsScene;
    
    public GameObject panelPrincipal;
    public GameObject panelParty;
    public GameObject panelMinigame;
    private GameObject oldPanel;
    
    public GameObject firstMenuPrincpal;
    public GameObject firstParty;
    public GameObject firstMinigame;
    public GameObject firstOptions;
    
    public Button partyBTN;
    public Button minigameBTN;
    public Button optionsBTN;
    public Button quitBTN;
    
    public Button partyBackBTN;
    public Button minigameBackBTN;
    
    #region MonoBehaviour Méthodes

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstMenuPrincpal);
        
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

    #endregion

    #region Class Méthodes

    private void PartyClick()
    {
        PanelChange(panelPrincipal, panelParty);
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
        SceneManager.LoadSceneAsync(optionsScene);
    }

    private void QuitClick()
    {
        Application.Quit();
    }

    private void Back(GameObject actualPanel)
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
