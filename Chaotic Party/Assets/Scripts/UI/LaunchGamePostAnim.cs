using UnityEngine;

public class LaunchGamePostAnim : MonoBehaviour
{
    [SerializeField] private MiniGameManager _miniGameManager;

    private void Awake()
    {
        _miniGameManager ??= FindObjectOfType<MiniGameManager>();
    }

    public void LaunchMinigame()
    {
        _miniGameManager.StartMiniGame();
    }
}
