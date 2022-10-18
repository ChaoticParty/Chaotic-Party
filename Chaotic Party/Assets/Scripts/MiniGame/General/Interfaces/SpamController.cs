using UnityEngine;

public abstract class SpamController : MonoBehaviour
{
    protected PlayerController player;
    public SpamManager spamManager;

    protected void Awake()
    {
        player ??= GetComponent<PlayerController>();
        spamManager ??= player.miniGameManager as SpamManager;
    }

    protected abstract void Click();
}
