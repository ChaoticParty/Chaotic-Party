using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spam : MonoBehaviour
{
    private PlayerController player;
    public SpamButton spamButton;
    public ISpamManager spamManager;
    
    private void Awake()
    {
        player = GetComponent<PlayerController>();
        spamManager = player.miniGameManager as ISpamManager;
        UnityEvent buttonEvent = new();
        switch (spamButton)
        {
            case SpamButton.A:
                buttonEvent = player.aJustPressed;
                break;
            case SpamButton.B:
                buttonEvent = player.bJustPressed;
                break;
            case SpamButton.X:
                buttonEvent = player.xJustPressed;
                break;
            case SpamButton.Y:
                buttonEvent = player.yJustPressed;
                break;
            case SpamButton.RightStick:
                buttonEvent = player.rightStickPressed;
                break;
            case SpamButton.LeftStick:
                buttonEvent = player.leftStickPressed;
                break;
            case SpamButton.Any:
                break;
            default:
                break;
        }
        buttonEvent.AddListener(Click);
    }

    private void Click()
    {
        spamManager.Click(player.index);
    }
}

public enum SpamButton
{
    A,
    B,
    X,
    Y,
    RightStick,
    LeftStick,
    Any
}