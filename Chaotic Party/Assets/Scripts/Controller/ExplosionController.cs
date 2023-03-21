using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MiniGameController
{
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private float explosionDelay;
    private bool vibrate = false;
    private new void Awake()
    {
        base.Awake();
        player.yLongPressed.AddListener(MacronExplosion);
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
        Debug.Log(delay);
        if (delay < explosionDelay) vibrate = true;
        else
        {
            EndExplosionAnimMenu();
            //lancement anim d'explosion
        }
    }

    public void EndExplosionAnimMenu()
    {
        menuManager.listPersonnages[menuManager.multiplayerManager.players.IndexOf(player)].SpawnSelectionScreen();
        gameObject.SetActive(false);
    }
}
