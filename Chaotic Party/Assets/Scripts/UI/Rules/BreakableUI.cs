using UnityEngine;

public abstract class BreakableUI : MonoBehaviour
{
    protected MiniGameManager _miniGameManager;
    protected MultiplayerManager _multiplayerManager;
    
    private void Awake()
    {
        _miniGameManager ??= FindObjectOfType<MiniGameManager>();
        _multiplayerManager ??= FindObjectOfType<MultiplayerManager>();
        SetUp();
    }

    protected abstract void SetUp();
}
