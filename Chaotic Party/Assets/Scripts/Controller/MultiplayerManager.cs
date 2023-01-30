using System.Collections.Generic;
using HinputClasses;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    public List<PlayerController> players;
    [SerializeField] private MiniGameManager miniGameManager;
    [SerializeField] private PlayersListSO playersListSo;
    [Space]
    [SerializeField] private bool isInMenu = false;
    
    private void Awake()
    {
        miniGameManager ??= FindObjectOfType<MiniGameManager>();
        playersListSo ??= ReferenceHolder.Instance.players;
        InitMultiplayer();
    }

    public void InitMultiplayer()
    {
        for (int i = 0; i < Hinput.gamepad.Count; i++)
        {
            Gamepad gamepad = Hinput.gamepad[i];
            //Debug.Log(i + " / "+gamepad.isConnected);
            if (players.Count <= i)
            {
                return;
            }
            PlayerController player = players[i];
            if (gamepad.isConnected)
            {
                gamepad.Enable();
                player.gamepad = gamepad;
                player.index = i;
                if (miniGameManager) miniGameManager.RegisterPlayer(player);
                if (isInMenu) continue;
                player.SetupSprite(playersListSo.players[i]);
            }
            else
            {
                gamepad.Disable();
                if(player) player.gameObject.SetActive(false);
            }
        }
    }
    
    public sbyte GamepadCount()
    {
        sbyte count = 0;
        for (int i = 0; i < Hinput.gamepad.Count; i++)
        {
            Gamepad gamepad = Hinput.gamepad[i];
            if (gamepad.isConnected)
            {
                count++;
            }
        }
        return count;
    }
}
