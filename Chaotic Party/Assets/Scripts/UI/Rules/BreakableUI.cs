using UnityEngine;

public abstract class BreakableUI : MonoBehaviour
{
    protected MiniGameManager _miniGameManager;
    
    private void Awake()
    {
        _miniGameManager ??= FindObjectOfType<MiniGameManager>();
        SetUp();
    }

    protected abstract void SetUp();
}
