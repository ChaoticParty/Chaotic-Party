using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ExplosionController : MiniGameController
{
    [SerializeField] private MenuManager menuManager;
    public GameObject actualPanel;
    [SerializeField] private float explosionDelay;
    private bool vibrate = false;
    private new void Awake()
    {
        base.Awake();
    }

    public override void AddListeners()
    {
        // player.yLongPressed.AddListener(MacronExplosion); //TODO ajouter les anims
        player.yJustPressed.AddListener(MacronExplosion);
        player.bJustPressed.AddListener(BackToMain);
        player.startPressed.AddListener(ReadyClick);
    }

    private void OnEnable()
    {
        AddListeners();
    }

    private void FixedUpdate()
    {
        if (vibrate)
        {
            //Anim de vibration
        }
    }

    private void MacronExplosion(float delay)
    {
        if (delay < explosionDelay) vibrate = true;
        else
        {
            EndExplosionAnimMenu();
            //lancement anim d'explosion
        }
    }
    private void MacronExplosion()
    {
        EndExplosionAnimMenu();
    }
    
    private void BackToMain()
    {
        menuManager.backAnim.SetTrigger("Push");
        menuManager.Back(actualPanel);
        EventSystem.current.SetSelectedGameObject(menuManager.partyBTN.gameObject);
    }

    private void ReadyClick()
    {
        // menuManager.TransitionAnimLaunch(true); //TODO montrer ca aux autres et voir si ca les inspirent
        menuManager.LauchGame();
    }

    public void EndExplosionAnimMenu()
    {
        menuManager.listPersonnages[menuManager.multiplayerManager.players.IndexOf(player)].SpawnSelectionScreen();
        gameObject.SetActive(false);
    }
}
