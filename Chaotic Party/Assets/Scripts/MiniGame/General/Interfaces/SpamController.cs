using UnityEngine;

public abstract class SpamController : MiniGameController
{
    public SpamManager spamManager;

    protected void Awake()
    {
        base.Awake();
        spamManager ??= player.miniGameManager as SpamManager;
    }

    protected abstract void Click();
}
