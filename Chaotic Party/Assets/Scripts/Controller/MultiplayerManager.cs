using System;
using System.Collections;
using System.Collections.Generic;
using HinputClasses;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    public List<PlayerController> players;
    
    private void Awake()
    {
        for (int i = 0; i < Hinput.gamepad.Count; i++)
        {
            Gamepad gamepad = Hinput.gamepad[i];
            if (gamepad.isConnected)
            {
                gamepad.Enable();
                players[i].gamepad = gamepad;
            }
            else
            {
                gamepad.Disable();
            }
        }
    }
}
