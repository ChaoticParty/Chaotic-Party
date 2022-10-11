using System;
using System.Collections;
using System.Collections.Generic;
using HinputClasses;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    public List<PlayerController> players;
    private MiniGameManager miniGameManager;
    
    private void Awake()
    {
        miniGameManager ??= FindObjectOfType<MiniGameManager>();
        for (int i = 0; i < Hinput.gamepad.Count; i++)
        {
            Gamepad gamepad = Hinput.gamepad[i];

            if (players.Count <= i)
            {
                return;
            }
            Debug.Log("avant player connecté");
            PlayerController player = players[i];
            if (gamepad.isConnected)
            {
                Debug.Log("player ajouté");
                gamepad.Enable();
                player.gamepad = gamepad;
                player.index = i;
                miniGameManager.RegisterPlayer(player);
            }
            else
            {
                gamepad.Disable();
                if(player) player.gameObject.SetActive(false);
            }
        }
    }
}
