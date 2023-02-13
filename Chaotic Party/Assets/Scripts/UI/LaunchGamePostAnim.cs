using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchGamePostAnim : MonoBehaviour
{
    [SerializeField] private MiniGameManager _miniGameManager;

    public void LaunchMinigame()
    {
        _miniGameManager.StartMiniGame();
    }
}
